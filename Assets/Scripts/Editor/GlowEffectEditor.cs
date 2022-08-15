using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using System.Collections;
using VVVVVV.Utils;
using VVVVVV.UI;
using VVVVVV.UI.Utils;

namespace VVVVVV.Editor
{
    [CustomEditor(typeof(GlowEffect))]
    public class GlowEffectEditor : UnityEditor.Editor
    {
        GlowEffect effect = null;
        void OnEnable()
        {
            effect = (GlowEffect)target;
        }
        public override void OnInspectorGUI()
        {
            GlowEffect component = (GlowEffect)target;

            EditorGUILayout.LabelField("Custom Glow Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            component.GlowOn = EditorGUILayout.Toggle("GlowOn: ", component.GlowOn);

            EditorGUI.indentLevel--;
            EditorGUILayout.LabelField("Color Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            EditorGUILayout.LabelField("On", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            component.OnRColorExpression = EditorGUILayout.TextField("OnRedColorExpression: ", component.OnRColorExpression);
            component.OnGColorExpression = EditorGUILayout.TextField("OnGreenColorExpression: ", component.OnGColorExpression);
            component.OnBColorExpression = EditorGUILayout.TextField("OnBlueColorExpression: ", component.OnBColorExpression);
            EditorGUI.indentLevel--;
            EditorGUILayout.LabelField("Off", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            component.OffRColorExpression = EditorGUILayout.TextField("OffRedColorExpression: ", component.OffRColorExpression);
            component.OffGColorExpression = EditorGUILayout.TextField("OffGreenColorExpression: ", component.OffGColorExpression);
            component.OffBColorExpression = EditorGUILayout.TextField("OffBlueColorExpression: ", component.OffBColorExpression);

            EditorGUI.indentLevel--;
        }
    }
}