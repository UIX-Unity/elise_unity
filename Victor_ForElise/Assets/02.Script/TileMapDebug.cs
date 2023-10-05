using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileMapDebug : MonoBehaviour
{
    [SerializeField]
    Camera mainCam;

    [Range(0, 200)]
    [SerializeField]
    private int xSize = 160;

    [Range(0,200)]
    [SerializeField]
    private int ySize = 90;

    [Range(0, 10)]
    [SerializeField]
    private float cubeSize = 0.5f;

    [Range(0, 3000)]
    [SerializeField]
    private float boxsizeX = 0.5f;

    [Range(0, 3000)]
    [SerializeField]
    private float boxsizeY = 0.5f;

    float ratioX = 4f;
    float ratioY = 3f;

    private void OnDrawGizmos()
    {
        //float screenRatio = (float)Screen.width / Screen.height;

        //float ratio = ratioX / ratioY;

        //float halfX = (xSize / 2f);
        //float halfY = (ySize / 2f);

        //float halfCubeSize = cubeSize * 0.5f;

        //Vector3 UpLeft = new Vector3(-halfCubeSize, halfCubeSize, 0f);
        //Vector3 UpRight = new Vector3(halfCubeSize, halfCubeSize, 0f);
        //Vector3 DownRight = new Vector3(halfCubeSize, -halfCubeSize, 0f);
        //Vector3 DownLeft = new Vector3(-halfCubeSize, -halfCubeSize, 0f);

        //for (float y=-halfY; y< halfY; y++)
        //{   
        //    for(float x =-halfX; x< halfX; x++)
        //    {
        //        Vector3 cubePos = new Vector3(x + 0.5f, y + 0.5f, 0f);

        //        Vector3 screenPos = mainCam.ScreenToViewportPoint(cubePos);

        //        Gizmos.DrawLine(cubePos + UpLeft, cubePos + UpRight);
        //        Gizmos.DrawLine(cubePos + UpRight, cubePos + DownRight);
        //        Gizmos.DrawLine(cubePos + DownRight, cubePos + DownLeft);
        //        Gizmos.DrawLine(cubePos + DownLeft, cubePos + UpLeft);
        //    }
        //}
    }
}
