using DragonDenDevTool.Utilities;
using EFT;

namespace DragonDenDevTool.Features.InRaid.Interfaces;

public interface IPlayerService
{
    void TeleportAllTo(Player anchor, float forwardOffset);
    bool TryKill(Player target);
    Structs.RaidInfo RaidInfo();
    Structs.PlayerInfo PlayerInfo();
    Structs.VitalsInfo VitalsInfo();
    void SetGodMode(bool on);
    void SetNoFall(bool on);
    void SetInstantSearch(bool on);
    void SetInfiniteStamina(bool on);
    void ApplyToggles();
    void FullHealAll();
    void ClearNegativeEffects();
    void AddEnergy(float amount);
    void AddHydration(float amount);
}