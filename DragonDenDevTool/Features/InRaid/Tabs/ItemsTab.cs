using System;
using System.Linq;
using Comfort.Common;
using DragonDenDevTool.Features.InRaid.Interfaces;
using DragonDenDevTool.Features.InRaid.Services;
using DragonDenDevTool.UI;
using DragonDenDevTool.Utilities;
using EFT;
using EFT.InventoryLogic;
using UnityEngine;

namespace DragonDenDevTool.Features.InRaid.Tabs;

public sealed class ItemsTab : IDevTab
{
    readonly DDStyle _s;
    readonly IItemService _items;

    string _search = "";
    ItemTemplate[] _results = Array.Empty<ItemTemplate>();
    bool _hasSearched;
    int _page, _pageSize = 30;
    string[] _amtBuf = Array.Empty<string>();

    public ItemsTab(DDStyle s, IItemService items)
    {
        _s = s;
        _items = items ?? new ItemService();
    }

    public string Title => "Items";

    public void Tick()
    {
    }

    public void OnGUI()
    {
        GUILayout.BeginHorizontal();

        GUI.SetNextControlName("ItemsSearch");
        _search = GUILayout.TextField(_search ?? "", _s.TextBox, GUILayout.Width(260), GUILayout.Height(27));

        if (GUILayout.Button("Search", _s.Button, GUILayout.Width(90), GUILayout.Height(27)))
        {
            _page = 0;
            var query = _search ?? "";
            _hasSearched = !string.IsNullOrWhiteSpace(query);
            _results = _items.SearchItems(query, 1000000);
            RefreshPage();
            GuiHelpers.ConsumeMouseClick();
        }

        if (GUILayout.Button("Clear", _s.Button, GUILayout.Width(60), GUILayout.Height(27)))
        {
            _hasSearched = false;
            _search = "";
            _page = 0;
            _results = Array.Empty<ItemTemplate>();
            RefreshPage();
            GuiHelpers.ConsumeMouseClick();
        }

        var e = Event.current;
        if (e != null && (e.type == EventType.KeyDown || e.type == EventType.KeyUp) && (e.keyCode == KeyCode.Return || e.keyCode == KeyCode.KeypadEnter))
        {
            if (GUI.GetNameOfFocusedControl() == "ItemsSearch")
            {
                GUI.FocusControl(string.Empty);
                GUIUtility.keyboardControl = 0;
                _page = 0;
                var query = _search ?? "";
                _hasSearched = !string.IsNullOrWhiteSpace(query);
                _results = _items.SearchItems(query, 1000000);
                RefreshPage();
                e.Use();
            }
        }

        GUILayout.Space(6);
        var total = _results.Length;
        var totalPages = Mathf.Max(1, Mathf.CeilToInt(total / (float)_pageSize));
        GUILayout.Label($"Results: {total}    Page {_page + 1}/{totalPages}", _s.SmallLabel, GUILayout.Width(170));
        GUI.enabled = _page > 0;
        if (GUILayout.Button("< Prev", _s.Button, GUILayout.Width(76), GUILayout.Height(27)))
        {
            _page = Mathf.Max(0, _page - 1);
            RefreshPage();
            GuiHelpers.ConsumeMouseClick();
        }

        GUI.enabled = _page < (totalPages - 1);
        if (GUILayout.Button("Next >", _s.Button, GUILayout.Width(76), GUILayout.Height(27)))
        {
            _page = Mathf.Min(totalPages - 1, _page + 1);
            RefreshPage();
            GuiHelpers.ConsumeMouseClick();
        }

        GUI.enabled = true;
        GUILayout.EndHorizontal();

        GUILayout.Space(6);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Name", _s.ColumnHeader, GUILayout.Width(360));
        GUILayout.Label("ID", _s.ColumnHeader, GUILayout.Width(260));
        GUILayout.Label("Action", _s.ColumnHeader, GUILayout.Width(220));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        var slice = _results.Skip(_page * _pageSize).Take(_pageSize)
            .Select(t => (name: LocalizationUtils.GetBestItemLabel(t), id: TemplateIdUtil.GetId(t))).ToArray();
        EnsureAmtBuf(slice.Length);

        for (var i = 0; i < slice.Length; i++)
        {
            var it = slice[i];
            GUILayout.BeginHorizontal(_s.Pill ?? GUI.skin.box);
            GUILayout.Label(it.name, _s.Label, GUILayout.Width(360));
            GUILayout.Label(it.id, _s.SmallLabel, GUILayout.Width(200));

            _amtBuf[i] = GUILayout.TextField(_amtBuf[i], _s.TextBox, GUILayout.Width(60), GUILayout.Height(27));
            if (GUILayout.Button("Drop Amount", _s.Button, GUILayout.Width(110), GUILayout.Height(27)))
            {
                var mp = Singleton<GameWorld>.Instance?.MainPlayer;
                if (mp)
                {
                    if (!int.TryParse(_amtBuf[i], out var a)) a = 1;
                    a = Mathf.Clamp(a, 1, 1000000);
                    _ = _items.DropItemAsync(it.id, mp, a);
                }

                GuiHelpers.ConsumeMouseClick();
            }

            if (GUILayout.Button("Drop 1", _s.Button, GUILayout.Width(80), GUILayout.Height(27)))
            {
                var player = Singleton<GameWorld>.Instance?.MainPlayer;
                if (player) _ = _items.DropItemAsync(it.id, player);
                GuiHelpers.ConsumeMouseClick();
            }

            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }

        if (slice.Length != 0 || !_hasSearched) return;
        GUILayout.Space(8);
        GUILayout.Label("No items found. Try a different search.", _s.SmallLabel);
    }

    void RefreshPage()
    {
        var sliceCount = _results.Skip(_page * _pageSize).Take(_pageSize).Count();
        EnsureAmtBuf(sliceCount);
        for (var i = 0; i < sliceCount; i++)
        {
            if (!string.IsNullOrEmpty(_amtBuf[i])) continue;
            var idx = _page * _pageSize + i;
            var tpl = _results[idx];
            var id = TemplateIdUtil.GetId(tpl);
            var max = 1;
            try
            {
                max = _items.GetStackMax(id);
            }
            catch
            {
                /* good girl action */
            }

            _amtBuf[i] = Math.Max(1, max).ToString();
        }
    }

    void EnsureAmtBuf(int n)
    {
        if (_amtBuf.Length != n) _amtBuf = Enumerable.Repeat("1", Math.Max(0, n)).ToArray();
    }
}