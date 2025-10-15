using EFT;
using UnityEngine;

namespace DragonDenDevTool.Utilities;
public static class TeleportUtils
{
    public static void SafeTeleport(Player player, Vector3 pos, float yLift = 0.15f, bool onServerToo = false)
    {
        if (!player) return;
        player.Teleport(pos, onServerToo);
    }
    public static void SafeTeleport(Player player, Player target, float yLift = 0.15f, bool onServerToo = false)
    {
        if (!player) return;
        var playerPosition = player.Position;
        var forward = player.Transform.forward;
        const float posSpread = 0.55f;
        const float fwdOffset = 0.50f;
        
        var pos = playerPosition
                  + forward * fwdOffset
                  + RandXZ(posSpread)
                  + new Vector3();
        
        target.Teleport(pos, onServerToo);
        return;

        Vector3 RandXZ(float radius)
        {
            var v = Random.insideUnitCircle * radius;
            return new Vector3(v.x, 0f, v.y);
        }
    }
}