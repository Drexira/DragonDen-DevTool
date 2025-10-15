using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using EFT;
using EFT.Interactive;
using EFT.InventoryLogic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DragonDenDevTool.Utilities;

public static class Utils
{
    public static readonly Regex PriceWithOld = new Regex(@"\((\d+)\)\s*<\s*s\s*>(\d+)<\s*/\s*s\s*>\s*\)",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

    public static readonly Regex UnityTags = new Regex(@"<\s*/?\s*(s|b|i|u|color|size|material|link|quad)\b[^>]*>",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);

    public static readonly Regex MultiSpace = new Regex(@"\s{2,}", 
        RegexOptions.CultureInvariant | RegexOptions.Compiled);


    public static Vector3 GetLookPoint(float upOffset, float aheadIfNoHit)
    {
        var cam = ActiveCamera();
        var origin = cam != null ? cam.transform.position : Vector3.zero;
        var dir = cam != null ? cam.transform.forward : Vector3.forward;
        var ray = new Ray(origin, dir);
        if (Physics.Raycast(ray, out var hit, 200f, ~0, QueryTriggerInteraction.Ignore))
            return hit.point + Vector3.up * upOffset;
        return origin + dir.normalized * aheadIfNoHit + Vector3.up * upOffset;
    }

    public static Camera ActiveCamera()
    {
        var t = AppDomain.CurrentDomain.GetAssemblies().SelectMany(a => a.GetTypes()).FirstOrDefault(x => x.Name == "GameCamera");
        if (t != null)
        {
            var p = t.GetProperty("instance", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            var inst = p != null ? p.GetValue(null) as MonoBehaviour : null;
            if (inst != null) return inst.GetComponent<Camera>();
        }

        if (Camera.main != null) return Camera.main;
        var cams = Object.FindObjectsOfType<Camera>();
        return cams.FirstOrDefault();
    }

    public static LootItem CreateLootItem(Item item, Vector3 pos, Quaternion rot)
    {
        var go = new GameObject("Loose_" + item.ShortName);
        go.transform.position = pos;
        go.transform.rotation = rot;
        var loot = go.AddComponent<LootItem>();
        var rfi = typeof(LootItem).GetField("Item", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (rfi != null) rfi.SetValue(loot, item);
        return loot;
    }
    
    public static string[] AllRoleNames
    {
        get
        {
            try
            {
                var names = Enum.GetNames(typeof(WildSpawnType))
                    .Where(n => !string.IsNullOrEmpty(n))
                    .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
                    .ToArray();

                Plugin.Logger?.LogInfo($"[Bots] Enum.GetNames(WildSpawnType) => count={names.Length}");
                if (names.Length > 0)
                    return names;
            }
            catch (Exception ex)
            {
                Plugin.Logger?.LogError($"[Bots] Enum.GetNames(WildSpawnType) threw: {ex}");
            }

            var fallback = new[]
            {
                "assault", "marksman", "bossTest", "bossBully", "followerTest", "followerBully",
                "bossKilla", "bossKojaniy", "followerKojaniy", "pmcBot", "cursedAssault", "bossGluhar",
                "followerGluharAssault", "followerGluharSecurity", "followerGluharScout", "followerGluharSnipe",
                "followerSanitar", "bossSanitar", "test", "assaultGroup", "sectantWarrior", "sectantPriest",
                "bossTagilla", "followerTagilla", "exUsec", "gifter", "bossKnight", "followerBigPipe", "followerBirdEye",
                "bossZryachiy", "followerZryachiy", "bossBoar", "followerBoar", "arenaFighter", "arenaFighterEvent",
                "bossBoarSniper", "crazyAssaultEvent", "peacefullZryachiyEvent", "sectactPriestEvent", "ravangeZryachiyEvent",
                "followerBoarClose1", "followerBoarClose2", "bossKolontay", "followerKolontayAssault", "followerKolontaySecurity",
                "shooterBTR", "bossPartisan", "spiritWinter", "spiritSpring", "peacemaker", "pmcBEAR", "pmcUSEC", "skier",
                "sectantPredvestnik", "sectantPrizrak", "sectantOni", "infectedAssault", "infectedPmc", "infectedCivil",
                "infectedLaborant", "infectedTagilla", "bossTagillaAgro", "bossKillaAgro", "tagillaHelperAgro"
            };
            Plugin.Logger?.LogWarning($"[Bots] Using fallback role list (count={fallback.Length}).");
            return fallback;
        }
    }
}