using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using TMPro;

namespace EditorScript
{
    public class EditorManager : BaseMonoSingleton<EditorManager>
    {
        private List<TextMeshProUGUI> longNoteIndexTmpList = new List<TextMeshProUGUI>();
        private Queue<TextMeshProUGUI> longNoteIndexPool = new Queue<TextMeshProUGUI>();

        #region < Variables >

        [SerializeField]
        private TextMeshProUGUI longNoteIndex;

        [SerializeField]
        private AudioSource metronomSource;

        [SerializeField]
        private MusicSlider bgmSlider;

        [SerializeField]
        private AudioWaveForm audioWaveForm;

        [SerializeField]
        private TMP_InputField fileNameInput;

        [SerializeField]
        private Transform box;

        [SerializeField]
        private NotePicker m_NotePicker;
        public NotePicker notePicker => m_NotePicker;

        [Header("BPM 조절"), Space(10)]
        [Range(60, 1000)]
        [SerializeField]
        private int bpm;

        [Header("오프셋"), Space(10)]
        [Range(-10, 10)]
        [SerializeField]
        private float offsetTime;
        public float GetOffsetTime => offsetTime;

        [Header("가이드라인 박스 사이즈"), Space(10)]
        [Range(-10, 10)]
        [SerializeField]
        private float boxSize;
        public float GetBoxSize => boxSize;

        private int selectBeat;

        #endregion

        public bool sliderClicked { get; private set; }
        public bool IsBeatLineOn { get; private set; }
        public bool IsGuideLineOn { get; private set; }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            // General Singletones Init
            DataManager.CreateInstance();


            sliderClicked = false;

            selectBeat = 4;

            // BGM Slider Init
            bgmSlider.beginAction += OnDragStart;
            bgmSlider.dragAction += OnDrag;
            bgmSlider.endAction += OnDragEnd;

            UIManaging.Instance.Initialize();
            GameManager.Instance.Initialize();
            GameManager.Instance.OnTick += OnTick;
            GameManager.Instance.OnUpdate += OnUpdate;

            notePicker.Initialize();

            GameManager.Instance.OnUpdate += notePicker.UpdatePicker;
            GameManager.Instance.OnResetPos += ResetBeatLine;

            IsBeatLineOn = UIManaging.Instance.beatLineTog.isOn;
            IsGuideLineOn = UIManaging.Instance.guideLineTog.isOn;
        }

        public void OnClickedBeatLeftBtn()
        {
            GameManager.Instance.PauseGame();

            GameManager.Instance.AddBeatCount(-1f, selectBeat);

            GameManager.Instance.ReCalculateTimeSample();
            GameManager.Instance.InitMusicSample();
            GameManager.Instance.ResetBarPos();
        }
        public void OnClickedBeatRightBtn()
        {
            GameManager.Instance.PauseGame();
            GameManager.Instance.AddBeatCount(1f, selectBeat);

            GameManager.Instance.ReCalculateTimeSample();
            GameManager.Instance.InitMusicSample();
            GameManager.Instance.ResetBarPos();
        }
        public void OnClickedBarLeftBtn()
        {
            GameManager.Instance.PauseGame();
            GameManager.AddBarCount(-1);

            GameManager.Instance.ReCalculateTimeSample();
            GameManager.Instance.InitMusicSample();
            GameManager.Instance.ResetBarPos();
        }
        public void OnClickedBarRightBtn()
        {
            GameManager.Instance.PauseGame();
            GameManager.AddBarCount(1);

            GameManager.Instance.ReCalculateTimeSample();
            GameManager.Instance.InitMusicSample();
            GameManager.Instance.ResetBarPos();
        }

        public void OnClick4BeatBtn()
        {
            selectBeat = 4;

            GameManager.Instance.AddBeatCount(0, selectBeat);
            GameManager.Instance.ReCalculateTimeSample();
            GameManager.Instance.InitMusicSample();
            GameManager.Instance.ResetBarPos();
        }
        public void OnClick8BeatBtn()
        {
            selectBeat = 8;

            GameManager.Instance.AddBeatCount(0, selectBeat);
            GameManager.Instance.ReCalculateTimeSample();
            GameManager.Instance.InitMusicSample();
            GameManager.Instance.ResetBarPos();
        }
        public void OnClick16BeatBtn()
        {
            selectBeat = 16;

            GameManager.Instance.AddBeatCount(0, selectBeat);
            GameManager.Instance.ReCalculateTimeSample();
            GameManager.Instance.InitMusicSample();
            GameManager.Instance.ResetBarPos();
        }
        public void OnClick32BeatBtn()
        {
            selectBeat = 32;

            GameManager.Instance.AddBeatCount(0, selectBeat);
            GameManager.Instance.ReCalculateTimeSample();
            GameManager.Instance.InitMusicSample();
            GameManager.Instance.ResetBarPos();
        }

