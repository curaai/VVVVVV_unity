using UnityEngine;
using UnityEngine.UI;

namespace VVVVVV.Glow
{
    [RequireComponent(typeof(Graphic))]
    public class CanvasGlow: GlowBase
    {
        protected Graphic graphic;

        protected virtual void Awake()
        {
            graphic = GetComponent<Graphic>();
        }

        protected virtual void Update()
        {
            graphic.color = ParseGlow();
        }
    }
}