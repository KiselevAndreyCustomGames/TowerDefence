using CodeBase;
using UnityEngine;

public class GridXZDebugger<TGridObject> : AGridDebugger<TGridObject>
{
    protected override Vector3 WorldTextRotation { get; set; } = new Vector3(90, 0, 0);

    public GridXZDebugger(AGrid<TGridObject> grid) : base(grid)
    {
    }
}