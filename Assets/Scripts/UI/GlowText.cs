using UnityEngine;
using UnityEngine.UI;
using UnityEditor;


namespace VVVVVV.UI
{
    public class GlowText : UnityEngine.UI.Text
    {
        public bool EnableGlow = true;
        public bool EnablePadding = true;

        public string originalText;
        public bool glowOn;

        public string onRColorExpression;
        public string onGColorExpression;
        public string onBColorExpression;
        public string offRColorExpression;
        public string offGColorExpression;
        public string offBColorExpression;

        public GlowText() : base()
        {
            originalText = text;
        }

        protected override void Start()
        {
            base.Start();
        }

        void Update()
        {
            if (!EnableGlow)
                return;

            if (glowOn)
            {
                var r = parse(onRColorExpression);
                var g = parse(onGColorExpression);
                var b = parse(onBColorExpression);
                this.color = new Color32(r, g, b, 255);
            }
            else
            {
                var r = parse(offRColorExpression);
                var g = parse(offGColorExpression);
                var b = parse(offBColorExpression);
                this.color = new Color32(r, g, b, 255);
            }

            if (glowOn && EnablePadding)
            {
                text = $"[ {originalText.ToUpper()} ]";
            }
            else
            {
                text = originalText;
            }
        }

        byte parse(string exp)
        {
            var glow = Utils.GlowColorAnimation.glow;
#if UNITY_EDITOR
            if (glow == null) glow = 0;
#endif
            var frand = Utils.RandomHelper.fRand();

            exp = exp.Replace("glow", glow.ToString());
            exp = exp.Replace("frand()", frand.ToString());
            ExpressionEvaluator.Evaluate(exp, out float res);
            return (byte)Mathf.FloorToInt(res);
        }
    }
}