﻿using System;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

namespace CodeBase
{
    public abstract class AGrid<TGridObject>
    {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public float CellSize { get; private set; }

        public Vector3 OriginPosition { get; private set; }

        protected readonly TGridObject[,] gridArray;

        public Action<int, int, TGridObject> GridObjectChanged;

        public AGrid(int width, int height, float cellSize, Vector3 originPosition, Func<TGridObject> createGridObject)
        {
            Width = width;
            Height = height;
            CellSize = cellSize;
            OriginPosition = originPosition;

            gridArray = new TGridObject[width, height];

            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    gridArray[x, y] = createGridObject();
                }
            }
        }

        public void SetGridObject(int x, int y, TGridObject gridObject)
        {
            if(InGrid(x, y))
            {
                gridArray[x, y] = gridObject;
                GridObjectChanged?.Invoke(x, y, gridObject);
            }
        }

        public void SetGridObject(Vector3 worldPosition, TGridObject gridObject)
        {
            GetXY(worldPosition, out int x, out int y);
            SetGridObject(x, y, gridObject);
        }

        public TGridObject GetGridObject(int x, int y)
        {
            if(InGrid(x, y))
            {
                return gridArray[x, y];
            }
            return default;
        }

        public TGridObject GetGridObject(Vector3 worldPosition)
        {
            GetXY(worldPosition, out int x, out int y);
            return GetGridObject(x, y);
        }

        public virtual Vector3 GetWorldPosition(int x, int y)
        {
            return new Vector3(x, y) * CellSize + OriginPosition;
        }

        protected virtual void GetXY(Vector3 worldPosition, out int x, out int y)
        {
            worldPosition -= OriginPosition;
            x = Mathf.FloorToInt(worldPosition.x / CellSize);
            y = Mathf.FloorToInt(worldPosition.y / CellSize);
        }

        protected bool OutOfGrid(int x, int y)
        {
            return x < 0 || y < 0 || x >= Width || y >= Height;
        }

        protected bool InGrid(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }
    }
}
