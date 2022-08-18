using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace VVVVVV.Utils
{
    [DisallowMultipleComponent]
    public class GlowColorAnimation : MonoBehaviour
    {
        public bool glowDirection = false;
        public static int glow { get; private set; }

        void Update()
        {
            glow += 2 * (glowDirection ? 1 : -1);

            if (glowDirection && 62 <= glow) glowDirection = false;
            if (!glowDirection && glow < 2) glowDirection = true;
        }
    }
}
