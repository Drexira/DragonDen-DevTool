using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comfort.Common;
using DragonDenDevTool.Features.InRaid.Interfaces;
using DragonDenDevTool.Utilities;
using EFT;
using UnityEngine;

namespace DragonDenDevTool.Features.InRaid.Services;

public class BotService : IBotService
{
    public async Task<bool> SpawnBotAsync(WildSpawnType role, int amount)
    {
        try
        {
            var botGame = Singleton<IBotGame>.Instance;
            var botsController = botGame?.BotsController;
            var spawner = botsController?.BotSpawner;

            if (botsController == null || spawner == null || !botsController.IsEnable)
            {
                NotificationManagerClass.DisplayMessageNotification("[DevTool] Bot spawner not available.");
                return false;
            }

            amount = Mathf.Clamp(amount, 1, 50);
            NotificationManagerClass.DisplayMessageNotification($"[DevTool] Attempting to spawn {role} x{amount}, please wait...");

            var wave = new BotWaveDataClass
            {
                BotsCount = amount,
                Side = (EPlayerSide)4,
                SpawnAreaName = "",
                Time = 0f,
                WildSpawnType = role,
                IsPlayers = false,
                Difficulty = BotDifficulty.normal,
                ChanceGroup = 100f,
                WithCheckMinMax = false
            };

            await spawner.ActivateBotsByWave(wave);
            NotificationManagerClass.DisplayMessageNotification($"[DevTool] Spawned {role} x{amount}");
            await Task.CompletedTask;
            return true;
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError($"[DevTool] Error spawning bot: {e.Message}");
            NotificationManagerClass.DisplayMessageNotification("[DevTool] Spawn failed.. See Console.");
            return false;
        }
    }

    public List<(string id, string nick, string side)> ListBots(string filter)
    {
        var gameWorld = Singleton<GameWorld>.Instance;
        if (!gameWorld) return new List<(string, string, string)>();

        var q = (filter ?? "").Trim().ToLowerInvariant();

        return gameWorld.RegisteredPlayers
            .OfType<Player>()
            .Where(p => !p.IsYourPlayer && Match(p))
            .Select(p => (
                p.ProfileId ?? "",
                p.Profile?.Nickname ?? "",
                p.Profile?.Side.ToString() ?? ""
            ))
            .ToList();

        bool Match(Player p)
        {
            if (string.IsNullOrEmpty(q)) return true;
            if (!string.IsNullOrEmpty(p.ProfileId) && p.ProfileId.ToLowerInvariant().Contains(q)) return true;
            var nick = p.Profile?.Nickname ?? "";
            if (!string.IsNullOrEmpty(nick) && nick.ToLowerInvariant().Contains(q)) return true;
            var side = p.Profile?.Side.ToString() ?? "";
            return !string.IsNullOrEmpty(side) && side.ToLowerInvariant().Contains(q);
        }
    }

    public int RegisterBotCount()
    {
        var gameWorld = Singleton<GameWorld>.Instance;
        return gameWorld?.RegisteredPlayers?.Count ?? 0;
    }

    public void TeleportAllBotsToMe(Player player, float forwardOffset)
    {
        try
        {
            var gameWorld = Singleton<GameWorld>.Instance;
            if (!gameWorld || !player) return;
            foreach (var bot in gameWorld.RegisteredPlayers.OfType<Player>())
            {
                if (bot.IsYourPlayer) continue;
                TeleportUtils.SafeTeleport(player,bot);
            }

            NotificationManagerClass.DisplayMessageNotification("[DevTool] Teleported all bots to you.");
        }
        catch
        {
            /* good girl action */
        }
    }

    public void TeleportAllSpecific(EBotType type)
    {
        try
        {
            var gameWorld = Singleton<GameWorld>.Instance;
            if (!gameWorld) return;
            var player = gameWorld.MainPlayer;
            
            foreach (var bot in gameWorld.RegisteredPlayers.OfType<Player>())
            {
                if (bot.IsYourPlayer) return;
                var pInfo = bot.Profile.Info;
                if (!pInfo.EplayerSide_0.Equals(type)) continue;
                
                TeleportUtils.SafeTeleport(bot, player);
            }
        }
        catch
        {
            /* good girl action */
        }
    }

    public void TeleportBotToMeById(string playerId)
    {
        try
        {
            var gameWorld = Singleton<GameWorld>.Instance;
            var player = gameWorld?.MainPlayer;
            if (!gameWorld || !player) return;
            var target = gameWorld.RegisteredPlayers.FirstOrDefault(x => x is Player p && p.ProfileId == playerId) as Player;
            if (!target) return;
            TeleportUtils.SafeTeleport(player, target);
            NotificationManagerClass.DisplayMessageNotification("[DevTool] Teleported " + (target.Profile != null ? target.Profile.Nickname : target.ProfileId) + " to you.");
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError($"[DevTool] Error while teleporting {playerId} to you: {e.Message}");
            NotificationManagerClass.DisplayMessageNotification("[DevTool] Error while teleporting bot.. See Console");
        }
    }

