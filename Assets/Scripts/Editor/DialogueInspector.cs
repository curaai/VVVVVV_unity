using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.UIElements;
using UnityEditor;
using VVVVVV.UI.Timeline.Dialogue;

namespace VVVVVV.Editor
{
    [CustomEditor(typeof(DialogueClip))]
    public class DialogueInsepctor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var clip = (DialogueClip)target;
            clip.template.DialogueUIObject = EditorGUILayout.ObjectField(
            "Dialogue Object Field",
                clip.template.DialogueUIObject,
                typeof(GameObject),
                true) as GameObject;
        }
    }
}