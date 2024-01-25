using CodeBase.Utility;
using UnityEngine;

namespace CodeBase
{
    public class GridXZ: AGrid
    {
        private readonly float _yPosition;

        public GridXZ(int width, int height, float cellSize, Vector3 originPosition, float yPosition = 0) 
            : base (width, height, cellSize, originPosition)
        {
            _yPosition = yPosition;
        }

        protected override Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, 0, y) * cellSize + new Vector3(0, _yPosition, 0) + originPosition;
        }

        protected override void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            worldPosition -= originPosition;
            x = Mathf.FloorToInt(worldPosition.x / cellSize);
            y = Mathf.FloorToInt(worldPosition.z / cellSize);
        }

        protected override void ShowGrid()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    debugTextArray[x, y] = UtilsClass.CreateWorldText($"{x}, {y}", null, GetWorldPosition(x, y) + new Vector3(cellSize, 0, cellSize) * 0.5f, new Vector3(90, 0, 0), (int)(cellSize * 2f));
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);
        }
    }
}