    public void TeleportMeToBot(string playerId)
    {
        try
        {
            var gameWorld = Singleton<GameWorld>.Instance;
            var player = gameWorld?.MainPlayer;
            if (!gameWorld || !player) return;
            var target = gameWorld.RegisteredPlayers.FirstOrDefault(x => x is Player p && p.ProfileId == playerId) as Player;
            if (!target) return;
            TeleportUtils.SafeTeleport(target,player);
            NotificationManagerClass.DisplayMessageNotification("[DevTool] Teleported you to " + (target.Profile != null ? target.Profile.Nickname : target.ProfileId));
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError($"[DevTool] Error while teleporting you:  {e.Message}");
            NotificationManagerClass.DisplayMessageNotification("[DevTool] Error while teleporting you to you.. See Console.");
        }
    }

    public bool KillBotById(string playerId)
    {
        try
        {
            var gameWorld = Singleton<GameWorld>.Instance;
            var target = gameWorld?.RegisteredPlayers.FirstOrDefault(x => x is Player p && p.ProfileId == playerId) as Player;
            if (!target)
            {
                NotificationManagerClass.DisplayMessageNotification("[DevTool] Player not found.");
                return false;
            }

            var damage = new DamageInfoStruct { Damage = 9999f, DamageType = EDamageType.Bullet, StaminaBurnRate = 0f, BleedBlock = true };
            target.ApplyDamageInfo(damage, EBodyPart.Head, EBodyPartColliderType.HeadCommon, 0f);
            NotificationManagerClass.DisplayMessageNotification($"[DevTool] Killed {target.Profile.Nickname}.");
            return true;
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError($"[DevTool] Error while teleporting {playerId}: {e.Message}");
            NotificationManagerClass.DisplayMessageNotification("[DevTool] Kill failed.. See Console.");
            return false;
        }
    }

    public int KillAllBots()
    {
        try
        {
            var gameWorld = Singleton<GameWorld>.Instance;
            if (!gameWorld) return 0;
            var killed = 0;
            foreach (var player in gameWorld.RegisteredPlayers.OfType<Player>())
            {
                if (!player || player.IsYourPlayer) continue;
                try
                {
                    var damage = new DamageInfoStruct { Damage = 9999f, DamageType = EDamageType.Bullet, StaminaBurnRate = 0f, BleedBlock = true };
                    player.ApplyDamageInfo(damage, EBodyPart.Head, EBodyPartColliderType.HeadCommon, 0f);
                    killed++;
                }
                catch
                {
                    /* good girl action */
                }
            }

            NotificationManagerClass.DisplayMessageNotification($"[DevTool] Killed {killed} bots.");
            return killed;
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError($"[DevTool] Kill All failed:  {e.Message}");
            NotificationManagerClass.DisplayMessageNotification("[DevTool] Kill All failed.. See Console.");
            return 0;
        }
    }

    public int KillAllSpecificBots(EBotType type)
    {
        try
        {
            var gameWorld = Singleton<GameWorld>.Instance;
            if (!gameWorld) return 0;
            var killed = 0;
            foreach (var player in gameWorld.RegisteredPlayers.OfType<Player>())
            {
                if (!player || player.IsYourPlayer) continue;
                try
                {
                    var pInfo = player.Profile.Info;
                    if (type == EBotType.Pmcs && player.Profile.Side is EPlayerSide.Bear or EPlayerSide.Usec || 
                        type == EBotType.Scavs && player.Profile.Side == EPlayerSide.Savage || 
                        type == EBotType.Bosses && pInfo.Settings.IsBoss())
                    {
                        var damage = new DamageInfoStruct { Damage = 9999f, DamageType = EDamageType.Bullet, StaminaBurnRate = 0f, BleedBlock = true };
                        player.ApplyDamageInfo(damage, EBodyPart.Head, EBodyPartColliderType.HeadCommon, 0f);
                        killed++;
                    }
                }
                catch (Exception e)
                {
                    Plugin.Logger.LogError($"[DevTool] Error while killing {player.Profile.Nickname}: {e.Message}");
                    NotificationManagerClass.DisplayMessageNotification($"[DevTool] Killing  {player.Profile.Nickname} failed.. See Console."); 
                }
            }

            NotificationManagerClass.DisplayMessageNotification($"[DevTool] Killed {killed} {type.ToString()}.");
            return killed;
        }
        catch (Exception e)
        {
            Plugin.Logger.LogError($"[DevTool] Kill All {type.ToString()} failed:  {e.Message}");
            NotificationManagerClass.DisplayMessageNotification($"[DevTool] Kill All {type.ToString()} failed.. See Console.");
            return 0;
        }
    }

    public string[] GetAllRoleNames()
    {
        return Utils.AllRoleNames ?? Array.Empty<string>();
    }
}

public enum EBotType
{
    Pmcs,
    Scavs,
    Bosses
}