using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MusicObjectPool : MonoBehaviour
{
    private Dictionary<NoteKeyCode, Queue<NoteObject>> m_ObjectPoolDic = new Dictionary<NoteKeyCode, Queue<NoteObject>>();
    private Dictionary<NoteKeyCode, NoteObject> m_ObjectDic = new Dictionary<NoteKeyCode, NoteObject>();

    [SerializeField]
    private Transform noteTrf;

    [SerializeField]
    private NoteObject[] noteObjects;

    public void Initialize(int RegistCount)
    {
        for(int i=0;i<noteObjects.Length;i++)
        {
            NoteObject noteTmp = noteObjects[i];
            m_ObjectDic.Add(noteTmp.key, noteTmp);
            Regist(noteTmp.key, RegistCount);
        }
    }

    public void Regist(NoteKeyCode noteKey, int howManyMake)
    {
        if(m_ObjectDic.ContainsKey(noteKey).Equals(false))
        {
            Debug.LogError("There is no correct object :: MusicObjectPool");
            return;
        }

        if (m_ObjectPoolDic.ContainsKey(noteKey).Equals(false))
        {
            Queue<NoteObject> queueTmp = new Queue<NoteObject>();

            m_ObjectPoolDic.Add(noteKey, queueTmp);
        }

        for (int i = 0; i < howManyMake; i++)
        {
            NoteObject noteTmp = Instantiate(m_ObjectDic[noteKey], noteTrf);
            m_ObjectPoolDic[noteKey].Enqueue(noteTmp);
            noteTmp.gameObject.SetActive(false);
        }
    }

    public NoteObject InstantiateObject(NoteKeyCode noteKey, Vector3 pos, Quaternion rot)
    {
        if (m_ObjectPoolDic[noteKey].Count == 0)
        {
            NoteObject obj = Instantiate(m_ObjectDic[noteKey], pos, rot, noteTrf);
            return obj;
        }
        else
        {
            NoteObject value = m_ObjectPoolDic[noteKey].Dequeue();
            value.thisTrf.position = pos;
            value.thisTrf.rotation = rot;

            value.gameObject.SetActive(true);

            return value;
        }
    }

    public void DestroyObject(NoteObject noteobject)
    {
        m_ObjectPoolDic[noteobject.key].Enqueue(noteobject);
        noteobject.gameObject.SetActive(false);
    }
}
