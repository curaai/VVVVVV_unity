using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using VVVVVV.Utils;

namespace VVVVVV.UI.Utils.Glow
{
    [RequireComponent(typeof(SpriteRenderer)), DisallowMultipleComponent]
    public class SpriteGlowListEffect : MonoBehaviour
    {
        public int idx;
        public List<GlowExpression> expressions;

        void Update()
        {
            GetComponent<SpriteRenderer>().color = GlowEffect.ParseGlow(expressions[idx]);
        }
    }
}