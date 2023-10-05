using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static partial class Helper
{
    public static void ForeachNoGCKey<T,T1>(Dictionary<T, T1> dic, UnityAction<T> action)
    {
        var enumerator = dic.GetEnumerator();
        while (enumerator.MoveNext())
        { 
            action?.Invoke(enumerator.Current.Key);
        }
    }

    public static void ForeachNoGCValue<T,T1>(Dictionary<T, T1> dic, UnityAction<T1> action)
    {
        var enumerator = dic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            action?.Invoke(enumerator.Current.Value);
        }
    }

    public static void ForeachNoGCPair<T,T1>(Dictionary<T, T1> dic, UnityAction<KeyValuePair<T, T1>> action)
    {
        var enumerator = dic.GetEnumerator();

        while (enumerator.MoveNext())
        {
            action?.Invoke(enumerator.Current);
        }
    }

    public static float AngleTo(this Vector2 this_, Vector2 to)
    {
        Vector2 direction = to - this_;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (angle < 0f) angle += 360f;
        return angle;
    }

    public static Vector2 RoundPos(float angle, float radius)
    {
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
        return new Vector2(x, y);
    }

    public static Vector3 RoundPosV3(float angle, float radius)
    {
        float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        float y = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
        return new Vector3(x, y, 0);
    }

    public static string ConvertToRomanNumeral(int numeral)
    {
        switch (numeral)
        {
            default:
            case 0: return string.Empty;
            case 1: return "Ⅰ";
            case 2: return "Ⅱ";
            case 3: return "Ⅲ";
            case 4: return "Ⅳ";
            case 5: return "Ⅴ";
            case 6: return "Ⅵ";
            case 7: return "Ⅶ";
            case 8: return "Ⅷ";
            case 9: return "Ⅸ";
            case 10: return "Ⅹ";
        }
    }

}
