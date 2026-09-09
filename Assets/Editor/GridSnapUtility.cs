using LevelStructure;
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
                stream.GetCreateGameObjectHierarchyEvent(i, out var createGameObjectHierarchyEvent);
                var newGameObject = EditorUtility.EntityIdToObject(createGameObjectHierarchyEvent.entityId) as GameObject;
                if (!newGameObject || !newGameObject.GetComponent<GridElement>())
                {
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