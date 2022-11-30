using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Playables;

namespace VVVVVV.UI.Timeline
{
    [Serializable]
    public class DialogueClip : PlayableAsset, ITimelineClipAsset
    {
        public DialogueBehaviour template = new DialogueBehaviour();
        public ClipCaps clipCaps { get => ClipCaps.None; }
        public override double duration { get => 2f; }

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            return ScriptPlayable<DialogueBehaviour>.Create(graph, template);
        }
    }
}