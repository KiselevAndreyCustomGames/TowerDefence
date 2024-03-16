using System;
using UnityEngine;

namespace CodeBase
{
    public class GridXZ<TGridObject> : AGrid<TGridObject>
    {
        public float YPosition { get; private set; }

        public GridXZ(int width, int height, float cellSize, Vector3 originPosition, Func<int, int, TGridObject> createGridObject, float yPosition = 0) 
            : base (width, height, cellSize, originPosition, createGridObject)
        {
            YPosition = yPosition;
        }

        public override Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, 0, y) * CellSize + new Vector3(0, YPosition, 0) + OriginPosition;
        }

        protected override void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            worldPosition -= OriginPosition;
            x = Mathf.FloorToInt(worldPosition.x / CellSize);
            y = Mathf.FloorToInt(worldPosition.z / CellSize);
        }
    }
}