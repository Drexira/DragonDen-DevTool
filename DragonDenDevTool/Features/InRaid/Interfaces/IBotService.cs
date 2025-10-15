using System.Collections.Generic;
using System.Threading.Tasks;
using DragonDenDevTool.Features.InRaid.Services;
using EFT;
using UnityEngine;

namespace DragonDenDevTool.Features.InRaid.Interfaces;

public interface IBotService
{
    Task<bool> SpawnBotAsync(WildSpawnType role, int amount);
    List<(string id, string nick, string side)> ListBots(string filter);
    int RegisterBotCount();
    void TeleportAllBotsToMe(Player player, float forwardOffset);
    void TeleportAllSpecific(EBotType type);
    void TeleportMeToBot(string playerId);
    void TeleportBotToMeById(string playerId);
    bool KillBotById(string playerId);
    int KillAllBots();
    int KillAllSpecificBots(EBotType type);
    string[] GetAllRoleNames();
}