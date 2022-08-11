using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace VVVVVV.Utils
{
    [DisallowMultipleComponent]
    public class GlowColorAnimation : MonoBehaviour
    {
        // TODO: Set color in other place, Only generate color in here 
        [SerializeField] private SpriteRenderer playerRenderer;

        public Color playerNormal => new Color32((byte)(160 - glow / 2 - RandomHelper.fRand() * 20), (byte)(200 - glow / 2), (byte)(220 - glow), 255);

        public bool glowDirection = false;
        public static int glow { get; private set; }

        void Update()
        {
            glow += 2 * (glowDirection ? 1 : -1);

            if (glowDirection && 62 <= glow) glowDirection = false;
            if (!glowDirection && glow < 2) glowDirection = true;

            playerRenderer.material.color = playerNormal;
        }
    }
}
