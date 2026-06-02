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
    private SerializedProperty data;
    
    private void OnEnable()
    {
        data = serializedObject.FindProperty("data");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(data, new GUIContent("Data"));

        var tiles = Selection.gameObjects.Select(go => go.GetComponent<Tile>()).ToArray();
        var so = new SerializedObject(tiles);
        so.FindProperty("data").objectReferenceValue = data.objectReferenceValue;

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
                    DestroyImmediate(tile.tileBlocked);
                    return;
                }
                
                tile.name = $"Tile ({tile.data.name})";
                if (tile.tileNormal)
                {
                    DestroyImmediate(tile.tileNormal);
                }

                if (tile.tileBlocked)
                {
                    DestroyImmediate(tile.tileBlocked);
                }

                if (!tile.data) return;
                tile.tileNormal = Instantiate(tile.data.prefabNormal, tile.transform);
                tile.tileBlocked = Instantiate(tile.data.prefabBlocked, tile.transform);
                
                tile.tileBlocked.gameObject.SetActive(false);
            }
        }
    }
}
