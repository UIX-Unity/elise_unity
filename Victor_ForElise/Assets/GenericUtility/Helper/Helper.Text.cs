using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Helper
{
    public static class Text
    {
        public static string GetIconID(string id)
        {
            return $"{id}_Icon";
        }

        public static string GetSpriteID(string id)
        {
            return $"{id}_Sprite";
        }
    }
}
