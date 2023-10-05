using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class BMSParser : MonoBehaviour
{
    GameSetting info;

    public BMSPlayer refBMSPlayer;
    [SerializeField] NoteManager noteManager;

    static int noteNum = 0;
    public int fullCount;

    private void Awake()
    {
        info = Resources.Load("GameSetting") as GameSetting;
        fullCount = 0;
        Parser();
    }

    private void Parser()
    {
        noteNum = 0;

        TextAsset text = info.selectedMusic.levelBMS[info.lv];
        TextReader reader = new StringReader(text.text);
        string lineData;
        char[] seps = new char[] { ' ', ':' };
        do
        {
            lineData = reader.ReadLine();
            Process(lineData);
        } while (lineData != null);
        reader.Close();
        noteManager.SortNoteAll();
        noteManager.ChannelSetting();
        noteManager.SetPlayerInputData();
        refBMSPlayer.fullNoteCount = fullCount;
    }

    public void Process(string lineData)
    {
        if (lineData == null) return;

        if (lineData.StartsWith("#"))
        {
            char[] seps = new char[] { ' ', ':' };
            string[] StringList = lineData.Split(seps);

            if (StringList[0].Equals("#PLAYER"))
            {
                int Player = int.Parse(StringList[1]);
            }
            else if (StringList[0].Equals("#GENRE"))
            {
            }
            else if (StringList[0].Equals("#TITLE"))
            {
            }
            else if (StringList[0].Equals("#ARTIST"))
            {
            }
            else if (StringList[0].Equals("#BPM"))
            {
                info.BPM = float.Parse(StringList[1]);
            }
            else if (StringList[0].Equals("#PLAYLEVEL"))
            {
            }
            else if (StringList[0].Equals("#RANK"))
            {
            }
            else if (StringList[0].Equals("#VOLWAV"))
            {
            }
            else if (StringList[0].Equals("#STAGEFILE"))
            {
            }
            else if (StringList[0].Equals("#TOTAL"))
            {
            }
            else if (StringList[0].Equals("#MIDIFILE"))
            {
            }
            else if (StringList[0].Equals("#VIDEOFILE"))
            {
            }
            else
            {
                try
                {
                    int BarNum = GetBarNum(StringList[0]);
                    int ChannelNum = GetChannelNum(StringList[0]);

                    int ChannelFirst = ChannelNum / 10;
                    int ChannelSecond = ChannelNum % 10;

                    Debug.Log("Bar: " + BarNum + ", Channel: " + ChannelNum + ", First : " + ChannelFirst + ", Second : " + ChannelSecond);
                    AddNoteData(BarNum, ChannelFirst, ChannelSecond, StringList[1]);
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message);
                }
            }
        }
        else
        {
            return;
        }
    }

    private int GetBarNum(string data)
    {
        return int.Parse(data.Substring(1, 3));
    }

    private int GetChannelNum(string data)
    {
        return int.Parse(data.Substring(4, 2));
    }

    void AddNoteData(int BarNumber, int ChannelFirst, int ChannelSecond, string data)
    {
        switch (ChannelFirst)
        {
            case 0:
                break;

            case 1:
            case 5:
                DefaultNoteChannel(BarNumber, ChannelFirst, ChannelSecond, data);
                break;
        }
    }

    void DefaultChannel()
    {

    }

    void DefaultNoteChannel(int BarNumber, int ChannelFirst, int ChannelSecond, string data)
    {
        Note note = new Note();

        if (ChannelSecond.Equals(8))
        {
            ChannelSecond = 6;
        }

        note.channelNumber = ChannelFirst; // 1 and 5
        note.keyNumber = ChannelSecond; //1 to 6
        note.noteNumber = noteNum;
        noteNum++;

        int n = data.Length / 2;

        for (int i = 0; i < n; i++)
        {
            int Num = int.Parse(data.Substring(i * 2, 2), NumberStyles.HexNumber);
            Debug.Log(Num);
            if (Num == 0) continue;

            double y = ((i / (double)n) * 1000) + (BarNumber * 1000);
            y *= info.speed;

            note.position = (float)y;

            fullCount++;

            noteManager.AddNote(note);

        }
    }
}