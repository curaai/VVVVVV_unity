using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.UIElements;
using UnityEditor;
using VVVVVV.UI.Timeline.Warp;

namespace VVVVVV.Editor
{
    [CustomEditor(typeof(WarpClip))]
    public class WarpInsepctor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            var clip = (WarpClip)target;
            clip.template.ObjectToWarp = (GameObject)EditorGUILayout.ObjectField(
            "Warp Object",
                clip.template.ObjectToWarp,
                typeof(GameObject),
                true); // allowSceneObjects

            clip.template.TargetRoom = (Room)EditorGUILayout.ObjectField(
            "Room Object",
                clip.template.TargetRoom,
                typeof(Room),
                true); // allowSceneObjects

            //  (string label, Vector2 value, params GUILayoutOption[] options);

            clip.template.InRoomPos = (Vector2)EditorGUILayout.Vector2Field("LocalPosition in Room", clip.template.InRoomPos);
        }
    }
}