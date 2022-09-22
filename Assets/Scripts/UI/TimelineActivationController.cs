using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace VVVVVV.UI
{
    [RequireComponent(typeof(PlayableDirector))]
    public class TimelineActivationController : MonoBehaviour
    {
        private PlayableDirector director;

        void Awake()
        {
            director = GetComponent<PlayableDirector>();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && director.state == PlayState.Paused)
            {
                director.Resume();
            }
        }
    }
}