using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace VVVVVV.Utils
{
    public static class AnimationHelper
    {
        public static IEnumerator CheckAnimationCompleted(Animator anim, string stateName, Action onComplete)
        {
            // check start
            while (!anim.GetCurrentAnimatorStateInfo(0).IsName(stateName))
                yield return null;
            // check End
            while (anim.GetCurrentAnimatorStateInfo(0).IsName(stateName))
                yield return null;

            if (onComplete != null)
                onComplete();
        }
    }
}