using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(Mover))]
public class MoverEditor : Editor
{
    public override void OnInspectorGUI()
    {
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
    
    private void OnSceneGUI()
    {
        var mover = target as Mover;
        for (int i = 0; i < mover.path.Count; i++)
        {
            mover.path[i] = Handles.PositionHandle(mover.path[i], Quaternion.identity);
        }
    }
}
