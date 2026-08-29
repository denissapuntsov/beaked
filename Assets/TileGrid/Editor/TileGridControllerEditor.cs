using System;
using LevelStructure;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TileGridController))]
public class TileGridControllerEditor : Editor
{
    TileGridController _controller;
    private void OnEnable()
    {
        _controller = target as TileGridController;
    }

    private void OnSceneGUI()
    {
        if (!_controller) return;
        
        foreach (var cell in _controller.CellToElementMap.Keys)
        {
            Handles.color = 
                cell.IsAccessible 
                ? Color.yellow
                : Color.red;
            Handles.DrawWireCube(cell.GridPosition - new Vector3(0, 0.25f, 0), new Vector3(0.8f, 0.3f, 0.8f));
        }
    }

    public override void OnInspectorGUI()
    {
        if (!_controller) return;
        DrawDefaultInspector();
        if (GUILayout.Button("Setup"))
        {
            _controller.Setup();
        }
    }
}
