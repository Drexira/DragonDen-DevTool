using DragonDenDevTool.Features.InRaid.Interfaces;
using DragonDenDevTool.Features.InRaid.Services;
using DragonDenDevTool.UI;
using DragonDenDevTool.Utilities;
using UnityEngine;

namespace DragonDenDevTool.Features.InRaid.Tabs;

public sealed class PlayerTab : IDevTab
{
    readonly DDStyle _s;
    readonly IPlayerService _players;

    Vector2 _scroll;
    bool _godMode;
    bool _noFall;
    bool _instantSearch;
    bool _infiniteStamina;

    float _nextResolveAt;
    Structs.RaidInfo _raid;
    Structs.PlayerInfo _pinfo;
    Structs.VitalsInfo _vitals;

    public PlayerTab(DDStyle s, IPlayerService players)
    {
        _s = s;
        _players = players ?? new PlayerService();
        _godMode = true;
        _players.SetGodMode(true);
        _players.ApplyToggles();
    }

    public string Title => "Player";

    public void Tick()
    {
        if (Time.realtimeSinceStartup >= _nextResolveAt)
        {
            _nextResolveAt = Time.realtimeSinceStartup + Settings.UpdateInterval.Value;
            _raid = _players.RaidInfo();
            _pinfo = _players.PlayerInfo();
            _vitals = _players.VitalsInfo();
            _players.ApplyToggles();
        }
    }

    public void OnGUI()
    {
        _s.BuildIfNeeded();
        GUILayout.Space(3);
        _scroll = GUILayout.BeginScrollView(_scroll);
        DrawRaidPanel();
        GUILayout.Space(4);
        DrawPlayerPanel();
        GUILayout.Space(4);
        DrawTogglesPanel();
        GUILayout.Space(5);
        DrawActionsPanel();
        GUILayout.Space(5);
        DrawVitalsPanel();
        GUILayout.Space(4);
        GUILayout.EndScrollView();
    }

    void DrawRaidPanel()
    {
        GUILayout.Label("Raid", _s.H2);
        using (new GUILayout.VerticalScope(_s.Pill))
        {
            using (new GUILayout.HorizontalScope())
            {
                Cell("Map", _raid.Map, 100);
                Cell("Time", $"{_raid.Min:00}:{_raid.Sec:00}", 100);
                Cell("Players", _raid.Total.ToString(), 120);
                Cell("PMC", _raid.PMC.ToString(), 100);
                Cell("Scavs", _raid.Scavs.ToString(), 100);
                GUILayout.FlexibleSpace();
            }
        }
    }

    void DrawPlayerPanel()
    {
        GUILayout.Label("Player Info", _s.H2);
        using (new GUILayout.VerticalScope(_s.Pill))
        {
            using (new GUILayout.HorizontalScope())
            {
                Cell("Name", _pinfo.Name, 240);
                Cell("Side", _pinfo.Side, 120);
                Cell("Level", _pinfo.Level, 100);
                GUILayout.FlexibleSpace();
            }

            using (new GUILayout.HorizontalScope())
            {
                Cell("Pos X", _pinfo.Pos.x.ToString("0.00"), 120);
                Cell("Pos Y", _pinfo.Pos.y.ToString("0.00"), 120);
                Cell("Pos Z", _pinfo.Pos.z.ToString("0.00"), 120);
                Cell("Rotation", _pinfo.Yaw.ToString("0.0") + "°", 120);
                GUILayout.FlexibleSpace();
            }

            GUILayout.Space(2);
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Copy Position", _s.Button, GUILayout.Width(140)))
                {
                    var p = _pinfo.Pos;
                    GUIUtility.systemCopyBuffer = $"{p.x:0.000}, {p.y:0.000}, {p.z:0.000}";
                }

                GUILayout.FlexibleSpace();
            }
        }
    }

    void DrawTogglesPanel()
    {
        GUILayout.Label("Protections & Modifiers", _s.ColumnHeader);
        using (new GUILayout.VerticalScope(_s.Pill))
        {
            using (new GUILayout.HorizontalScope())
            {
                var godMode = GUILayout.Toggle(_godMode, "God Mode", _s.Button, GUILayout.Width(200));
                if (godMode != _godMode)
                {
                    _godMode = godMode;
                    _players.SetGodMode(_godMode);
                }

                var noFall = GUILayout.Toggle(_noFall, "No Fall Damage", _s.Button, GUILayout.Width(160));
                if (noFall != _noFall)
                {
                    _noFall = noFall;
                    _players.SetNoFall(_noFall);
                }

                GUILayout.FlexibleSpace();
            }

            GUILayout.Space(4);
            using (new GUILayout.HorizontalScope())
            {
                var instantSearch = GUILayout.Toggle(_instantSearch, "Instant Search", _s.Button, GUILayout.Width(160));
                if (instantSearch != _instantSearch)
                {
                    _instantSearch = instantSearch;
                    _players.SetInstantSearch(_instantSearch);
                }

                var infiniteStamina = GUILayout.Toggle(_infiniteStamina, "Infinite Stamina", _s.Button, GUILayout.Width(180));
                if (infiniteStamina != _infiniteStamina)
                {
                    _infiniteStamina = infiniteStamina;
                    _players.SetInfiniteStamina(_infiniteStamina);
                }

                GUILayout.FlexibleSpace();
            }
        }
    }

    void DrawActionsPanel()
    {
        GUILayout.Label("Health Actions", _s.H2);
        using (new GUILayout.VerticalScope(_s.Pill))
        {
            using (new GUILayout.HorizontalScope())
            {
                if (GUILayout.Button("Full Heal", _s.Button, GUILayout.Width(120)))
                    _players.FullHealAll();

                if (GUILayout.Button("Clear Negative Effects", _s.Button, GUILayout.Width(160)))
                    _players.ClearNegativeEffects();

                if (GUILayout.Button("Refill Hydration", _s.Button, GUILayout.Width(150)))
                    _players.AddHydration(999f);

                if (GUILayout.Button("Refill Energy", _s.Button, GUILayout.Width(130)))
                    _players.AddEnergy(999f);

                GUILayout.FlexibleSpace();
            }
        }
    }

    void DrawVitalsPanel()
    {
        GUILayout.Label("Vitals", _s.H2);
        using (new GUILayout.VerticalScope(_s.Pill))
        {
            using (new GUILayout.HorizontalScope())
            {
                Cell("HP", $"{_vitals.HPCurrent:0}/{_vitals.HPMax:0}", 160);
                Cell("Energy", $"{_vitals.EnergyCurrent:0}/{_vitals.EnergyMax:0}", 180);
                Cell("Hydration", $"{_vitals.HydrationCurrent:0}/{_vitals.HydrationMax:0}", 200);
            }

            GUILayout.Space(2);

            using (new GUILayout.HorizontalScope())
            {
                Cell("Body Temp", _vitals.BodyTemp.ToString("0.0") + "°C", 160);
                Cell("DamageCoeff", _vitals.DamageCoeff.ToString("0.00"), 180);
            }
        }
    }

    void Cell(string label, string value, float width)
    {
        using (new GUILayout.VerticalScope(GUILayout.Width(width)))
        {
            GUILayout.Label(label, _s.SmallLabel);
            GUILayout.Label(value ?? "—", _s.Label);
        }
    }
}