        public void RemoveTextAll()
        {
            // Remove Txt
            for (int i = 0; i < longNoteIndexTmpList.Count; i++)
            {
                TextMeshProUGUI textTmp = longNoteIndexTmpList[i];
                textTmp.gameObject.SetActive(false);
                longNoteIndexPool.Enqueue(textTmp);
            }
            longNoteIndexTmpList.Clear();
        }

        public void ResetBeatLine(int barCount, float offsetPos)
        {
            RemoveTextAll();

            // GetLongNote
            Vector3[] nowLongNote = GameManager.Instance.GetLongNoteMeshPosAtZeroPoint();

            for(int i=0;i<nowLongNote.Length;i++)
            {
                Vector3 longNoteVector = nowLongNote[i];
                TextMeshProUGUI textTmp;
                if (longNoteIndexPool.Count > 0)
                {
                    textTmp = longNoteIndexPool.Dequeue();
                }
                else
                {
                    textTmp = Instantiate(longNoteIndex, UIManaging.Instance.infoCanvas);
                }
                textTmp.transform.position = new Vector3(longNoteVector.x,longNoteVector.y, 0f);
                textTmp.text = longNoteVector.z.ToString();
                textTmp.gameObject.SetActive(true);

                longNoteIndexTmpList.Add(textTmp);
            }
           

            

            float BarDistance = GameManager.Instance.BarDistance;

            for (int i = 0; i < barCount + 1; i++)
            {
                for (int beat = 1; beat < 32; beat++)
                {
                    // PosZ 구하는 공식
                    // 0.03125f 는 1비트당 비율값을 나타냄
                    float posZ = (offsetPos + (BarDistance * (i - 1))) + ((BarDistance * 0.03125f) * beat);

                    if (posZ >= 0f)
                    {
                        MakeBeatLine(beat, posZ);
                    }
                }
            }
        }

        public void ChangeGuideLineTog(bool value)
        {
            IsGuideLineOn = value;

            GameManager.Instance.PauseGame();
        }

        public void ChangeBeatLineTog(bool value)
        {
            IsBeatLineOn = value;

            GameManager.Instance.PauseGame();
        }

        public void OnDragStart()
        {
            sliderClicked = true;
            GameManager.Instance.PauseGame();
        }

        public void OnDrag(float value)
        {
            GameManager.GetMusic.ChangeTime(value);
        }

        public void OnDragEnd()
        {
            sliderClicked = false;

            GameManager.Instance.InitMusicSample();
            GameManager.Instance.ReCalculateTimeSample();
            GameManager.Instance.InitMusicSample();
            GameManager.Instance.ResetBarPos();
        }

        private void MakeBeatLine(int curBeat,float PosZ)
        {
            if (IsBeatLineOn)
            {
                if (selectBeat >= 32)
                {
                    if (curBeat % 2 == 1)
                    {
                        NoteObject noteObject = GameManager.GetMusicPool.InstantiateObject(NoteKeyCode.Line,
                    new Vector3(0f, 0f, PosZ), Quaternion.identity);
                        noteObject.Initialize(GameManager.Instance.Beat32Mat);
                        GameManager.GetNoteMover.AddNoteList(noteObject);
                    }
                }
                if (selectBeat >= 16)
                {
                    if (curBeat % 4 == 2)
                    {
                        NoteObject noteObject = GameManager.GetMusicPool.InstantiateObject(NoteKeyCode.Line,
                    new Vector3(0f, 0f, PosZ), Quaternion.identity);
                        noteObject.Initialize(GameManager.Instance.Beat16Mat);
                        GameManager.GetNoteMover.AddNoteList(noteObject);
                    }
                }
                if (selectBeat >= 8)
                {
                    if (curBeat % 8 == 4)
                    {
                        NoteObject noteObject = GameManager.GetMusicPool.InstantiateObject(NoteKeyCode.Line,
                    new Vector3(0f, 0f, PosZ), Quaternion.identity);
                        noteObject.Initialize(GameManager.Instance.Beat8Mat);
                        GameManager.GetNoteMover.AddNoteList(noteObject);
                    }
                }
                if (selectBeat >= 4)
                {
                    if (curBeat % 8 == 0)
                    {
                        NoteObject noteObject = GameManager.GetMusicPool.InstantiateObject(NoteKeyCode.Line,
                    new Vector3(0f, 0f, PosZ), Quaternion.identity);
                        noteObject.Initialize(GameManager.Instance.Beat4Mat);
                        GameManager.GetNoteMover.AddNoteList(noteObject);
                    }
                }
            }
        }


