using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace VVVVVV.UI.Timeline.Warp
{
    [Serializable]
    public class WarpBehaviour : PlayableBehaviour
    {
        public GameObject ObjectToWarp;
        public Room TargetRoom;
        public Vector2 InRoomPos;

        private PlayableDirector director;
        private bool IsWarped = false;
        public override void OnPlayableCreate(Playable playable)
        {
            director = (playable.GetGraph().GetResolver() as PlayableDirector);
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (!IsWarped)
            {
                Game.Instance.ChangeRoom(TargetRoom.pos, InRoomPos);
                IsWarped = true;
            }
        }
    }
}