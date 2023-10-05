using UnityEngine;
using System.Collections.Generic;

namespace EditorScript
{
    public class NotePicker : MonoBehaviour
    {
        private Camera mainCam;

        [SerializeField]
        private NoteKeyCode curNoteKeyCode;
        public void SetKeyCode(NoteKeyCode key) { curNoteKeyCode = key; }

        private NoteObject marker;

        [SerializeField]
        private float BoxXSize = 6.4f;
        [SerializeField]
        private float BoxYSize = 4.77f;
        [SerializeField]
        private float NoteSize = 0.7f;

        int beforeBeat;

        public void Initialize()
        {
            mainCam = Camera.main;

            marker = GameManager.GetMusicPool.InstantiateObject(NoteKeyCode.Default, Vector3.zero, Quaternion.identity);
            marker.gameObject.SetActive(false);
        }

        public void UpdatePicker(float deltaTime)
        {
            if (GameManager.Instance.IsMusicLoaded && GameManager.GetMusic.IsAudioPlay.Equals(false))
            {
                if (marker.gameObject.activeSelf.Equals(false))
                {
                    marker.gameObject.SetActive(true);
                }

                int beat = GameManager.Instance.BeatCount;
                if (beforeBeat != beat)
                {
                    marker.Initialize(GameManager.Instance.PickMaterial(beat));

                    beforeBeat = GameManager.Instance.BeatCount;
                }

                Vector3 mousePos = Input.mousePosition;
                Vector3 pos = mainCam.ScreenToWorldPoint(mousePos);
                pos.z = 0f;

                if (pos.x < BoxXSize && pos.x > -BoxXSize &&
                       pos.y > -BoxYSize && pos.y < BoxYSize)
                {
                    // 마우스가 무조건 안에 있게
                    pos.x = Mathf.Clamp(pos.x, -BoxXSize + NoteSize, BoxXSize - NoteSize);
                    pos.y = Mathf.Clamp(pos.y, -BoxYSize + NoteSize, BoxYSize - NoteSize);

                    if (EditorManager.Instance.IsGuideLineOn)
                    {
                        float boxSize = EditorManager.Instance.GetBoxSize;
                        float halfBoxSize = boxSize * 0.5f;
                        float boxOffsetX = boxSize * 4f;
                        float boxOffsetY = boxSize * 3f;

                        int x = (int)((pos.x + boxOffsetX) / boxSize);
                        int y = (int)((pos.y + boxOffsetY) / boxSize);

                        Vector3 boxPos = new Vector3(boxSize * (x - 4) + halfBoxSize, boxSize * (y - 3) + halfBoxSize, 0f);

                        pos = boxPos;
                    }

                    marker.thisTrf.position = pos;

                    if (Input.GetMouseButtonUp(0))
                    {
                        PickNote(pos);
                        GameManager.Instance.SyncSheetAndNoteContainer();
                        GameManager.Instance.ResetBarPos();
                    }
                    if (Input.GetMouseButtonUp(1))
                    {
                        EraseNote(pos, 1f);
                        GameManager.Instance.SyncSheetAndNoteContainer();
                        GameManager.Instance.ResetBarPos();
                    }
                }
                else
                {
                    if (marker.gameObject.activeSelf.Equals(true))
                    {
                        marker.gameObject.SetActive(false);
                    }
                }
            }
            else
            {
                if (marker.gameObject.activeSelf.Equals(true))
                {
                    marker.gameObject.SetActive(false);
                }
            }
        }

        private void PickNote(Vector3 pos)
        {
            int longNoteIndex;
            if (curNoteKeyCode == NoteKeyCode.Line ||
                curNoteKeyCode == NoteKeyCode.LongMesh)
            {
                return;
            }
            else if (curNoteKeyCode == NoteKeyCode.LongStart || curNoteKeyCode == NoteKeyCode.LongEnd)
            {
                longNoteIndex = int.Parse(UIManaging.Instance.longNoteIndex.text);
            }
            else
            {
                longNoteIndex = -1;
            }

            NoteData noteData = new NoteData(pos.x, pos.y, 
                GameManager.Instance.BarCount, GameManager.Instance.BeatCount, 
                curNoteKeyCode, longNoteIndex);
            GameManager.GetSheet.noteDatas.Add(noteData);
        }

        private void EraseNote(Vector3 pos, float range)
        {
            List<NoteData> listTmp = GameManager.GetSheet.noteDatas;

            for (int i = 0; i < listTmp.Count; i++)
            {
                NoteData data = listTmp[i];
                if (data.PosX < pos.x + range && data.PosX > pos.x - range &&
                    data.PosY < pos.y + range && data.PosY > pos.y - range &&
                    data.BarIndex == GameManager.Instance.BarCount &&
                    data.BeatIndex == GameManager.Instance.BeatCount)
                {
                    listTmp.RemoveAt(i);
                    break;
                }
            }
        }
    }
}
