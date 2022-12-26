using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace VVVVVV.Glow
{
    [DisallowMultipleComponent]
    public abstract class GlowBase : MonoBehaviour
    {
        [SerializeField] private GlowExpression exp;
        [SerializeField] private bool _GlowOn;
        public virtual bool GlowOn
        {
            get => _GlowOn;
            set => _GlowOn = value;
        }

        // When scene load assign inherit Glow
        private void OnValidate() => GlowOn = _GlowOn;

        protected Color32 ParseGlow()
        {
            // srand() => Shared Random term in exp
            // frand() => 0~1 ranged random term 
            var exp = GlowOn ? this.exp.OnExp : this.exp.OffExp;
            var srand = fRand();

            byte parse(string exp)
            {
                var glow = GlowHelper.glow;

                exp = exp.Replace("glow", glow.ToString());
                exp = exp.Replace("frand()", fRand().ToString());
                exp = exp.Replace("srand()", srand.ToString());
                ExpressionEvaluator.Evaluate(exp, out float res);
                return (byte)Mathf.FloorToInt(res);
            }

            var r = parse(exp.r);
            var g = parse(exp.g);
            var b = parse(exp.b);
            return new Color32(r, g, b, 255);
        }

        private static float fRand() => UnityEngine.Random.Range(0, 1f);
    }
}