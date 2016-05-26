using UnityEngine;
using System.Collections;
using UnityEditor;

//[CustomEditor(typeof(UnitComponent), true)]
//public class UnitComponentInspector : Editor {

//    private UnitComponent _component;

//    public void OnEnable()
//    {
//        _component = target as UnitComponent;
//    }

//    public override void OnInspectorGUI() {
//        serializedObject.Update();

//        base.DrawDefaultInspector();
//        _component.Priority = EditorGUILayout.IntField("Priority", _component.Priority);
//        //_component.IsSelectable = EditorGUILayout.Toggle("Selectable", _component.IsSelectable);

//        //EditorGUILayout.PropertyField(serializedObject.FindProperty("Priority"));

//        serializedObject.ApplyModifiedProperties();
//    }
//}
