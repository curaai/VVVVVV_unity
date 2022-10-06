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
        public static bool IsPlayingNow = false;
        public override Type controlType => Type.UI;

        private PlayableDirector director;

        void Awake()
        {
            director = GetComponent<PlayableDirector>();
            OnSpace += resume;

            // hooking OnSpace in cutscene 
            var cutscene = GameObject.Find("CutScene").GetComponent<CutSceneController>();
            cutscene.OnSpace = null;
            director.stopped += (PlayableDirector d) => cutscene.OnSpace += cutscene.Close;
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