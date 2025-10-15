using System;
using System.Linq;
using DragonDenDevTool.Features.InRaid.Interfaces;
using DragonDenDevTool.UI;
using EFT.Interactive;
using EFT.Interactive.SecretExfiltrations;
using UnityEngine;

namespace DragonDenDevTool.Features.InRaid.Tabs;

public sealed class ExfilsTab : IDevTab, IDisposable
{
    readonly DDStyle _s;
    readonly IExfilService _exfils;

    Vector2 _scroll;
    string _filter = "";
    bool _showPmc = true;
    bool _showScav = true;
    bool _showSecret = true;

    ExfiltrationPoint[] _pmc = Array.Empty<ExfiltrationPoint>();
    ScavExfiltrationPoint[] _scav = Array.Empty<ScavExfiltrationPoint>();
    SecretExfiltrationPoint[] _secret = Array.Empty<SecretExfiltrationPoint>();

    public ExfilsTab(DDStyle s, IExfilService exfilService)
    {
        _s = s;
        _exfils = exfilService;
        RefreshData();
        _exfils.OnStatusChanged += OnAnyStatusChanged;
    }

    public string Title => "Exfils";

    public void Tick()
    {
        if (_pmc == null || _scav == null || _secret == null) RefreshData();
    }

    public void OnGUI()
    {
        using (new GUILayout.HorizontalScope())
        {
            GUILayout.Label("Filter", _s.SmallLabel, GUILayout.Width(42));
            _filter = GUILayout.TextField(_filter ?? "", _s.SearchBox, GUILayout.MinWidth(160), GUILayout.Height(27));
            _showPmc = GUILayout.Toggle(_showPmc, "PMC", _s.Button, GUILayout.Width(70), GUILayout.Height(27));
            _showScav = GUILayout.Toggle(_showScav, "Scav", _s.Button, GUILayout.Width(70), GUILayout.Height(27));
            _showSecret = GUILayout.Toggle(_showSecret, "Secret", _s.Button, GUILayout.Width(80), GUILayout.Height(27));
            if (GUILayout.Button("Refresh", _s.Button, GUILayout.Width(90), GUILayout.Height(27))) RefreshData();
            GUILayout.FlexibleSpace();
        }

        GUILayout.Space(6);
        
        GUILayout.Label("[Warning] Some extracts will make you fall under the ground!", _s.LabelWarning);
        
        _scroll = GUILayout.BeginScrollView(_scroll);

        if (_showPmc) DrawGroup("PMC Exfils", _pmc.ToArray());
        if (_showScav) DrawGroup("Scav Exfils", _scav?.Cast<ExfiltrationPoint>().ToArray());
        if (_showSecret) DrawGroup("Secret Exfils", _secret?.Cast<ExfiltrationPoint>().ToArray());

        GUILayout.EndScrollView();
    }

    void DrawGroup(string header, ExfiltrationPoint[] exfils)
    {
        if (exfils == null || exfils.Length == 0) return;

        GUILayout.Label(header, _s.H2);
        foreach (var exfil in exfils)
        {
            if (!exfil) continue;
            if (!_exfils.PassesFilter(exfil, _filter)) continue;

            using (new GUILayout.VerticalScope(_s.Pill))
            {
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label(exfil.Settings?.Name ?? "?", _s.Label, GUILayout.ExpandWidth(true));
                    if (exfil.Settings?.Id != String.Empty)
                        GUILayout.Label("(" + exfil.Settings?.Id + ")", _s.Label, GUILayout.ExpandWidth(true));
                    GUILayout.Label(_exfils.ExfilStatusToText(exfil.Status), _s.SmallLabel, GUILayout.Width(140));
                    if (GUILayout.Button("Teleport", _s.Button, GUILayout.Width(100), GUILayout.Height(27)))
                    {
                        _exfils.TeleportToExfil(exfil.transform.position + Vector3.up * 0.15f);
                    }
                }

                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label("Type: " + exfil.Settings?.ExfiltrationType, _s.SmallLabel, GUILayout.Width(220));
                    GUILayout.Label("Reusable: " + exfil.Reusable, _s.SmallLabel, GUILayout.Width(120));
                    GUILayout.Label("Chance: " + exfil.Settings?.Chance.ToString("0.#") + "%", _s.SmallLabel, GUILayout.Width(120));
                    GUILayout.Label("Queued: " + exfil.QueuedPlayers.Count, _s.SmallLabel, GUILayout.Width(100));
                    GUILayout.FlexibleSpace();
                }

                using (new GUILayout.HorizontalScope())
                {
                    var pos = exfil.transform.position;
                    GUILayout.Label("Pos: " + $"{pos.x:0.0}, {pos.y:0.0}, {pos.z:0.0}", _s.SmallLabel, GUILayout.Width(260));
                    GUILayout.Label("Start: " + exfil.Settings?.StartTime, _s.SmallLabel, GUILayout.Width(120));
                    GUILayout.Label("Time: " + exfil.Settings?.ExfiltrationTime, _s.SmallLabel, GUILayout.Width(120));
                    if (exfil.Status == EExfiltrationStatus.Countdown)
                        GUILayout.Label("Started: " + exfil.ExfiltrationStartTime.ToString("0.0") + "s", _s.SmallLabel, GUILayout.Width(140));
                    GUILayout.FlexibleSpace();
                }

                if (exfil.EligibleEntryPoints != null && exfil.EligibleEntryPoints.Length > 0)
                    GUILayout.Label("Entry Points: " + string.Join(", ", exfil.EligibleEntryPoints), _s.SmallLabel);

                var tips = _exfils.GetTips(exfil);
                foreach (var tip in tips)
                    GUILayout.Label("Tip: " + tip, _s.SmallLabel);
            }
        }

        GUILayout.Space(8);
    }

    void RefreshData()
    {
        var sets = _exfils.GetCurrentExfil();
        _pmc = sets.pmc ?? Array.Empty<ExfiltrationPoint>();
        _scav = sets.scav ?? Array.Empty<ScavExfiltrationPoint>();
        _secret = sets.secret ?? Array.Empty<SecretExfiltrationPoint>();
    }

    void OnAnyStatusChanged(ExfiltrationPoint _)
    {
        RefreshData();
    }

    public void Dispose()
    {
        _exfils.OnStatusChanged -= OnAnyStatusChanged;
    }
}