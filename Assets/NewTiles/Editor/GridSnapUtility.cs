using NewTiles;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class GridSnapUtility
{
    static GridSnapUtility()
    {
        ObjectChangeEvents.changesPublished += ChangesPublished;

    }
    private static void ChangesPublished(ref ObjectChangeEventStream stream)
    {
        for (int i = 0; i < stream.length; i++)
        {
            if (stream.GetEventType(i) == ObjectChangeKind.CreateGameObjectHierarchy)
            {
                var grid = Object.FindAnyObjectByType<TileGrid>();
                if (!grid) return;
                
                stream.GetCreateGameObjectHierarchyEvent(i, out var createGameObjectHierarchyEvent);
                var newGameObject = EditorUtility.InstanceIDToObject(createGameObjectHierarchyEvent.instanceId) as GameObject;
                if (!newGameObject || !newGameObject.GetComponent<GridElement>())
                {
                    return;
                }
                newGameObject.transform.SetParent(grid.transform);

                if (!Object.FindAnyObjectByType<GridElement>())
                {
                    newGameObject.transform.position = grid.transform.position;
                    return;
                }
                
                var pos = newGameObject.transform.position;
                
                newGameObject.transform.position = new Vector3(
                    Mathf.Round(pos.x),
                    pos.y - Mathf.Floor(pos.y) > 0.5f ? Mathf.Floor(pos.y) + 0.5f : Mathf.Round(pos.y),
                    Mathf.Round(pos.z));
            }
        }
    }
}
