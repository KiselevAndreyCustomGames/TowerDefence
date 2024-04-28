using CodeMonkey;
using System.Collections.Generic;
using UnityEngine;

public class HexPathfinding
{
    private HexGrid<PathNode> _grid;
    private IPathFindDataBase<PathNode> openNodes;
    private List<PathNode> closedNodes;

    public HexPathfinding(int width, int height, IPathFindDataBase<PathNode> changable)
    {
        _grid = new HexGrid<PathNode>(width, height, 10f, Vector3.zero, (HexGrid<PathNode> g, int x, int y) => new PathNode(g, x, y));
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GetNode(x, y).Neibours = GetNeighbours(x, y);
            }
        }

        openNodes = changable;
    }

    public IGrid<PathNode> GetGrid()
    {
        return _grid;
    }

    public PathNode GetNode(int x, int y)
    {
        return _grid.GetGridObject(x, y);
    }

    public List<PathNode> GetNeighbours(int x, int y)
    {
        List<PathNode> neighbourList = new();

        var isEvenRow = (y & 1) == 0;
        // Left
        if (x > 0) neighbourList.Add(_grid.GetGridObject(x - 1, y));
        //Rigth
        if (x + 1 < _grid.Width) neighbourList.Add(_grid.GetGridObject(x + 1, y));
        // Down
        if (y > 0)
        {
            //// Left Down
            //if (isEvenRow && x > 0) neighbourList.Add(GetGridObject(x - 1, y - 1));
            //else if (isEvenRow == false) neighbourList.Add(GetGridObject(x, y - 1));
            //// Rigth Down
            //if (isEvenRow) neighbourList.Add(GetGridObject(x, y - 1));
            //else if (isEvenRow == false && x + 1 < Width) neighbourList.Add(GetGridObject(x + 1, y - 1));

            if (isEvenRow && x > 0) neighbourList.Add(_grid.GetGridObject(x - 1, y - 1)); // Left if even row
            else if (isEvenRow == false && x + 1 < _grid.Width) neighbourList.Add(_grid.GetGridObject(x + 1, y - 1));   // Right if odd row
            neighbourList.Add(_grid.GetGridObject(x, y - 1)); // Left for odd row, right for even row
        }
        // Up
        if (y + 1 < _grid.Height)
        {
            //// Left Up
            //if (isEvenRow && x > 0) neighbourList.Add(GetGridObject(x - 1, y + 1));
            //else if (isEvenRow == false) neighbourList.Add(GetGridObject(x, y + 1));
            //// Rigth Up
            //if (isEvenRow) neighbourList.Add(GetGridObject(x, y + 1));
            //else if (isEvenRow == false && x + 1 < Width) neighbourList.Add(GetGridObject(x + 1, y + 1));

            if (isEvenRow && x > 0) neighbourList.Add(_grid.GetGridObject(x - 1, y + 1)); // Left if even row
            else if (isEvenRow == false && x + 1 < _grid.Width) neighbourList.Add(_grid.GetGridObject(x + 1, y + 1));   // Right if odd row
            neighbourList.Add(_grid.GetGridObject(x, y + 1)); // Left for odd row, right for even row
        }

        return neighbourList;
    }
}
