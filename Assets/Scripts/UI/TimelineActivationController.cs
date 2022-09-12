using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace VVVVVV.UI
{
    public class TimelineActivationController : MonoBehaviour
    {
        private PlayableDirector director;

        void Start()
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