using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListDebugger : MonoBehaviour
{
    public List<NoteObject> listTmp;

    // Update is called once per frame
    void Update()
    {
        listTmp = GameManager.GetNoteMover.GetNoteObjectList;
    }
}
