using System.Reflection;
using Comfort.Common;
using EFT;

namespace DragonDenDevTool.Utilities;

public static class MapIdUtil
{
    public static string GetCurrentMapId()
    {
        var gameWorld = Singleton<GameWorld>.Instance;
        if (!gameWorld) return "unknown";
        var type = gameWorld.GetType();
        var pLocId = type.GetProperty("LocationId", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var pLoc = type.GetProperty("Location", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        var value = pLocId != null ? pLocId.GetValue(gameWorld) : pLoc != null ? pLoc.GetValue(gameWorld) : null;
        var mapId = value != null ? value.ToString() : "";
        return string.IsNullOrWhiteSpace(mapId) ? "unknown" : mapId;
    }
}