using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace VVVVVV.Utils
{
    public static class AnimatorHelper
    {
        public static IEnumerator SetAnimStateCallback(Animator anim, string stateName, Action onComplete)
        {
            bool isCurrentStateName(string stateName) => anim.GetCurrentAnimatorStateInfo(0).IsName(stateName);
            bool isPlayDone() => 1 <= anim.GetCurrentAnimatorStateInfo(0).normalizedTime;

            // check start
            while (!isCurrentStateName(stateName))
                yield return null;
            // check End
            while (isCurrentStateName(stateName) && !isPlayDone())
                yield return null;

            if (onComplete != null)
                onComplete();
        }
    }
}