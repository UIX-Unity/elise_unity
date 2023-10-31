using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
namespace NewGame
{
    public class Parser : MonoBehaviour
    {
        //this field for just directly drag and drop the bms file into and test them
        [SerializeField] TextAsset textAssetTesting; 
        //Color pool for notes
        [SerializeField] Color[] colors;
        private GameSetting info;
        private NoteManager noteManager;
        static int noteNum = 0;
        private int fullCount;
        private int colorIndex;
        private int typeIndex;

        private void Awake()
        {
            info = Resources.Load("GameSetting") as GameSetting;
            noteManager = this.GetComponent<NoteManager>();
            fullCount = 0;
            TextParser();
        }

        //Start reading the BMS file
        private void TextParser()
        {
            TextAsset textAsset = textAssetTesting; // this text asset loaded from GameSetting.selectedMusic.levelBms[info.lv]
            TextReader reader = new StringReader(textAsset.text);
            string lineData;
            do
            {
                lineData = reader.ReadLine();
                Process(lineData);
            } while (lineData != null);
            reader.Close();
            noteManager.SortNoteData();
        }

        private void Process(string lineData)
        {
            if (lineData == null) return;

            if (lineData.StartsWith("#"))
            {
                char[] seps = new char[] { ' ', ':', ',' };
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
                else if (StringList[0].Equals("#COLOR"))
                {
                    colorIndex = int.Parse(StringList[1]);
                }
                else if (StringList[0].Equals("#TYPE"))
                {
                    typeIndex = int.Parse(StringList[1]);
                }
                else
                {
                    try
                    {
                        int barNum = GetBarNum(StringList[0]);
                        int channelNum = GetChannelNum(StringList[0]);

                        AddNoteData(barNum, channelNum, typeIndex, colorIndex, StringList);
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

        //Add Notedata into the list
        private void AddNoteData(int barNum, int channelNum, int typeIndex, int colorIndex, string[] stringList)
        {
            NoteData noteData = new NoteData();
            Vector3 notePosition = new Vector3(float.Parse(stringList[1]) * 100f, float.Parse(stringList[2]) * 100f, (barNum * 1000f + channelNum * 100f) * info.speed);
            NoteType noteType = (NoteType)typeIndex - 1;

            noteData = TypeHandler(noteType, noteData, stringList);
            noteData.noteType = noteType;
            noteData.noteColor = colors[colorIndex - 1];
            noteData.notePosition = notePosition;

            noteData.noteID = fullCount;
            noteManager.AddNoteData(noteData);
            fullCount++;
        }

        private int GetBarNum(string data)
        {
            return int.Parse(data.Substring(1, 3));
        }

        private int GetRotation(string data)
        {
            return int.Parse(data.Substring(4, 1));
        }

        private int GetChannelNum(string data)
        {
            return int.Parse(data.Substring(5, 1));
        }

        private int GetColor(string data)
        {
            return int.Parse(data.Substring(6, 1));
        }

        //Handler of the type of the note data in BMS file
        private NoteData TypeHandler(NoteType noteType, NoteData noteData, String[] stringList)
        {
            switch (noteType)
            {
                case NoteType.BasicNote:
                    return noteData;
                case NoteType.LongNote:
                    int longNoteLength = int.Parse(stringList[3]) * 1000;
                    Debug.Log(" " + longNoteLength);
                    noteData.longNoteLength = longNoteLength;
                    return noteData;
                case NoteType.DirectionNote:
                    SwipeDirection swipeDirection = (SwipeDirection)int.Parse(stringList[3]);
                    Debug.Log(" " + swipeDirection);
                    noteData.swipeDirection = swipeDirection;
                    return noteData;
                case NoteType.LongDirectionNote:
                    int longDirNoteLength = int.Parse(stringList[3]) * 1000;
                    SwipeDirection longSwipeDirection = (SwipeDirection)int.Parse(stringList[4]);
                    Debug.Log(" " + longDirNoteLength);
                    Debug.Log(" " + longSwipeDirection);
                    noteData.longNoteLength = longDirNoteLength;
                    noteData.swipeDirection = longSwipeDirection;
                    return noteData;
            }
            return noteData;
        }

        // private Color HexToColor(string hex)
        // {
        //     float r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
        //     float g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber) / 255f;
        //     float b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber) / 255f;

        //     return new Color(r, g, b);
        // }

        public int GetFullCount()
        {
            return fullCount;
        }
    }
}
