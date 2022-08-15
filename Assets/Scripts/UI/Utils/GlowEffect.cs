using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using VVVVVV.Utils;

namespace VVVVVV.UI.Utils
{
    [RequireComponent(typeof(Graphic)), DisallowMultipleComponent]
    public class GlowEffect : MonoBehaviour
    {
        public bool GlowOn;

        public string OnRColorExpression;
        public string OnGColorExpression;
        public string OnBColorExpression;
        public string OffRColorExpression;
        public string OffGColorExpression;
        public string OffBColorExpression;

        private byte r, g, b;
        void Update()
        {
            if (GlowOn)
            {
                r = parse(OnRColorExpression);
                g = parse(OnGColorExpression);
                b = parse(OnBColorExpression);
            }
            else
            {
                r = parse(OffRColorExpression);
                g = parse(OffGColorExpression);
                b = parse(OffBColorExpression);
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