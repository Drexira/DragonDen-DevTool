using BepInEx;
using UnityEngine;

namespace DragonDenDevTool.Utilities;

public static class GuiHelpers
{
    public static void ConsumeMouseClick()
    {
        UnityInput.Current.ResetInputAxes();
    }

    public static bool IsMouseOver(Rect r)
    {
        var guiMouse = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
        return r.Contains(guiMouse);
    }
}