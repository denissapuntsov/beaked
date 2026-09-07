using LevelStructure;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GridElement))]
public class GridElementEditor : Editor
{
    private GridElement _element;
    private GridElement[] _children;
    
    private void OnEnable()
    {
        _element = target as GridElement;
        _children = _element?.GetComponentsInChildren<GridElement>();
    }
    
    private void OnSceneGUI()
    {
        if (!_element || _element.cells == null || _element.cells.Count == 0) return;
        
        foreach (var cell in _element.cells)
        {
            Handles.color = 
                cell.IsAccessible 
                    ? Color.yellow
                    : Color.red;
            Handles.DrawWireCube(cell.GridPosition - new Vector3(0, 0.25f, 0), new Vector3(0.8f, 0.3f, 0.8f));
        }
        
        if (_children.Length == 0) return;

        foreach (var child in _children)
        {
            foreach (var cell in child.cells)
            {
                if (!cell.IsSolid)
                {
                    Handles.color = Color.gray;
                }
                else
                {
                    Handles.color = 
                        cell.IsAccessible 
                            ? Color.yellow
                            : Color.red;
                }
                Handles.DrawWireCube(cell.GridPosition - new Vector3(0, 0.25f, 0), new Vector3(0.8f, 0.3f, 0.8f));
            }
        }
    }
    
    public override void OnInspectorGUI()
    {
        if (!_element) return;
        DrawDefaultInspector();
        if (GUILayout.Button("Rescan"))
        {
            var controller = FindAnyObjectByType<TileGridController>();
            if (!controller) return;
            controller.Setup();
        }
    }
}
