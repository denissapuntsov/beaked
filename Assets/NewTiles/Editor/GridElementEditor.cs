using System;
using UnityEditor;
using UnityEngine;

namespace NewTiles
{
    [CustomEditor(typeof(GridElement))]
    public class GridElementEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            GridElement gridElement = (GridElement)target;
            
        }
    }
}
