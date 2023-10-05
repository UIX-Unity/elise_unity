using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class Utility
{
    public static void ForeachNoGCKey<T, T1>(Dictionary<T, T1> dic, UnityAction<T> action)
    {
        var enumerator = dic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            action?.Invoke(enumerator.Current.Key);
        }
    }

    public static void ForeachNoGCValue<T, T1>(Dictionary<T, T1> dic, UnityAction<T1> action)
    {
        var enumerator = dic.GetEnumerator();
        while (enumerator.MoveNext())
        {
            action?.Invoke(enumerator.Current.Value);
        }
    }

    public static void ForeachNoGCPair<T, T1>(Dictionary<T, T1> dic, UnityAction<KeyValuePair<T, T1>> action)
    {
        var enumerator = dic.GetEnumerator();

        while (enumerator.MoveNext())
        {
            action?.Invoke(enumerator.Current);
        }
    }

    public static void AddVertexToMesh(NoteObject noteMesh, NoteData curNoteData)
    {
        List<Vector3> verticesTmp = new List<Vector3>();

        MeshFilter meshFilterTmp = noteMesh.GetComponent<MeshFilter>();
        meshFilterTmp.mesh.GetVertices(verticesTmp);

        // Vertex 계산 ----
        
        // 롱노트의 Head 데이터
        NoteData firstLongNoteData = GameManager.Instance.noteContainer.GetLongNoteDataDic[curNoteData.LongNoteIndex].First.Value;

        // 현재 노트의 첫번째 노트와의 비트 차이
        int beatDifferent =
            ((32 * curNoteData.BarIndex) + curNoteData.BeatIndex) - ((32 * firstLongNoteData.BarIndex) + firstLongNoteData.BeatIndex);

        // 현재 노트의 ZPos
        float CalculateZPos = ((GameManager.Instance.BarDistance * 0.03125f) * beatDifferent);

        Vector3 vertexPos = new Vector3(curNoteData.PosX - firstLongNoteData.PosX, curNoteData.PosY - firstLongNoteData.PosY, CalculateZPos);
        Vector3[] verticesCalculate = MakeSquareVertices(vertexPos, GameManager.GetBoxSize);

        // Vertex 추가
        for (int i = 0; i < verticesCalculate.Length; i++)
        {
            verticesTmp.Add(verticesCalculate[i]);
        }
        meshFilterTmp.mesh.SetVertices(verticesTmp);

        // 트라이 계산
        int[] calculateTri = MakeBoxTriangle(meshFilterTmp.mesh.triangles, verticesTmp.Count);

        // 트라이 추가
        meshFilterTmp.mesh.triangles = calculateTri;

        // 트라이 노멀 재계산
        meshFilterTmp.mesh.RecalculateNormals();
    }

    public static Vector3[] MakeSquareVertices(Vector3 pos, float squareSize)
    {
        float halfSquareSize = squareSize * 0.5f;

        float RightXSize = pos.x + halfSquareSize;
        float LeftXSize = pos.x - halfSquareSize;
        float UpYSize = pos.y + halfSquareSize;
        float DowmYSize = pos.y - halfSquareSize;

        Vector3[] vertices = new Vector3[]
        {
            // 우상단
                new Vector3(RightXSize, UpYSize, pos.z),

                // 우하단
                new Vector3(RightXSize, DowmYSize, pos.z),

                // 좌하단
                new Vector3(LeftXSize, DowmYSize, pos.z),

                // 좌상단
                new Vector3(LeftXSize, UpYSize, pos.z)
        };

        return vertices;
    }

    public static int[] MakeBoxTriangle(int[] triangleArray, int vertexLength)
    {
        int[] tmpTri = triangleArray;
        int triLength = tmpTri.Length;
        int triAddLength = triLength + 24;

        Array.Resize(ref tmpTri, triAddLength);

        int lastVertexStart = vertexLength - 8;


        // 오른쪽면
        //0, 4, 5,
        //0, 5, 1,

        // 윗면
        //0, 7, 4,
        //0, 3, 7,

        // 왼쪽 면
        //3, 6, 7,
        //3, 2, 6,

        // 아래 면
        //2, 5, 6,
        //2, 1, 5

        // 오른쪽면
        tmpTri[triLength] = lastVertexStart;
        tmpTri[triLength + 1] = lastVertexStart + 4;
        tmpTri[triLength + 2] = lastVertexStart + 5;

        tmpTri[triLength + 3] = lastVertexStart;
        tmpTri[triLength + 4] = lastVertexStart + 5;
        tmpTri[triLength + 5] = lastVertexStart + 1;

        // 오른쪽면
        tmpTri[triLength + 6] = lastVertexStart;
        tmpTri[triLength + 7] = lastVertexStart + 7;
        tmpTri[triLength + 8] = lastVertexStart + 4;

        tmpTri[triLength + 9] = lastVertexStart;
        tmpTri[triLength + 10] = lastVertexStart + 3;
        tmpTri[triLength + 11] = lastVertexStart + 7;

        // 오른쪽면
        tmpTri[triLength + 12] = lastVertexStart + 3;
        tmpTri[triLength + 13] = lastVertexStart + 6;
        tmpTri[triLength + 14] = lastVertexStart + 7;

        tmpTri[triLength + 15] = lastVertexStart + 3;
        tmpTri[triLength + 16] = lastVertexStart + 2;
        tmpTri[triLength + 17] = lastVertexStart + 6;

        // 오른쪽면
        tmpTri[triLength + 18] = lastVertexStart + 2;
        tmpTri[triLength + 19] = lastVertexStart + 5;
        tmpTri[triLength + 20] = lastVertexStart + 6;

        tmpTri[triLength + 21] = lastVertexStart + 2;
        tmpTri[triLength + 22] = lastVertexStart + 1;
        tmpTri[triLength + 23] = lastVertexStart + 5;

        return tmpTri;
    }

    public static Mesh MakeMeshTriangle(Vector3 cur, Vector3 next, float BoxSize)
    {
        float halfBoxSize = BoxSize * 0.5f;

        float frontBoxXSize = cur.x + halfBoxSize;
        float frontBoxYSize = cur.y + halfBoxSize;
        float backBoxXSize = next.x + halfBoxSize;
        float backBoxYSize = next.y + halfBoxSize;


        Mesh meshTmp = new Mesh();
        meshTmp.vertices = new Vector3[]
        {
                // 전면 우상단
                new Vector3(frontBoxXSize, frontBoxYSize, 0f),

                // 전면 우하단
                new Vector3(frontBoxXSize, -frontBoxYSize, 0f),

                // 전면 좌하단
                new Vector3(-frontBoxXSize, -frontBoxYSize, 0f),

                // 전면 좌상단
                new Vector3(-frontBoxXSize, frontBoxYSize, 0f),

                // 후면 우상단
                new Vector3(backBoxXSize, backBoxYSize, next.z),

                // 후면 우하단
                new Vector3(backBoxXSize, -backBoxYSize, next.z),

                // 후면 좌하단
                new Vector3(-backBoxXSize, -backBoxYSize, next.z),

                // 후면 좌상단
                new Vector3(-backBoxXSize, backBoxYSize, next.z)
        };

        meshTmp.triangles = new int[]
            {
                    // 오른쪽면
                    //0, 4, 5,
                    //0, 5, 1,

                    // 윗면
                    //0, 7, 4,
                    //0, 3, 7,

                    // 왼쪽 면
                    //3, 6, 7,
                    //3, 2, 6,

                    // 아래 면
                    //2, 5, 6,
                    //2, 1, 5
            };

        return meshTmp;
    }
}
