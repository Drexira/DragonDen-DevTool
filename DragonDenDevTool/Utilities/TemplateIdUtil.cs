using System.Reflection;
using EFT.InventoryLogic;

namespace DragonDenDevTool.Utilities;

public static class TemplateIdUtil
{
    public static string GetId(ItemTemplate template)
    {
        if (template == null) return "";
        var type = template.GetType();

        var field = type.GetField("_id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (field != null)
        {
            var value = field.GetValue(template);
            return value?.ToString() ?? "";
        }

        var property = type.GetProperty("_id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                 ?? type.GetProperty("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (property != null)
        {
            var value = property.GetValue(template, null);
            return value?.ToString() ?? "";
        }

        field = type.GetField("Id", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        if (field != null)
        {
            var value = field.GetValue(template);
            return value?.ToString() ?? "";
        }

        return "";
    }
}