using UnityEngine;
using UnityEngine.UI;

namespace VVVVVV.Glow
{
    [RequireComponent(typeof(Text))]
    public class TextFocusGlow : CanvasGlow
    {
        private string originalString = string.Empty;

        public override bool GlowOn
        {
            get => base.GlowOn; set
            {
                base.GlowOn = value;

                var textObj = ((Text)graphic);
                if (textObj == null) return;
                if (value)
                    textObj.text = $"[ {originalString.ToUpper()} ]";
                else
                    textObj.text = originalString;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            originalString = ((Text)graphic).text;
            // Validate when load from scene
            GlowOn = GlowOn;
        }
    }
}