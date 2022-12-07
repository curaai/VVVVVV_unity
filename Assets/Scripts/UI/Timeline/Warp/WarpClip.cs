using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace VVVVVV.UI.Timeline.Warp
{
    [Serializable]
    public class WarpClip : PlayableAsset, ITimelineClipAsset
    {
        public WarpBehaviour template = new WarpBehaviour();
        public ClipCaps clipCaps { get => ClipCaps.None; }
        public override double duration { get => 0.5f; }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<WarpBehaviour>.Create(graph, template);
        }
    }
}