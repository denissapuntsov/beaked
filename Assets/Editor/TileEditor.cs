using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[CustomEditor(typeof(Tile))]
[CanEditMultipleObjects]
public class TileEditor: Editor
{
    private SerializedProperty data, blocksTileUnderneath;
    
    private void OnEnable()
    {
        data = serializedObject.FindProperty("data");
        blocksTileUnderneath = serializedObject.FindProperty("blocksTileUnderneath");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(blocksTileUnderneath, new GUIContent("Blocks Tile Underneath"));
        EditorGUILayout.PropertyField(data, new GUIContent("Data"));

        var tiles = Selection.gameObjects.Select(go => go.GetComponent<Tile>()).ToArray();
        var so = new SerializedObject(tiles);
        so.FindProperty("data").objectReferenceValue = data.objectReferenceValue;
        so.FindProperty("blocksTileUnderneath").boolValue = blocksTileUnderneath.boolValue;

        if (data.hasMultipleDifferentValues)
        {
            return;
        }
        
        if (so.hasModifiedProperties)
        {
            so.ApplyModifiedProperties();
            foreach (var tile in tiles)
            {
                if (data.objectReferenceValue == null)
                {
                    tile.name = "Tile (Empty)";
                    DestroyImmediate(tile.tileNormal);
                    return;
                }
                
                tile.name = $"Tile ({tile.data.name})";
                if (tile.tileNormal)
                {
                    DestroyImmediate(tile.tileNormal);
                }
                
                if (!tile.data) return;
                tile.tileNormal = Instantiate(tile.data.prefabNormal, tile.transform);
            }
        }
    }
}
