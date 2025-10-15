using UnityEngine;

namespace DragonDenDevTool.Utilities;

public class Structs
{
    public struct RaidInfo
    {
        public string Map;
        public int Total;
        public int PMC;
        public int Scavs;
        public int Min;
        public int Sec;
    }

    public struct PlayerInfo
    {
        public string Name;
        public string Side;
        public string Level;
        public Vector3 Pos;
        public float Yaw;
    }

    public struct VitalsInfo
    {
        public float HPCurrent;
        public float HPMax;
        public float EnergyCurrent;
        public float EnergyMax;
        public float HydrationCurrent;
        public float HydrationMax;
        public float BodyTemp;
        public float DamageCoeff;
    }
}