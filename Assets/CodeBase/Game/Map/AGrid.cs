using CodeBase.Utility;
using UnityEngine;

namespace CodeBase
{
    public abstract class AGrid
    {
        protected readonly int width;
        protected readonly int height;
        protected readonly float cellSize;

        protected readonly Vector3 originPosition;

        protected readonly int[,] gridArray;
        protected readonly TextMesh[,] debugTextArray;

        public AGrid(int width, int height, float cellSize, Vector3 originPosition)
        {
            this.width = width;
            this.height = height;
            this.cellSize = cellSize;
            this.originPosition = originPosition;

            gridArray = new int[width, height];
            debugTextArray = new TextMesh[width, height];

            ShowGrid();
        }

        public void SetValue(int x, int y, int value)
        {
            if(InGrid(x, y))
            {
                gridArray[x, y] = value;
                debugTextArray[x, y].text = value.ToString();
            }
        }

        public void SetValue(Vector3 worldPosition, int value)
        {
            GetXY(worldPosition, out int x, out int y);
            SetValue(x, y, value);
        }

        public int GetValue(int x, int y)
        {
            if(InGrid(x, y))
            {
                return gridArray[x, y];
            }
            return default;
        }

        public int GetValue(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            return GetValue(x, y);
        }

        protected virtual Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * cellSize + originPosition;
        }

        protected virtual void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            worldPosition -= originPosition;
            x = Mathf.FloorToInt(worldPosition.x / cellSize);
            y = Mathf.FloorToInt(worldPosition.y / cellSize);
        }

        protected virtual void ShowGrid()
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    debugTextArray[x,y] = UtilsClass.CreateWorldText($"{x}, {y}", null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * 0.5f, default, (int)(cellSize * 2f));
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }

            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 1f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 1f);
        }

        protected bool OutOfGrid(int x, int y)
        {
            return x < 0 || y < 0 || x >= width || y >= height;
        }

        protected bool InGrid(int x, int y)
        {
            return x >= 0 && y >= 0 && x < width && y < height;
        }
    }
}