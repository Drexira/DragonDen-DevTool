using System;
using UnityEngine;

namespace DragonDenDevTool.Utilities;

[Serializable]
public class Hotspot
{
    public string Id;
    public string MapId;
    public string Name;
    public string Category;
    public Vector3 Position;
    public bool IsGlobal;
}