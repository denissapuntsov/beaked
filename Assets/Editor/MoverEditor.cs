using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Mover))]
public class MoverEditor : Editor
{
    private void OnSceneGUI()
    {
        var mover = target as Mover;
        for (int i = 0; i < mover.path.Count; i++)
        {
            mover.path[i] = Handles.PositionHandle(mover.path[i], Quaternion.identity);
        }
    }
}
