using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static partial class Helper
{
    public static class Math
    {
        public static float ContAngle(Vector3 fwd, Vector3 targetDir)
        {
            float angle = Vector3.Angle(fwd, targetDir);

            if (AngleDir(fwd, targetDir, Vector3.forward) == -1)
            {
                angle = 360.0f - angle;
                if (angle > 359.9999f)
                    angle -= 360.0f;
                return angle;
            }
            else
                return angle;
        }

        public static int AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
        {
            Vector3 perp = Vector3.Cross(fwd, targetDir);
            float dir = Vector3.Dot(perp, up);

            if (dir > 0.0)
                return 1;
            else if (dir < 0.0)
                return -1;
            else
                return 0;
        }

    }
}
