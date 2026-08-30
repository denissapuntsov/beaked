using LevelStructure;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridElement))]
public class GridElementEditor : Editor
{
    private GridElement _element;
    
    private void OnEnable()
    {
        _element = target as GridElement;
    }
    
    private void OnSceneGUI()
    {
        if (!_element) return;
        
        foreach (var cell in _element.cells)
        {
            Handles.color = 
                cell.IsAccessible 
                    ? Color.yellow
                    : Color.red;
            Handles.DrawWireCube(cell.GridPosition - new Vector3(0, 0.25f, 0), new Vector3(0.8f, 0.3f, 0.8f));
        }
    }
}
