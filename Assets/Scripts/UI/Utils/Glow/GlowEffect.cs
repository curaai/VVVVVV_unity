using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using VVVVVV.Utils;

namespace VVVVVV.UI.Utils.Glow
{
    [Serializable]
    public class GlowExpression
    {
        public string r;
        public string g;
        public string b;
    }

    [RequireComponent(typeof(Graphic)), DisallowMultipleComponent]
    public class GlowEffect : MonoBehaviour
    {
        public bool GlowOn;
        public GlowExpression on;
        public GlowExpression off;

        void Update()
        {
            GetComponent<Graphic>().color = ParseGlow(GlowOn ? on : off);
        }

        public static Color32 ParseGlow(GlowExpression exp)
        {
            byte parse(string exp)
            {
                var glow = GlowColorAnimation.glow;
#if UNITY_EDITOR
                if (glow == null) glow = 0;
#endif
                var frand = RandomHelper.fRand();

                exp = exp.Replace("glow", glow.ToString());
                exp = exp.Replace("frand()", frand.ToString());
                ExpressionEvaluator.Evaluate(exp, out float res);
                return (byte)Mathf.FloorToInt(res);
            }

            var r = parse(exp.r);
            var g = parse(exp.g);
            var b = parse(exp.b);
            return new Color32(r, g, b, 255);
        }
    }
}