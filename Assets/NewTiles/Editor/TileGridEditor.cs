using NewTiles;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TileGrid))]
public class TileGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        TileGrid so = (TileGrid)target;
        EditorGUILayout.Space();
        var scanButton = GUILayout.Button("Scan");
        if (scanButton)
        {
            so.Scan();
        }
    }
}
