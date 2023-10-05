using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static partial class Helper
{
    public static class Parser
    {
        public static int StringToInt(string str, int defaultValue = 0)
        {
            int value;
            if (int.TryParse(str, out value))
            {
                return value;
            }
            return defaultValue;
        }

        public static float StringToFloat(string str, float defaultValue = 0)
        {
            float value;
            if (float.TryParse(str, out value))
            {
                return value;
            }
            return defaultValue;
        }
    }
}
