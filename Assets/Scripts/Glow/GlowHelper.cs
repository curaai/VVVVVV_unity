using UnityEngine;

namespace VVVVVV.Glow
{
    [DisallowMultipleComponent]
    public class GlowHelper: Utils.Singleton<GlowHelper>
    {
        public static int glow { get; private set; }
        private bool glowDirection = false;

        void Update()
        {
            glow += 2 * (glowDirection ? 1 : -1);

            if (glowDirection && 62 <= glow) glowDirection = false;
            if (!glowDirection && glow < 2) glowDirection = true;
        }
    }
}
