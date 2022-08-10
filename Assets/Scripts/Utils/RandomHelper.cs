using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

namespace VVVVVV.Utils
{
    public class RandomHelper
    {
        public static float fRand() => Random.Range(0, 1f);
    }
}