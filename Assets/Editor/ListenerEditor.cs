using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Listener))]
public class ListenerEditor : Editor
{
    private Listener _listener;
    private SerializedProperty _onAnySoundProperty;
    private SerializedProperty _reactionsProperty;
    
    private void OnEnable()
    {
        _listener = target as Listener;
        _onAnySoundProperty = serializedObject.FindProperty("onAnySound");
        _reactionsProperty = serializedObject.FindProperty("reactions");
    }

    public override void OnInspectorGUI()
    {
        if (!_listener) return;
        DrawDefaultInspector();
        if (_listener.isTransmitter)
        {
            EditorGUILayout.PropertyField(_onAnySoundProperty, new GUIContent("On Transmit Any Sound"));
        }
        else
        {
            EditorGUILayout.PropertyField(_reactionsProperty, new GUIContent("Reactions"));
        }
        
        serializedObject.ApplyModifiedProperties();
    }
}
