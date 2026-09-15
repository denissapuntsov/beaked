using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(Mover))]
public class MoverEditor : Editor
{
    private Mover _mover;
    private SerializedProperty _offsets, _path;
    
    private void OnEnable()
    {
        _mover = target as Mover;
        _offsets = serializedObject.FindProperty("offsets");
        _path = serializedObject.FindProperty("path");
    }

    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        var offsets = EditorGUILayout.PropertyField(_offsets, new GUIContent("Path"));
        
        if (EditorGUI.EndChangeCheck())
        {
            _path.ClearArray();
            for (int i = 0; i < _offsets.arraySize; i++)
            {
                Vector3 newEntry;
                Vector3 nextValue = _offsets.GetArrayElementAtIndex(i).vector3Value;
                if (i == 0)
                {
                    newEntry = _mover.transform.position + nextValue;
                }
                else
                {
                    newEntry = nextValue + _mover.path[i - 1];
                }
                _path.InsertArrayElementAtIndex(i);
                _path.GetArrayElementAtIndex(i).vector3Value = newEntry;
            }

            serializedObject.ApplyModifiedProperties();
        }
        
        DrawDefaultInspector();
        
        Mover mover = (Mover)target;
        if (mover.path.Count > 0)
        {
            if (GUILayout.Button("Move"))
            {
                mover.Move(null);
            }
        }
    }
    
    /*private void OnSceneGUI()
    {
        var mover = target as Mover;
        _offsets.GetArrayElementAtIndex(0).vector3Value =
            Handles.PositionHandle(mover.transform.position + mover.offsets[0], Quaternion.identity);
        /*for (int i = 0; i < _offsets.arraySize; i++)
        {
            if (i == 0)
            {
                
            }
            _offsets.GetArrayElementAtIndex(i).vector3Value = Handles.PositionHandle(mover.path[i], Quaternion.identity);
        }#1#
    }*/
}
