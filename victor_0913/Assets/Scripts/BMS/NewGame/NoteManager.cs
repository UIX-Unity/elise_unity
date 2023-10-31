using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace NewGame
{
    public enum SwipeDirection { Up = 1, Down = 3, Right = 2, Left = 4 }
    public enum NoteType { BasicNote = 0, LongNote = 1, DirectionNote = 2, LongDirectionNote = 3 }
    [System.Serializable]
    public struct NoteData
    {
        public NoteType noteType;
        public Color noteColor;
        public Vector3 notePosition;
        public Vector3 noteRotation;
        public int longNoteLength;
        public SwipeDirection swipeDirection;

        public int noteID; // For debugging
    }
    //This class control how notes in the screen pooling and controls the initiation of notes
    public class NoteManager : MonoBehaviour
    {
        [SerializeField] private NewGameScene newGameScene;
        [SerializeField] private List<NoteData> noteDataList = new List<NoteData>();
        [SerializeField] private Transform noteHolder;
        [SerializeField] private GameObject noteObject;
        private ObjectPool notePool;
        private GameSetting info;
        private void Awake()
        {
            info = Resources.Load("GameSetting") as GameSetting;
            notePool = ObjectPool.CreateInstance(noteObject, (int)info.BPM);
            noteObject.gameObject.SetActive(false);
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                List<GameObject> test = notePool.GetAvailableObjectsPool();
                Debug.Log("" + test.Count);
            }
            if (newGameScene.GameState == GameState.Playing)
            {
                if (notePool.GetAvailableObjectsPool().Any())
                {
                    if (noteDataList.Count == 0)
                    {
                        return;
                    }
                    NoteData noteData = noteDataList[noteDataList.Count - 1];
                    noteDataList.Remove(noteData);

                    switch (noteData.noteType)
                    {
                        case NoteType.BasicNote:
                            InitNoteData<BasicNote>(noteData);
                            break;
                        case NoteType.LongNote:
                            InitNoteData<LongNote>(noteData);
                            break;
                        case NoteType.DirectionNote:
                            InitNoteData<DirectionNote>(noteData);
                            break;
                        case NoteType.LongDirectionNote:
                            InitNoteData<LongDirectionNote>(noteData);
                            break;
                    }
                }
            }
        }

        public void InitNoteData<T>(NoteData noteData) where T : Note
        {
            T noteInstance = notePool.GetPooledObject<T>();
            notePool.RemovePoolableObject(noteInstance.gameObject);

            noteInstance.transform.SetParent(noteHolder, false);
            noteInstance.transform.localPosition = Vector3.zero;
            noteInstance.gameObject.SetActive(true);
            noteInstance.Init(noteData);

        }
        public void AddNoteData(NoteData noteData)
        {
            this.noteDataList.Add(noteData);
        }
        public void RemoveNoteData(NoteData noteData)
        {
            this.noteDataList.Remove(noteData);
        }
        public void SortNoteData()
        {
            noteDataList.Sort((A, B) => B.notePosition.z.CompareTo(A.notePosition.z));
        }
    }
}
