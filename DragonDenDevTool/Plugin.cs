using BepInEx;
using BepInEx.Logging;
using DragonDenDevTool.Patches;
using DragonDenDevTool.Utilities;

namespace DragonDenDevTool;

[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
[BepInDependency("com.SPT.custom", "4.0")]
public class Plugin : BaseUnityPlugin
{
    public new static ManualLogSource Logger { get; set; }

    public void Awake()
    {
        Logger ??= BepInEx.Logging.Logger.CreateLogSource("DragonDenDevTool");

        Settings.Init(Config);
        PatchManager.EnablePatches();
    }
}