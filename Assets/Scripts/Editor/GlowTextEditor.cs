using UnityEngine;
using UnityEditor;
using UnityEditor.UI;
using System.Collections;
using VVVVVV.Utils;
using VVVVVV.UI;
using VVVVVV.UI.Utils;

namespace VVVVVV.Editor
{
    [CustomEditor(typeof(GlowText))]
    public class GlowTextEditor : UnityEditor.UI.TextEditor
    {
        public override void OnInspectorGUI()
        {
            GlowText component = (GlowText)target;

            EditorGUILayout.LabelField("Custom Glow Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            component.glowOn = EditorGUILayout.Toggle("glowOn: ", component.glowOn);
            component.EnableGlow = EditorGUILayout.Toggle("EnableGlow: ", component.EnableGlow);
            component.EnablePadding = EditorGUILayout.Toggle("EnablePadding: ", component.EnablePadding);
            component.originalText = EditorGUILayout.TextField("OriginalText: ", component.originalText);

            EditorGUI.indentLevel--;
            EditorGUILayout.LabelField("Color Settings", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            component.onRColorExpression = EditorGUILayout.TextField("OnRedColorExpression: ", component.onRColorExpression);
            component.onGColorExpression = EditorGUILayout.TextField("OnGreenColorExpression: ", component.onGColorExpression);
            component.onBColorExpression = EditorGUILayout.TextField("OnBlueColorExpression: ", component.onBColorExpression);
            component.offRColorExpression = EditorGUILayout.TextField("OffRedColorExpression: ", component.offRColorExpression);
            component.offGColorExpression = EditorGUILayout.TextField("OffGreenColorExpression: ", component.offGColorExpression);
            component.offBColorExpression = EditorGUILayout.TextField("OffBlueColorExpression: ", component.offBColorExpression);

            EditorGUI.indentLevel--;
            base.OnInspectorGUI();
        }
    }
}