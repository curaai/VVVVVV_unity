using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace VVVVVV.UI
{
    [RequireComponent(typeof(PlayableDirector))]
    public class TimelineActivationController : IControllable
    {
        public override Type controlType => Type.UI;

        private PlayableDirector director;

        void Awake()
        {
            director = GetComponent<PlayableDirector>();
            OnSpace += resume;
        }

        void OnEnable() => FocusNow = true;
        void OnDisable() => FocusNow = false;

        void resume()
        {
            if (director.state == PlayState.Paused)
                director.Resume();
        }
    }
}