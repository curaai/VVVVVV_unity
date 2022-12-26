using System;
using UnityEngine;

namespace VVVVVV.Glow
{
    [CreateAssetMenu(fileName = "Glow Expression", menuName ="ScriptableObject/Glow Expression")]
    public class GlowExpression : ScriptableObject
    {
        [Serializable]
        public struct Exp
        {
            public string r;
            public string g;
            public string b;
        }

        public Exp OnExp;
        public Exp OffExp;
    }
}