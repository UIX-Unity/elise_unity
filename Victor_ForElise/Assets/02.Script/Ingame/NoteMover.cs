using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteMover : MonoBehaviour
{
    private List<NoteObject> noteObjectList = new List<NoteObject>();
    public List<NoteObject> GetNoteObjectList => noteObjectList;

    private Dictionary<int, NoteObject> longNoteMeshDic = new Dictionary<int, NoteObject>();
    public Dictionary<int, NoteObject> GetLongNoteMeshDic => longNoteMeshDic;


    public void AddNoteList(NoteObject noteObject)
    {
        noteObjectList.Add(noteObject);
    }

    public void AddLongNoteMesh(int index, NoteObject noteObject)
    {
        longNoteMeshDic.Add(index, noteObject);
    }

    public void RemoveAllNoteList()
    {
        int listCount = noteObjectList.Count;
        for (int i=0;i< listCount; i++)
        {
            NoteObject tmp = noteObjectList[i];

            GameManager.GetMusicPool.DestroyObject(tmp);
        }
        noteObjectList.Clear();
        longNoteMeshDic.Clear();
    }

    public void UpdateNote(float deltaTime)
    {
        List<NoteObject> destoryList = new List<NoteObject>();

        float barSpeed = GameManager.Instance.BarDistance * (deltaTime / GameManager.GetMusic.BarPerSec);

        int listCount = noteObjectList.Count;
        for (int i = 0; i < listCount; i++)
        {
            NoteObject tmp = noteObjectList[i];
            
            tmp.thisTrf.Translate(Vector3.back * barSpeed);

            if (tmp.key == NoteKeyCode.LongMesh)
            {
                continue;
            }
            else if(tmp.key == NoteKeyCode.LongEnd)
            {
                if (tmp.thisTrf.position.z < -50f)
                {
                    destoryList.Add(tmp);
                    destoryList.Add(GetLongNoteMeshDic[tmp.data.LongNoteIndex]);
                }
            }
            // 마디 라인은 정확히 원점에서 사라지게
            else if (tmp.key == NoteKeyCode.Line)
            {
                if (tmp.thisTrf.position.z < 0f)
                {
                    destoryList.Add(tmp);
                }
            }
            // 노트들은 원점 보다 살짝 늦게 사라지게
            else
            {
                if (tmp.thisTrf.position.z < -50f)
                {
                    destoryList.Add(tmp);
                }
            }
        }

        for (int i = 0; i < destoryList.Count; i++)
        {
            NoteObject tmp = destoryList[i];

            noteObjectList.Remove(tmp);
            GameManager.GetMusicPool.DestroyObject(tmp);

            if (tmp.data.NoteType == NoteKeyCode.LongMesh)
            {
                GetLongNoteMeshDic.Remove(tmp.data.LongNoteIndex);
            }
        }
    }
}
