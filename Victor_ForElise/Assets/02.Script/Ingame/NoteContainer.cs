using UnityEngine;
using System.Collections.Generic;


public class NoteContainer
{
    private Dictionary<int, List<NoteData>> m_NoteDataDic = new Dictionary<int, List<NoteData>>();

    // 롱노트 데이터 저장해두기
    private Dictionary<int, LinkedList<NoteData>> m_LongNoteDataDic = new Dictionary<int, LinkedList<NoteData>>();
    public Dictionary<int, LinkedList<NoteData>> GetLongNoteDataDic => m_LongNoteDataDic;

    // 롱노트 데이터 Node 값 저장
    // key: (LongNoteIndex, BarIndex, BeatIndex)
    private MultiKeyDictionary<int, int, int, LinkedListNode<NoteData>> m_LongNoteNodeDic = new MultiKeyDictionary<int, int, int, LinkedListNode<NoteData>>();
    public MultiKeyDictionary<int, int, int, LinkedListNode<NoteData>> GetLongNoteNodeDic => m_LongNoteNodeDic;

    public bool TryGetNoteListByBarIndex(int barIndex, out List<NoteData> noteDatas)
    {
        if(m_NoteDataDic.TryGetValue(barIndex, out noteDatas).Equals(false))
        {
            return false;
        }

        if (noteDatas == null)
        {
            Debug.Log("Need Init");
            return false;
        }

        return true;
    }

    public void SheetDataToNoteContainer(Sheet sheet)
    {
        m_NoteDataDic.Clear();
        m_LongNoteDataDic.Clear();
        m_LongNoteNodeDic.Clear();

        // Sort By BeatIndex
        NoteData[] noteDatas = sheet.noteDatas.ToArray();

        for (int i = 0; i < noteDatas.Length - 1; i++)
        {
            for (int j = 0; j < noteDatas.Length - i - 1; j++)
            {
                if (noteDatas[j].CompareDataIndex(noteDatas[j + 1]) > 0)
                {
                    NoteData tmp = noteDatas[j];
                    noteDatas[j] = noteDatas[j + 1];
                    noteDatas[j + 1] = tmp;
                }
            }
        }

        for (int i = 0; i < noteDatas.Length; i++)
        {
            NoteData tmp = noteDatas[i];
            int barIndex = tmp.BarIndex;

            if (m_NoteDataDic.ContainsKey(barIndex).Equals(false))
            {
                List<NoteData> listTmp = new List<NoteData>();
                m_NoteDataDic.Add(barIndex, listTmp);
            }

            m_NoteDataDic[barIndex].Add(tmp);

            if(tmp.NoteType == NoteKeyCode.LongStart ||
                tmp.NoteType == NoteKeyCode.LongEnd)
            {
                if (m_LongNoteDataDic.ContainsKey(tmp.LongNoteIndex).Equals(false))
                {
                    LinkedList<NoteData> linkedListTmp = new LinkedList<NoteData>();
                    m_LongNoteDataDic.Add(tmp.LongNoteIndex, linkedListTmp);
                }

                LinkedListNode<NoteData> nodeTmp = m_LongNoteDataDic[tmp.LongNoteIndex].AddLast(tmp);
                m_LongNoteNodeDic.Add(tmp.LongNoteIndex, tmp.BarIndex, tmp.BeatIndex, nodeTmp);
            }
        }
    }
}
