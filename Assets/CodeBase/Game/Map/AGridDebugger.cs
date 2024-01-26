using CodeBase;
using CodeBase.Utility;
using UnityEngine;

public abstract class AGridDebugger<TGridObject>
{
    protected readonly AGrid<TGridObject> grid;
    protected readonly TextMesh[,] debugTextArray;

    protected virtual Vector3 WorldTextRotation { get; set; } = default;

    public AGridDebugger(AGrid<TGridObject> grid)
    {
        this.grid = grid;

        debugTextArray = new TextMesh[grid.Width, grid.Height];
        grid.GridObjectChanged += OnGridObjectChanged;

        ShowGrid();
    }

    ~AGridDebugger()
    {
        grid.GridObjectChanged -= OnGridObjectChanged;
    }

    protected virtual void ShowGrid()
    {
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                debugTextArray[x, y] = UtilsClass.CreateWorldText($"{grid.GetGridObject(x, y)}", null, grid.GetWorldPosition(x, y) + new Vector3(grid.CellSize, grid.CellSize) * 0.5f, WorldTextRotation, (int)(grid.CellSize * 2f));
                Debug.Log($"{grid.GetGridObject(x, y)}, {grid.GetWorldPosition(x, y) + new Vector3(grid.CellSize, grid.CellSize) * 0.5f}");
                Debug.DrawLine(grid.GetWorldPosition(x, y), grid.GetWorldPosition(x, y + 1), Color.white, 100f);
                Debug.DrawLine(grid.GetWorldPosition(x, y), grid.GetWorldPosition(x + 1, y), Color.white, 100f);
            }
        }

        Debug.DrawLine(grid.GetWorldPosition(0, grid.Height), grid.GetWorldPosition(grid.Width, grid.Height), Color.white, 100f);
        Debug.DrawLine(grid.GetWorldPosition(grid.Width, 0), grid.GetWorldPosition(grid.Width, grid.Height), Color.white, 100f);
    }

    private void OnGridObjectChanged(int x, int y, TGridObject obj)
    {
        debugTextArray[x, y].text = obj.ToString();
    }
}
