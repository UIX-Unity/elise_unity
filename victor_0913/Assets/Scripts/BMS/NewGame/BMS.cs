using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
namespace NewGame
{
    public class BMS : MonoBehaviour
    {
        [SerializeField] TextAsset textAssetTesting;
        private GameSetting info;
        private NoteManager noteManager;
        private Color currentColor;
        private SwipeDirection currentSwipeDir;
        private int currentRotation;
        private int fullCount;
        
        private void Awake()
        {
            info = Resources.Load("GameSetting") as GameSetting;
            noteManager = this.GetComponent<NoteManager>();
            fullCount = 0;
            TextParser();
        }

        private void TextParser()
        {
            TextAsset textAsset = textAssetTesting;
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
                else if (StringList[0].Equals("#COLOR"))
                {
                }
                else if (StringList[0].Equals("#DIRECTION"))
                {
                }
                else if (StringList[0].Equals("#ROTATION"))
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

        }

        public int GetFullCount()
        {
            return fullCount;
        }
    }
}