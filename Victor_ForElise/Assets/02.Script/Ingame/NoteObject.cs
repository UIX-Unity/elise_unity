using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObject : MonoBehaviour
{
    [SerializeField]
    private NoteKeyCode m_Key;
    public NoteKeyCode key => m_Key;

    [SerializeField]
    private NoteData m_Data;
    public NoteData data => m_Data;

    [SerializeField]
    private Transform m_ThisTrf;
    public Transform thisTrf => m_ThisTrf;

    [SerializeField]
    private Renderer[] m_Renderers;
    public Renderer[] renderers => m_Renderers;

    private void Awake()
    {
        if(m_ThisTrf == null)
        {
            m_ThisTrf = transform;
        }
    }

    public void SetData(NoteData data)
    {
        m_Data = data;
    }

    public void Initialize(Material mat)
    {
        for(int i=0;i< renderers.Length;i++)
        {
            renderers[i].material = mat;
        }
    }
}
