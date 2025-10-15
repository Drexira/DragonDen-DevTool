using System;
using System.IO;

namespace DragonDenDevTool.Utilities;

public static class DevPaths
{
    static readonly string PluginsDir;

    static DevPaths()
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var rootTry = Path.Combine(baseDir, "BepInEx");
        var bepRoot = Directory.Exists(rootTry) ? rootTry : baseDir;
        PluginsDir = Path.Combine(bepRoot, "plugins", "DragonDenDevTool");
    }

    public static string RootFolder => PluginsDir;
    public static string HotspotsFavoritesFile => Path.Combine(PluginsDir, "hotspots_favourites.json");
    public static string HotspotsGlobalFile => Path.Combine(PluginsDir, "hotspots_global.json");

    public static void EnsureFolders()
    {
        if (!Directory.Exists(PluginsDir)) Directory.CreateDirectory(PluginsDir);
    }
}