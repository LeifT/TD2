using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof (ControlGroups))]
// ReSharper disable once CheckNamespace
public class ControlGroupsEditor : Editor {
    private ReorderableList _unitList;

    // ReSharper disable once UnusedMember.Local
    private void OnEnable() {
        _unitList = new ReorderableList(serializedObject, serializedObject.FindProperty("Groups"), true, true, true,
            true);

        _unitList.drawElementCallback = (rect, index, isActive, isFocused) => {
            var element = _unitList.serializedProperty.GetArrayElementAtIndex(index);
            var elementRect = rect;
            elementRect.yMin += 1;
            elementRect.yMax -= 1;
            elementRect.x += 5;

            EditorGUI.LabelField(elementRect, new GUIContent(string.Concat("Group ", index + 1)));

            elementRect.xMin += 60;
            elementRect.xMax -= 15;
            EditorGUI.PropertyField(elementRect, element, GUIContent.none);
        };

        _unitList.drawHeaderCallback = rect => { EditorGUI.LabelField(rect, "Groups"); };

        _unitList.onSelectCallback = l => {
            var prefab =
                l.serializedProperty.GetArrayElementAtIndex(l.index).FindPropertyRelative("Prefab").objectReferenceValue
                    as GameObject;
            if (prefab) {
                EditorGUIUtility.PingObject(prefab.gameObject);
            }
        };

        _unitList.onCanRemoveCallback = l => { return l.count > 1; };
        _unitList.onRemoveCallback = l => {
            if (EditorUtility.DisplayDialog("Warning!", "Are you sure you want to delete the wave?", "Yes", "No")) {
                ReorderableList.defaultBehaviours.DoRemoveButton(l);
            }
        };
        _unitList.onAddCallback = l => {
            l.serializedProperty.arraySize++;
        };
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();
        var rect = GUILayoutUtility.GetRect(0, 20, GUIStyle.none);
        rect.xMin = rect.xMax - 85;
        EditorGUI.LabelField(rect, "Alt  Ctrl  Shift");

        var select = serializedObject.FindProperty("Select");
        var add = serializedObject.FindProperty("Add");
        var addAndRemove = serializedObject.FindProperty("AddAndRemove");
        var assign = serializedObject.FindProperty("Assign");
        var assignAndRemove = serializedObject.FindProperty("AssignAndRemove");

        DrawStuff(select);
        DrawStuff(add);
        DrawStuff(addAndRemove);
        DrawStuff(assign);
        DrawStuff(assignAndRemove);


        if (_unitList != null) {
            _unitList.DoLayoutList();
        }
        else {
            Debug.Log("UnitList is null");
        }
        serializedObject.ApplyModifiedProperties();
    }

    //private void clickHandler(object target) {
    //    var data = (WaveCreationParams) target;
    //    var index = unitList.serializedProperty.arraySize;
    //    unitList.serializedProperty.arraySize++;
    //    unitList.index = index;
    //    var element = unitList.serializedProperty.GetArrayElementAtIndex(index);
    //    element.FindPropertyRelative("Type").enumValueIndex = (int) data.Type;
    //    element.FindPropertyRelative("Count").intValue = data.Type == MobWave.WaveType.Boss ? 1 : 20;
    //    element.FindPropertyRelative("Prefab").objectReferenceValue =
    //        AssetDatabase.LoadAssetAtPath(data.Path, typeof (GameObject)) as GameObject;
    //    serializedObject.ApplyModifiedProperties();
    //}

    //private struct WaveCreationParams {
    //    public string Path;
    //    public MobWave.WaveType Type;
    //}

    private void DrawStuff(SerializedProperty a) {
        var rect = GUILayoutUtility.GetRect(0, 20, GUIStyle.none);

        EditorGUI.PropertyField(new Rect(rect.x, rect.y, 20, 20), a.FindPropertyRelative("Enable"), GUIContent.none);
        var isDisabled = a.FindPropertyRelative("Enable").boolValue;

        EditorGUI.BeginDisabledGroup(!isDisabled);
        rect.xMin += 20;
        EditorGUI.LabelField(rect, a.displayName, GUIStyle.none);
        rect.xMin = rect.xMax - 85;

        EditorGUI.PropertyField(new Rect(rect.x, rect.y, 20, 20), a.FindPropertyRelative("Alt"), GUIContent.none);
        rect.x += 30;
        EditorGUI.PropertyField(new Rect(rect.x, rect.y, 20, 20), a.FindPropertyRelative("Ctrl"), GUIContent.none);
        rect.x += 30;
        EditorGUI.PropertyField(new Rect(rect.x, rect.y, 20, 20), a.FindPropertyRelative("Shift"), GUIContent.none);

        EditorGUI.EndDisabledGroup();

        rect.yMin += 20;
    }
}