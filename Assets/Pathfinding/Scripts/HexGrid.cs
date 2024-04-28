using CodeBase;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HexGrid<TGridObject> : IGrid<TGridObject>
{
    private const float HEX_VERTICAL_OFFSET_MULTIPLIER = 0.75f;
    private const float HEX_HORIZONTAL_OFFSET_MULTIPLIER = 0.5f;

    private readonly int _width;
    private readonly int _height;
    private readonly float _cellSize;
    private readonly Vector3 _originPosition;
    private readonly TGridObject[,] _gridArray;

    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

    public int Width => _width;
    public int Height => _height;
    public float CellSize => _cellSize;

    public HexGrid(int width, int height, float cellSize, Vector3 originPosition, Func<HexGrid<TGridObject>, int, int, TGridObject> createGridObject) {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _originPosition = originPosition;

        _gridArray = new TGridObject[width, height];

        for (int x = 0; x < _gridArray.GetLength(0); x++) {
            for (int y = 0; y < _gridArray.GetLength(1); y++) {
                _gridArray[x, y] = createGridObject(this, x, y);
            }
        }
    }

    public Vector3 GetWorldPosition(int x, int y) {
        return new Vector3(x, y * HEX_VERTICAL_OFFSET_MULTIPLIER) * _cellSize + 
            ((y & 1) == 1 ? _cellSize * HEX_HORIZONTAL_OFFSET_MULTIPLIER * Vector3.right : Vector3.zero) +
            _originPosition
            + _cellSize * 0.5f * new Vector3(1, 1);
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y) {
        x = Mathf.FloorToInt((worldPosition - _originPosition).x / _cellSize);
        y = Mathf.RoundToInt((worldPosition - _originPosition).y / (_cellSize * HEX_VERTICAL_OFFSET_MULTIPLIER));
        //Debug.Log($"worldPosition: {worldPosition}\n" +
        //    $"_originPosition: {_originPosition}\n" +
        //    $"_cellSize: {_cellSize}\n" +
        //    $"_cellSize * HEX_VERTICAL_OFFSET_MULTIPLIER: {_cellSize * HEX_VERTICAL_OFFSET_MULTIPLIER}\n" +
        //    $"x: {x}, y: {y}");
        x = Mathf.Clamp(x, 0, _width - 1);
        y = Mathf.Clamp(y, 0, _height - 1);

        var closestIndex = new Vector3Int(x, y);
        var isEvenRow = (y & 1) == 0;

        var nearIndexes = new List<Vector3Int>();
        // Left
        if (x > 0) nearIndexes.Add(new Vector3Int(x - 1, y));
        //Rigth
        if (x + 1 < Width) nearIndexes.Add(new Vector3Int(x + 1, y));
        // Down
        if (y > 0)
        {
            if (isEvenRow && x > 0) nearIndexes.Add(new Vector3Int(x - 1, y - 1)); // Left if even row
            else if (isEvenRow == false && x + 1 < Width) nearIndexes.Add(new Vector3Int(x + 1, y - 1));   // Right if odd row
            nearIndexes.Add(new Vector3Int(x, y - 1)); // Left for odd row, right for even row
        }
        // Up
        if (y + 1 < Height)
        {
            if (isEvenRow && x > 0) nearIndexes.Add(new Vector3Int(x - 1, y + 1)); // Left if even row
            else if (isEvenRow == false && x + 1 < Width) nearIndexes.Add(new Vector3Int(x + 1, y + 1));   // Right if odd row
            nearIndexes.Add(new Vector3Int(x, y + 1)); // Left for odd row, right for even row
        }

        foreach (var index in nearIndexes)
        {
            if(Vector3.Distance(worldPosition, GetWorldPosition(index.x, index.y)) <
                Vector3.Distance(worldPosition, GetWorldPosition(closestIndex.x, closestIndex.y)))
                closestIndex = index;
        }

        x = closestIndex.x;
        y = closestIndex.y;
    }

    public void SetGridObject(int x, int y, TGridObject value) {
        if (x >= 0 && y >= 0 && x < _width && y < _height) {
            _gridArray[x, y] = value;
            OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }

    public void TriggerGridObjectChanged(int x, int y) {
        OnGridObjectChanged?.Invoke(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value) {
        GetXY(worldPosition, out int x, out int y);
        SetGridObject(x, y, value);
    }

    public TGridObject GetGridObject(int x, int y) {
        if (x >= 0 && y >= 0 && x < _width && y < _height) {
            return _gridArray[x, y];
        } else {
            return default;
        }
    }

    public TGridObject GetGridObject(Vector3 worldPosition) {
        GetXY(worldPosition, out int x, out int y);
        return GetGridObject(x, y);
    }
}
