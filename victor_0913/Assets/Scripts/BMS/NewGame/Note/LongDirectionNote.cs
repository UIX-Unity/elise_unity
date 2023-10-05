using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NewGame;
using System;

namespace NewGame
{
    public class LongDirectionNote : LongNote
    {
        private SwipeDirection currentSwipeDir;
        private SwipeDirection correctSwipeDir;

        public override void Init(NoteData noteData)
        {
            base.Init(noteData);
            correctSwipeDir = noteData.swipeDirection;

            Vector3 directionHolderPos = new Vector3(0f, 0f, lineLength);
            visual.directionHolder.localPosition = directionHolderPos;
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
        }

        protected override void InShowRangeHandler()
        {
            base.InShowRangeHandler();
            visual.directionArray[(int)correctSwipeDir - 1].gameObject.SetActive(true);
        }

        protected override void ReturnToPool()
        {
            visual.directionHolder.gameObject.SetActive(false);
            visual.directionArray[(int)correctSwipeDir - 1].gameObject.SetActive(false);
            base.ReturnToPool();
        }

        protected override void PressHandler()
        {
            base.PressHandler();
            visual.directionHolder.gameObject.SetActive(true);
            pressedPosition = Input.mousePosition;
        }

        protected override void ReleaseHandler()
        {
            base.ReleaseHandler();
        }

        protected override void LongLineHolding()
        {
            visual.directionHolder.localPosition += new Vector3(0, 0, -moveValue);
            base.LongLineHolding();
        }

        protected override void OnLineFinished()
        {
            if (linePercentage >= 1f)
            {
                releasePosition = Input.mousePosition;
                Vector3 dragVectorDirection = (releasePosition - pressedPosition).normalized;
                currentSwipeDir = GetDragDirection(dragVectorDirection);
                if (currentSwipeDir == correctSwipeDir)
                {
                    SuccessHit();
                }
                ReleaseHandler();
            }
        }
    }
}

