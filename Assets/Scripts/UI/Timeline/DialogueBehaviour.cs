using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace VVVVVV.UI.Timeline
{
    /*
    Dialouge Pipeline 

    0. Timeline Start - call pause 
    1. clip(this) play - call play
    2. clip(this) process and pause - pause timeline in ProcessFrame
    3. clip(this) pause - call pause
    4. Timeline fetch input and resume 
    5. clip(this) play - call play (resume)
    6. clip(this) end - call pause (deactive dialouge)
    7. clip(another) ~ ~ ~ 
    8. Timeline Stop - call pause (all behaviours(each clip) in track)
    */

    [Serializable]
    public class DialogueBehaviour : PlayableBehaviour
    {
        [SerializeField] private double TimeToPause = 0.5f;
        public GameObject DialogueUIObject;

        private PlayableDirector director;
        private bool oncePaused = false;
        private bool resumed = false;

        public override void OnPlayableCreate(Playable playable)
        {
            director = (playable.GetGraph().GetResolver() as PlayableDirector);
        }

        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            if (oncePaused) resumed = true;
            if (DialogueUIObject != null) DialogueUIObject.SetActive(true);
            base.OnBehaviourPlay(playable, info);
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (resumed && DialogueUIObject != null) DialogueUIObject.SetActive(false);
            base.OnBehaviourPause(playable, info);
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (director.state != PlayState.Paused && !oncePaused && TimeToPause < playable.GetTime())
            {
                director.Pause();
                oncePaused = true;
            }
        }
    }
}