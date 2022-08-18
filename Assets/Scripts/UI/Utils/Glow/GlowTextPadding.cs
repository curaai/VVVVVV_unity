using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using VVVVVV.Utils;

namespace VVVVVV.UI.Utils.Glow
{
    [RequireComponent(typeof(Text))]
    [RequireComponent(typeof(GlowEffect))]
    public class GlowTextPadding : MonoBehaviour
    {
        private string oriText;
        private GlowEffect effect => GetComponent<GlowEffect>();
        private Text text => GetComponent<Text>();
        void Awake()
        {
            oriText = text.text;
        }

        void OnEnable()
        {
            if (effect.GlowOn)
            {
                oriText = text.text;
                text.text = $"[ {text.text.ToUpper()} ]";
            }
            else
            {
                text.text = oriText;
            }
        }
        void OnDisable()
        {
            text.text = oriText;
        }
    }
}