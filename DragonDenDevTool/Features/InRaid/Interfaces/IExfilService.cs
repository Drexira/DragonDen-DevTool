using System;
using EFT.Interactive;
using EFT.Interactive.SecretExfiltrations;
using UnityEngine;

namespace DragonDenDevTool.Features.InRaid.Interfaces;

public interface IExfilService
{
    (ExfiltrationPoint[] pmc, ScavExfiltrationPoint[] scav, SecretExfiltrationPoint[] secret) GetCurrentExfil();

    void TeleportToExfil(Vector3 target);

    string ExfilStatusToText(EExfiltrationStatus s);

    bool PassesFilter(ExfiltrationPoint p, string filter);

    string[] GetTips(ExfiltrationPoint p);

    event Action<ExfiltrationPoint> OnStatusChanged;
}