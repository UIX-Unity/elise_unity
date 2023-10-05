using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace EditorScript
{
    public class UIManaging : BaseMonoSingleton<UIManaging>
    {
        [SerializeField]
        private Transform m_InfoCanvas;
        public Transform infoCanvas => m_InfoCanvas;

        [SerializeField]
        private CustomButton m_SaveBtn;
        public CustomButton saveBtn => m_SaveBtn;

        [SerializeField]
        private CustomButton m_LoadBtn;
        public CustomButton loadBtn => m_LoadBtn;

        [SerializeField]
        private CustomButton m_PlayBtn;
        public CustomButton playBtn => m_PlayBtn;

        [SerializeField]
        private CustomButton m_PauseBtn;
        public CustomButton pauseBtn => m_PauseBtn;

        [SerializeField]
        private CustomButton m_StopBtn;
        public CustomButton stopBtn => m_StopBtn;

        [Header("마디 컨트롤"), Space(10)]
        [SerializeField]
        private TextMeshProUGUI m_BarCountTxt;
        public TextMeshProUGUI barCountTxt => m_BarCountTxt;

        [SerializeField]
        private CustomButton m_BarLeftBtn;
        public CustomButton barLeftBtn => m_BarLeftBtn;

        [SerializeField]
        private CustomButton m_BarRigthtBtn;
        public CustomButton barRigthtBtn => m_BarRigthtBtn;

        [Header("박자 컨트롤"), Space(10)]
        [SerializeField]
        private TextMeshProUGUI m_BeatCountTxt;
        public TextMeshProUGUI beatCountTxt => m_BeatCountTxt;

        [SerializeField]
        private CustomButton m_BeatLeftBtn;
        public CustomButton beatLeftBtn => m_BeatLeftBtn;

        [SerializeField]
        private CustomButton m_BeatRigthtBtn;
        public CustomButton beatRigthtBtn => m_BeatRigthtBtn;

        [Header("비트 선택"), Space(10)]
        [SerializeField]
        private CustomButton m_Beat4Btn;
        public CustomButton Beat4Btn => m_Beat4Btn;

        [SerializeField]
        private CustomButton m_Beat8Btn;
        public CustomButton Beat8Btn => m_Beat8Btn;

        [SerializeField]
        private CustomButton m_Beat16Btn;
        public CustomButton Beat16Btn => m_Beat16Btn;

        [SerializeField]
        private CustomButton m_Beat32Btn;
        public CustomButton Beat32Btn => m_Beat32Btn;

        [Header("가이드 라인"), Space(10)]
        [SerializeField]
        private Toggle m_GuideLineTog;
        public Toggle guideLineTog => m_GuideLineTog;

        [Header("비트 라인"), Space(10)]
        [SerializeField]
        private Toggle m_BeatLineTog;
        public Toggle beatLineTog => m_BeatLineTog;

        [Header("슬라이더 백그라운드"), Space(10)]
        [SerializeField]
        private Image m_SliderBackGround;
        public Image sliderBackGround => m_SliderBackGround;

        [Header("노트 타입 선택"), Space(10)]
        [SerializeField]
        private TMP_Dropdown m_NoteTypeDropDown;
        public TMP_Dropdown noteTypeDropDown => m_NoteTypeDropDown;

        [Header("롱노트 인덱스"), Space(10)]
        [SerializeField]
        private TMP_InputField m_LongNoteIndex;
        public TMP_InputField longNoteIndex => m_LongNoteIndex;

        public void Initialize()
        {
            // UI Btn Init
            saveBtn.onClick.AddListener(EditorManager.Instance.SaveData);
            loadBtn.onClick.AddListener(EditorManager.Instance.LoadData);
            playBtn.onClick.AddListener(EditorManager.Instance.OnClickedPlayBtn);
            pauseBtn.onClick.AddListener(EditorManager.Instance.OnClickedPauseBtn);
            stopBtn.onClick.AddListener(EditorManager.Instance.OnClickedStopBtn);

            // Bar Controller Init
            barLeftBtn.onClick.AddListener(EditorManager.Instance.OnClickedBarLeftBtn);
            barRigthtBtn.onClick.AddListener(EditorManager.Instance.OnClickedBarRightBtn);

            // Beat Controller Init
            beatLeftBtn.onClick.AddListener(EditorManager.Instance.OnClickedBeatLeftBtn);
            beatRigthtBtn.onClick.AddListener(EditorManager.Instance.OnClickedBeatRightBtn);

            // Beat Selector Init
            Beat4Btn.onClick.AddListener(EditorManager.Instance.OnClick4BeatBtn);
            Beat8Btn.onClick.AddListener(EditorManager.Instance.OnClick8BeatBtn);
            Beat16Btn.onClick.AddListener(EditorManager.Instance.OnClick16BeatBtn);
            Beat32Btn.onClick.AddListener(EditorManager.Instance.OnClick32BeatBtn);

            // BeatLine Toggle Init
            beatLineTog.onValueChanged.AddListener(EditorManager.Instance.ChangeBeatLineTog);

            // GuideLine Toggle Init
            guideLineTog.onValueChanged.AddListener(EditorManager.Instance.ChangeGuideLineTog);

            // DropDown Init
            List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

            for(int i=0;i<Enum.GetValues(typeof(NoteKeyCode)).Length;i++)
            {
                TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
                option.text = ((NoteKeyCode)i).ToString();

                options.Add(option);
            }

            m_NoteTypeDropDown.AddOptions(options);
            m_NoteTypeDropDown.onValueChanged.AddListener(EditorManager.Instance.OnChangeNoteTypeDropDown);

            m_NoteTypeDropDown.value = (int)NoteKeyCode.Default;
        }
    }
}

