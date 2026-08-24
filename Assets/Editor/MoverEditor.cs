/*
using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Mover))]
public class MoverEditor : Editor
{
    private void OnSceneGUI()
    {
        var mover = target as Mover;
        if (!mover ||
            !mover.enabled ||
            mover.path.Count == 0)
        {
            return;
        }

        if (Event.current.type != EventType.Repaint) return;
        
        var path = mover.path;
        for (int i = 0; i < path.Count; i++)
        {
            Handles.color = Handles.xAxisColor;
            Handles.ArrowHandleCap(
                i,
                path[i] - new Vector3(0f, 0.5f, 0f),
                Quaternion.LookRotation(Vector3.right),
                0.5f,
                EventType.Repaint
            );
            Handles.color = Handles.yAxisColor;
            Handles.ArrowHandleCap(
                i,
                path[i] - new Vector3(0f, 0.5f, 0f),
                Quaternion.LookRotation(Vector3.up),
                0.5f,
                EventType.Repaint
            );
            Handles.color = Handles.zAxisColor;
            Handles.ArrowHandleCap(
                i,
                path[i] - new Vector3(0f, 0.5f, 0f),
                Quaternion.LookRotation(Vector3.forward),
                0.5f,
                EventType.Repaint
            );
        }
    }
}
*/
