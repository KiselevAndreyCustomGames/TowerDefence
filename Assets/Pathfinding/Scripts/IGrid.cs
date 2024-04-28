using System;
using UnityEngine;

public interface IGrid<TGridObject>
{
    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged;

    public int Width { get; }
    public int Height { get; }
    public float CellSize { get; }

    public void TriggerGridObjectChanged(int x, int y);
    public TGridObject GetGridObject(int x, int y);
    public Vector3 GetWorldPosition(int x, int y);
    public void GetXY(Vector3 worldPosition, out int x, out int y);
}

public class OnGridObjectChangedEventArgs : EventArgs
{
    public int x;
    public int y;
}
