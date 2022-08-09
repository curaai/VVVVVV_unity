using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace VVVVVV
{
    public class GlowColorAnimation : MonoBehaviour
    {
        // TODO: Set color in other place, Only generate color in here 
        [SerializeField] private SpriteRenderer playerRenderer;
        [SerializeField] private Text roomnameText;

        public Color playerNormal => new Color32((byte)(160 - glow / 2 - fRand() * 20), (byte)(200 - glow / 2), (byte)(220 - glow), 255);
        public Color roomname => new Color32(196, 196, (byte)(255 - glow), 255);

        public bool glowDirection = false;
        public int glow { get; private set; }

        void Update()
        {
            glow += 2 * (glowDirection ? 1 : -1);

            if (glowDirection && 62 <= glow) glowDirection = false;
            if (!glowDirection && glow < 2) glowDirection = true;


            roomnameText.color = roomname;
            playerRenderer.material.color = playerNormal;
        }

        public float fRand() => Convert.ToSingle(new System.Random().NextDouble());
    }
}
