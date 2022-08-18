using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using VVVVVV.Utils;

namespace VVVVVV.UI.Utils
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

        private byte r, g, b;
        void Update()
        {
            if (GlowOn)
            {
                r = parse(on.r);
                g = parse(on.g);
                b = parse(on.b);
            }
            else
            {
                r = parse(off.r);
                g = parse(off.g);
                b = parse(off.b);
            }
            GetComponent<Graphic>().color = new Color32(r, g, b, 255);
        }

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
    }
}