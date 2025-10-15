using System.Reflection;

namespace DragonDenDevTool.Utilities;

public class ReflectionHelper
{
    public static string SafeProp(object obj, string prop)
    {
        if (obj == null) return null;
        try
        {
            var pi = obj.GetType().GetProperty(prop, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var v = pi?.GetValue(obj);
            return v?.ToString();
        }
        catch
        {
            return null;
        }
    }

    public static bool TryReadInt(object root, string nestedObjName, string prop, out int value)
    {
        value = 0;
        try
        {
            if (root == null) return false;
            var nested = root.GetType().GetProperty(nestedObjName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)?.GetValue(root);
            if (nested == null) return false;
            var pi = nested.GetType().GetProperty(prop, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            if (pi == null) return false;
            var v = pi.GetValue(nested);
            if (v is int i)
            {
                value = i;
                return true;
            }

            if (int.TryParse(v?.ToString(), out var p))
            {
                value = p;
                return true;
            }

            return false;
        }
        catch
        {
            return false;
        }
    }

    public static bool? TryReadBool(object obj, string prop)
    {
        try
        {
            var pi = obj.GetType().GetProperty(prop, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            var v = pi?.GetValue(obj);
            if (v is bool b) return b;
            if (bool.TryParse(v?.ToString(), out var p)) return p;
            return null;
        }
        catch
        {
            return null;
        }
    }

    public static void TryInvoke(object obj, string method)
    {
        try
        {
            var mi = obj.GetType().GetMethod(method, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            mi?.Invoke(obj, null);
        }
        catch
        {
            /* good girl action */
        }
    }
}