        private void OnUpdate(float deltaTime)
        {
            if (GameManager.Instance.IsMusicLoaded.Equals(false))
            {
                return;
            }

            UIManaging.Instance.barCountTxt.text = GameManager.Instance.BarCount.ToString();
            UIManaging.Instance.beatCountTxt.text = GameManager.Instance.BeatCount.ToString();

            if (sliderClicked.Equals(false))
            {
                bgmSlider.slider.value = GameManager.GetMusic.timeRatioSample;
            }

            if(Input.mouseScrollDelta.y != 0f)
            {
                GameManager.Instance.PauseGame();

                GameManager.Instance.AddBeatCount(Convert.ToInt32(Input.mouseScrollDelta.y), selectBeat);

                GameManager.Instance.ReCalculateTimeSample();
                GameManager.Instance.InitMusicSample();
                GameManager.Instance.ResetBarPos();
            }

            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(GameManager.GetMusic.IsAudioPlay)
                {
                    OnClickedPauseBtn();
                }
                else
                {
                    OnClickedPlayBtn();
                }
            }
        }

        private void OnTick()
        {
            metronomSource.Play();

            box.rotation = new Quaternion { eulerAngles = box.rotation.eulerAngles + new Vector3(0f, 0f, -45f) };

            for (int beat = 1; beat < 32; beat++)
            {
                // PosZ 구하는 공식
                // 0.03125f 는 1비트당 비율값을 나타냄
                float posZ = GameManager.GetSpawnDistance + ((GameManager.Instance.BarDistance * 0.03125f) * beat);

                MakeBeatLine(beat, posZ);
            }
        }

        public void SaveData()
        {
            Sheet newSheet;
            List<NoteData> noteList;

            if (GameManager.GetSheet == null)
            {
                noteList = new List<NoteData>();
            }
            else
            {
                noteList = GameManager.GetSheet.noteDatas;
            }

            string path = $"{fileNameInput.text}";

            newSheet = new Sheet(path.ToLower(), bpm, offsetTime, noteList);

            GameManager.SetSheet(newSheet);


            if (DataManager.sheetDataStorage.SaveSheetData(path, GameManager.GetSheet))
            {
                if (GameManager.Instance.IsMusicLoaded)
                {
                    GameManager.Instance.InitMusic();
                    GameManager.Instance.SyncSheetAndNoteContainer();

                    GameManager.Instance.InitMusicSample();
                    GameManager.Instance.ResetBarPos();
                }
            }

            string buildPath = $"{Application.streamingAssetsPath}/{DataManagement.ResourceDataFolder}/{path.ToLower()}";

            DirectoryInfo di = new DirectoryInfo(buildPath);

            if(di.Exists.Equals(false))
            {
                di.Create();
            }
        }

        public void LoadData()
        {
            string path = $"{fileNameInput.text}";

            GameManager.Instance.LoadData(path);

            Sheet sheet = GameManager.GetSheet;

            bpm = sheet.BPM;
            offsetTime = sheet.Offset;
            fileNameInput.text = sheet.Name;

            audioWaveForm.Initialize();
        }

        public void OnClickedPlayBtn()
        {
            RemoveTextAll();
            GameManager.Instance.PlayGame();
        }
        public void OnClickedPauseBtn()
        {
            GameManager.Instance.AddBeatCount(0, selectBeat);
            GameManager.Instance.PauseGame();
        }
        public void OnClickedStopBtn()
        {
            if (GameManager.Instance.IsMusicLoaded)
            {
                GameManager.GetMusic.Stop();

                GameManager.Instance.InitMusicSample();
                GameManager.Instance.ResetBarPos();
            }
        }
        public void OnChangeNoteTypeDropDown(int i)
        {
            notePicker.SetKeyCode((NoteKeyCode)i);
        }

        private void OnDrawGizmos()
        {
            if (GameManager.Instance != null && GameManager.Instance.IsMusicLoaded && GameManager.GetMusic.IsAudioPlay.Equals(false) && IsGuideLineOn)
            {
                float halfBoxSize = boxSize * 0.5f;

                for (int y=0;y<6;y++)
                {
                    for(int x=0;x<8;x++)
                    {
                        // 임시 공식
                        Vector3 boxPos = new Vector3(boxSize * (x - 4) + halfBoxSize, boxSize * (y - 3) + halfBoxSize, 0f);
                        BoxGizmos(halfBoxSize, boxPos);
                    }
                }
            }
        }

        private void BoxGizmos(float halfBoxSize, Vector3 pos)
        {
            Vector3 UpLeft = new Vector3(pos.x - halfBoxSize, pos.y + halfBoxSize, 0f);
            Vector3 UpRight = new Vector3(pos.x + halfBoxSize, pos.y + halfBoxSize, 0f);
            Vector3 DownRight = new Vector3(pos.x + halfBoxSize, pos.y - halfBoxSize, 0f);
            Vector3 DownLeft = new Vector3(pos.x - halfBoxSize, pos.y - halfBoxSize, 0f);

            Gizmos.DrawLine(UpLeft, UpRight);
            Gizmos.DrawLine(UpRight, DownRight);
            Gizmos.DrawLine(DownRight, DownLeft);
            Gizmos.DrawLine(DownLeft, UpLeft);
        }
    }
}
