using System;
using System.Collections.Generic;
using System.Linq;
using BepInEx;
using Comfort.Common;
using DragonDenDevTool.Features.InRaid.Interfaces;
using DragonDenDevTool.Features.InRaid.Services;
using DragonDenDevTool.Features.InRaid.Tabs;
using DragonDenDevTool.UI;
using DragonDenDevTool.Utilities;
using EFT;
using UnityEngine;

namespace DragonDenDevTool.Features.InRaid.Systems;

public class DevToolUIController : MonoBehaviour
{
    IItemService _items;
    IPlayerService _players;
    IBotService _bots;
    IExfilService _exfils;
    IHotspotService _hotspot;

    DDStyle _s = new DDStyle();

    Rect _main = new Rect(80, 80, 900, 600);
    Vector2 _scroll;
    int _tabIndex;

    readonly List<IDevTab> _tabs = new List<IDevTab>();
    bool _mouseOverWindow;

    PlayerRotateBlocker _rotateBlocker;
    Player _cachedPlayer;
    bool _wasUIOpen;

    public void Init(IItemService items, IPlayerService players, IBotService bots, IExfilService exfils, IHotspotService hotspot)
    {
        _items = items ?? new ItemService();
        _players = players ?? new PlayerService();
        _bots = bots ?? new BotService();
        _exfils = exfils ?? new ExfilService();
        _hotspot = hotspot ?? new HotspotService();
    }

    void Awake()
    {
        _items ??= new ItemService();
        _players ??= new PlayerService();
        _bots ??= new BotService();
        _exfils ??= new ExfilService();
        _hotspot ??= new HotspotService();

        CursorUtils.Initialize();
        _cachedPlayer = ResolvePlayer();

        AddTab(new PlayerTab(_s, _players));
        AddTab(new ItemsTab(_s, _items));
        AddTab(new BotsTab(_s, _bots));
        AddTab(new ExfilsTab(_s, _exfils));
        AddTab(new HotspotsTab(_s, _hotspot));

        if (Settings.DebugMode.Value) AddTab(new HotspotMakerTab(_s));
        else RemoveTabByTitle("Hotspot Maker");

        Settings.DebugMode.Subscribe(value =>
        {
            if (value) AddTab(new HotspotMakerTab(_s));
            else RemoveTabByTitle("Hotspot Maker");
        });
    }

    void Update()
    {
        if (Settings.IsKeyPressed(Settings.ToggleUI.Value))
        {
            IsUIOpen = !IsUIOpen;
        }

        if (_wasUIOpen != IsUIOpen)
        {
            if (IsUIOpen)
            {
                if (!_cachedPlayer)
                {
                    _cachedPlayer = ResolvePlayer();
                }

                if (_rotateBlocker == null && _cachedPlayer)
                {
                    _rotateBlocker = new PlayerRotateBlocker(_cachedPlayer);
                }

                _rotateBlocker?.Lock();
            }
            else
            {
                _rotateBlocker?.Unlock();
            }

            _wasUIOpen = IsUIOpen;
        }

        if (!IsUIOpen) return;

        CursorUtils.ApplyState(0, true);

        var guiMouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        _mouseOverWindow = _main.Contains(guiMouse);

        if (_tabs.Count == 0) return;
        _tabIndex = Mathf.Clamp(_tabIndex, 0, _tabs.Count - 1);
        _tabs[_tabIndex].Tick();
    }

    void LateUpdate()
    {
        if (IsUIOpen) CursorUtils.ApplyState(0, true);
    }

    void OnGUI()
    {
        if (!IsUIOpen) return;

        _s.BuildIfNeeded();

        GUI.depth = 0;
        _main = GUI.Window(777, _main, MainWindowFunc, "", _s.Win);

        if (!_mouseOverWindow) return;

        if (Event.current.type == EventType.MouseDown ||
            Event.current.type == EventType.MouseUp ||
            Event.current.type == EventType.ScrollWheel)
        {
            UnityInput.Current.ResetInputAxes();
        }
    }

    void MainWindowFunc(int id)
    {
        GUILayout.BeginHorizontal(GUILayout.Height(30));
        GUILayout.Label("  DragonDen Dev Tool - Raid", _s.HeaderBar, GUILayout.ExpandWidth(true));
        GUILayout.EndHorizontal();
        var headerRect = GUILayoutUtility.GetLastRect();
        GUI.DragWindow(headerRect);

        if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape)
        {
            IsUIOpen = false;
            _rotateBlocker?.Unlock();
            _wasUIOpen = false;
            return;
        }

        GUILayout.Space(6);
        using (new GUILayout.HorizontalScope())
        {
            for (var i = 0; i < _tabs.Count; i++)
            {
                var st = (_tabIndex == i) ? _s.TabOn : _s.TabOff;
                if (!GUILayout.Button(_tabs[i].Title, st, GUILayout.Height(27))) continue;

                _tabIndex = i;
                GuiHelpers.ConsumeMouseClick();
            }

            GUILayout.FlexibleSpace();
        }

        GUILayout.Space(6);
        _scroll = GUILayout.BeginScrollView(_scroll);
        if (_tabs.Count > 0)
            _tabs[_tabIndex].OnGUI();
        GUILayout.EndScrollView();
    }

    static bool IsUIOpen
    {
        get => CursorUtils.IsUiOpen;
        set => CursorUtils.IsUiOpen = value;
    }

    void OnDestroy()
    {
        _rotateBlocker?.Unlock();
        _rotateBlocker = null;
        _cachedPlayer = null;
        _wasUIOpen = false;
    }

    static Player ResolvePlayer()
    {
        var gameWorld = Singleton<GameWorld>.Instance;
        if (gameWorld && gameWorld.MainPlayer) return gameWorld.MainPlayer;
        var all = FindObjectsOfType<Player>();
        return all.Length > 0 ? all[0] : null;
    }

    void AddTab(IDevTab tab)
    {
        if (tab == null) return;
        if (_tabs.Any(t => string.Equals(t.Title, tab.Title, StringComparison.OrdinalIgnoreCase)))
        {
            return;
        }

        _tabs.Add(tab);
        _tabIndex = Mathf.Clamp(_tabIndex, 0, _tabs.Count - 1);
    }

    void RemoveTabByTitle(string title)
    {
        if (string.IsNullOrEmpty(title)) return;
        var tabIndex = -1;
        for (int i = 0; i < _tabs.Count; i++)
            if (string.Equals(_tabs[i].Title, title, StringComparison.OrdinalIgnoreCase))
            {
                tabIndex = i;
                break;
            }

        if (tabIndex < 0) return;
        _tabs.RemoveAt(tabIndex);
        _tabIndex = Mathf.Clamp(_tabIndex, 0, Mathf.Max(0, _tabs.Count - 1));
    }
}