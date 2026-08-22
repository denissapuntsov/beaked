using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace LevelStructure
{
    public class TileGridView
    {
        public List<GridElement> GridElements;

        public void BounceGridElement(GridElement gridElement)
        {
            var startingPosition = gridElement.transform.position;
            gridElement.transform
                .DOPunchPosition(new Vector3(0, -0.2f, 0), 0.5f, 2, 2)
                .OnComplete(() =>
                {
                    gridElement.transform.position = startingPosition;
                });
        }
    }
}