using System.Reflection;
using Comfort.Common;
using DragonDenDevTool.Features.InRaid.Services;
using DragonDenDevTool.Features.InRaid.Systems;
using EFT;
using SPT.Reflection.Patching;

namespace DragonDenDevTool.Patches
{
    internal class NewGamePatch : ModulePatch
    {
        protected override MethodBase GetTargetMethod() =>
            typeof(GameWorld).GetMethod(nameof(GameWorld.OnGameStarted));

        [PatchPrefix]
        private static void PatchPrefix()
        {
            var gameWorld = Singleton<GameWorld>.Instance;
        
            if (gameWorld == null) return;
            var devTool = gameWorld.gameObject.AddComponent<DevToolUIController>();

            devTool.Init(
                new ItemService(),
                new PlayerService(),
                new BotService(),
                new ExfilService(),
                new HotspotService()
            );
        }
    }
}