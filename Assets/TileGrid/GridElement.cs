using UnityEngine;

namespace LevelStructure
{
    /// <summary>
    /// Foundational Component for communicating with the Beaked! tile grid.
    /// </summary>
    public class GridElement : MonoBehaviour
    {
        [field: SerializeField] public Vector3 GridSize { get; private set; } = Vector3.one;
        [field: SerializeField] public bool IsSolid { get; private set; } = true;
    }
}
