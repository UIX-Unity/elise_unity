using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NewGame
{
    public class BasicNote : Note
    {
        public override void Init(NoteData noteData)
        {
            base.Init(noteData);
        }

        protected override void InShowRangeHandler()
        {
            base.InShowRangeHandler();
        }
        
        protected override void ReturnToPool()
        {
            base.ReturnToPool();
        }

        protected override void PressHandler()
        {
            CalculateScoreTap();
            
            visual.PlayAnimation(true);
            pressed = false;
        }

        protected override void ReleaseHandler()
        {

        }

        private void CalculateScoreTap()
        {
            SuccessHit();
        }
    }

}
