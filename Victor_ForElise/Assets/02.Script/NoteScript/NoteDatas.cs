using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TO DO...
// Note의 X 좌표
// Note의 Y 좌표
// Note의 시간
// Note의 종류
// 롱노트시간

public enum NoteKeyCode
{
    Line,
    Default,
    LeftSlide,
    UpSlide,
    RightSlide,
    DownSlide,
    LongStart,
    LongEnd,
    LongMesh
}

[Serializable]
public struct NoteData
{
    public float PosX;
    public float PosY;
    public int BarIndex;
    public int BeatIndex;
    public NoteKeyCode NoteType;
    public int LongNoteIndex;

    public NoteData(float PosX, float PosY, int BarIndex, int BeatIndex, NoteKeyCode noteType, int longNoteIndex)
    {
        this.PosX = PosX;
        this.PosY = PosY;
        this.BarIndex = BarIndex;
        this.BeatIndex = BeatIndex;
        this.NoteType = noteType;
        this.LongNoteIndex = longNoteIndex;
    }

    public string GetLog()
    {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine($"Pos:{PosX},{PosY} BarIndex:{BarIndex} BeatIndex:{BeatIndex} NoteType:{NoteType} LongNoteIndex:{LongNoteIndex}");

        return builder.ToString();
    }

    public static bool operator == (NoteData a, NoteData b)
    {
        if(a.PosX != b.PosX)
        {
            return false;
        }
        if(a.PosY != b.PosY)
        {
            return false;
        }
        if(a.BarIndex != b.BarIndex)
        {
            return false;
        }
        if(a.BeatIndex != b.BeatIndex)
        {
            return false;
        }
        if (a.NoteType != b.NoteType)
        {
            return false;
        }
        if (a.LongNoteIndex != b.LongNoteIndex)
        {
            return false;
        }

        return true;
    }

    public static bool operator != (NoteData a, NoteData b)
    {
        if (a.PosX != b.PosX)
        {
            return true;
        }
        if (a.PosY != b.PosY)
        {
            return true;
        }
        if (a.BarIndex != b.BarIndex)
        {
            return true;
        }
        if (a.BeatIndex != b.BeatIndex)
        {
            return true;
        }
        if (a.NoteType != b.NoteType)
        {
            return true;
        }
        if (a.LongNoteIndex != b.LongNoteIndex)
        {
            return true;
        }

        return false;
    }

    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }

    public override int GetHashCode()
    {
        return base.GetHashCode();
    }

    public override string ToString()
    {
        return base.ToString();
    }

    public int CompareDataIndex(NoteData noteData)
    {
        //정렬 조건을 설정한다.
        //x보다 앞이면 -1
        //x와 같으면 0
        //x보다 뒤면 1
        if (this.BarIndex < noteData.BarIndex)
        {
            return -1;
        }
        else if(this.BarIndex == noteData.BarIndex)
        {
            if (this.BeatIndex < noteData.BeatIndex)
            {
                return -1;
            }
            else if (this.BeatIndex == noteData.BeatIndex)
            {
                return 0;
            }
            else
            {
                return 1;
            }
        }
        else
        {
            return 0;
        }
    }
}

[Serializable]
public class Sheet
{
    public string Name;
    public int BPM;
    public float Offset;

    public List<NoteData> noteDatas;

    public Sheet()
    {
        Name = "";
        BPM = 0;
        Offset = 0;

        noteDatas = new List<NoteData>();
    }

    public Sheet(Sheet sheet)
    {
        Name = sheet.Name;
        BPM = sheet.BPM;
        Offset = sheet.Offset;

        noteDatas = sheet.noteDatas;
    }

    public Sheet(string name, int bpm, float offset, List<NoteData> list)
    {
        Name = name;
        BPM = bpm;
        Offset = offset;
        noteDatas = list;
    }

    public string GetLog()
    {
        StringBuilder builder = new StringBuilder();

        builder.AppendLine($"Name:{Name}");
        builder.AppendLine($"BPM:{BPM}");
        builder.AppendLine($"Offset:{Offset}");
        builder.AppendLine("\nNoteDatas");
        for (int i=0;i<noteDatas.Count;i++)
        {
            builder.AppendLine($"Note{i}\n{noteDatas[i].GetLog()}");
        }

        return builder.ToString();
    }
}

