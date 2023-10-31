using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NewGame
{
    //Long note will be scored if player press and hold until end of note or it's will be failed if player release 
    public class LongNote : Note
    {
        protected float lineMovePercent = 0f, linePercentage = 0f;
        protected int lineLength;
        protected bool isHold;

        public override void Init(NoteData noteData)
        {
            base.Init(noteData);
            lineLength = noteData.longNoteLength;
            for (int i = 0; i < visual.lineArray.Length; i++)
            {
                UISprite line = visual.lineArray[i];
                line.height = visual.noteObject.GetComponent<UISprite>().height;
                line.width = lineLength;
            }
            for (int i = 0; i < visual.cornerArray.Length; i++)
            {
                visual.cornerArray[i].localPosition = cornerPositionData[i];
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            if (isHold) LongLineHolding();
        }

        protected override void DragOutHandler()
        {
            base.DragOutHandler();
        }

        protected override void InShowRangeHandler()
        {
            base.InShowRangeHandler();
            visual.lineHolder.gameObject.SetActive(true);
            visual.cornerHolder.gameObject.SetActive(true);
        }

        protected override void ReturnToPool()
        {
            visual.ScreenSprite.enabled = true;
            visual.cornerHolder.gameObject.SetActive(false);
            visual.lineHolder.localPosition = Vector3.zero;
            visual.lineHolder.gameObject.SetActive(false);
            base.ReturnToPool();
        }

        protected override void PressHandler()
        {
            visual.NotePosition = visual.ScreenButtonPosition;
            visual.ScreenSprite.enabled = false;

            isHold = true;
        }

        protected override void ReleaseHandler()
        {
            visual.PlayAnimation(true);
            isHold = false;
        }

        protected virtual void LongLineHolding()
        {
            visual.NotePosition = visual.ScreenButtonPosition;
            visual.lineHolder.localPosition += new Vector3(0, 0, -moveValue);

            lineMovePercent += moveValue;
            linePercentage = lineMovePercent / lineLength;

            for (int i = 0; i < visual.lineArray.Length; i++)
            {
                UISprite line = visual.lineArray[i];
                line.fillAmount = 1 - linePercentage;
            }
            for (int i = 0; i < visual.cornerArray.Length; i++)
            {
                visual.cornerArray[i].localPosition = Vector2.Lerp(cornerPositionData[i], Vector2.zero, linePercentage);
            }
            OnLineFinished();
        }

        protected virtual void OnLineFinished()
        {
            if (linePercentage >= 1f)
            {
                SuccessHit();
                ReleaseHandler();
            }
        }
    }

}
