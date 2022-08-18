using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using VVVVVV.Utils;

namespace VVVVVV.UI.Utils.Glow
{
    [RequireComponent(typeof(SpriteRenderer)), DisallowMultipleComponent]
    public class SpriteGlowEffect : MonoBehaviour
    {
        public bool GlowOn;
        public GlowExpression on;
        public GlowExpression off;

        void Update()
        {
            GetComponent<SpriteRenderer>().color = GlowEffect.ParseGlow(GlowOn ? on : off);
        }
    }
}