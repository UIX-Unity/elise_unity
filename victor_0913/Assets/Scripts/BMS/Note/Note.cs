using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Note
{
    public int keyNumber;
    public int channelNumber;
    public int barNumber;
    public float position;
    public int longNoteSize;
    public int noteNumber;
}