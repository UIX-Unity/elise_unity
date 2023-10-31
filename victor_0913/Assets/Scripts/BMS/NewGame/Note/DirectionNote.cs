using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace NewGame
{
    //With direction note player have to swipe the direction to match the direction in the note to get score
    public class DirectionNote : Note
    {
        private SwipeDirection swipeDirection;
        private SwipeDirection correctSwipeDirection;
        private bool isSwiping;

        public override void Init(NoteData noteData)
        {
            base.Init(noteData);
            correctSwipeDirection = noteData.swipeDirection;
        }

        protected override void InShowRangeHandler()
        {
            base.InShowRangeHandler();
            visual.directionHolder.gameObject.SetActive(true);
            visual.directionArray[(int)correctSwipeDirection - 1].gameObject.SetActive(true);
        }

        protected override void ReturnToPool()
        {
            visual.directionHolder.gameObject.SetActive(false);
            visual.directionArray[(int)correctSwipeDirection - 1].gameObject.SetActive(false);
            isSwiping = false;
            base.ReturnToPool();

        }

        protected override void PressHandler()
        {
            pressedPosition = Input.mousePosition;
            isSwiping = true;
        }

        protected override void ReleaseHandler()
        {
            if (isSwiping)
            {
                releasePosition = Input.mousePosition;
                Vector3 dragVectorDirection = (releasePosition - pressedPosition).normalized;
                swipeDirection = GetDragDirection(dragVectorDirection);
                CalculateScoreSwipe();
                visual.PlayAnimation(true);
            }
        }

        private void CalculateScoreSwipe()
        {
            if (swipeDirection == correctSwipeDirection)
            {
                SuccessHit();
            }
        }
    }

}
