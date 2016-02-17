using UnityEditor;
using UnityEngine;

[CustomEditor(typeof (UnitManagement))]
// ReSharper disable once CheckNamespace
public class UnitManagementEditor : Editor {
    public override void OnInspectorGUI() {
        serializedObject.Update();
        var rect = GUILayoutUtility.GetRect(0, 20, GUIStyle.none);
        rect.xMin = rect.xMax - 85;
        EditorGUI.LabelField(rect, "Alt  Ctrl  Shift");

        var select = serializedObject.FindProperty("NextSubgroup");
        var add = serializedObject.FindProperty("PreviousSubgroup");
        var addAndRemove = serializedObject.FindProperty("Select");
        var assign = serializedObject.FindProperty("CancelDragSelect");
        var assignAndRemove = serializedObject.FindProperty("SelectAll");

        DrawStuff(select);
        DrawStuff(add);
        DrawStuff(addAndRemove);
        DrawStuff(assign);
        DrawStuff(assignAndRemove);
        serializedObject.ApplyModifiedProperties();
    }

    private void DrawStuff(SerializedProperty a) {
        var rect = GUILayoutUtility.GetRect(0, 20, GUIStyle.none);
        EditorGUI.LabelField(rect, a.displayName, GUIStyle.none);
        rect.xMin += 115;
        rect.xMax -= 85;
        EditorGUI.PropertyField(rect, a.FindPropertyRelative("Key"), GUIContent.none);
        rect.xMax += 85;
        rect.xMin = rect.xMax - 80;
        EditorGUI.PropertyField(new Rect(rect.x, rect.y, 20, 20), a.FindPropertyRelative("Alt"), GUIContent.none);
        rect.x += 30;
        EditorGUI.PropertyField(new Rect(rect.x, rect.y, 20, 20), a.FindPropertyRelative("Ctrl"), GUIContent.none);
        rect.x += 30;
        EditorGUI.PropertyField(new Rect(rect.x, rect.y, 20, 20), a.FindPropertyRelative("Shift"), GUIContent.none);
        rect.yMin += 20;
    }
}