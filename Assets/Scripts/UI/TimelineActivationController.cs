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

            var cutscene = GameObject.Find("CutScene").GetComponent<CutSceneController>();
            cutscene.Open(GetComponentInParent<Room>().UI);
            director.stopped += (PlayableDirector d) => cutscene.Close();

            // hooking OnSpace in cutscene 
            cutscene.OnSpace = null;
            // rollback original hooked OnSpace of cutscene
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