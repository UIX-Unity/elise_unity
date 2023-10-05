using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static partial class Helper
{
    public static class Guid
    {
        public static string CreateGUID()
        {
            return System.Guid.NewGuid().ToString();
        }
    }
}
