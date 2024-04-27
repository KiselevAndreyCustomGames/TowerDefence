using CodeMonkey;
using CodeMonkey.Utils;
using System;
using UnityEngine;

public class TestPathfinding
{
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public Grid<PathNode> Grid { get; private set; }

    private readonly TestPathfindGridVisual _visual;
    private readonly TernaryTreeDB<PathNode> _tree = new();

    public TestPathfinding(int width, int height, TestPathfindGridVisual visual)
    {
        Grid = new Grid<PathNode>(width, height, 10f, Vector3.zero, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
        //for (int x = 0; x < width; x++)
        //{
        //    for (int y = 0; y < height; y++)
        //    {
        //        GetNode(x, y).Neibours = GetNeighbourList(GetNode(x, y));
        //    }
        //}

        _visual = visual;
        _visual.Setup(Grid);
    }
    
    public PathNode GetNode(int x, int y)
    {
        return Grid.GetGridObject(x, y);
    }

    public void SetDestination()
    {
        _tree.Clear();

        Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
        var destinationNode = Grid.GetGridObject(mouseWorldPosition);
        for(int x = 0; x < Grid.GetWidth(); x++)
        {
            for(int y = 0; y < Grid.GetHeight(); y++)
            {
                var node = GetNode(x, y);
                node.fCost = CalculateDistanceCost(node, destinationNode);
            }
        }
        _visual.UpdateNodes(Grid, destinationNode);
    }

    public void ChangeTree()
    {
        Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
        var node = Grid.GetGridObject(mouseWorldPosition);
        if(node != null)
        {
            if (_tree.Contains(node))
            {
                _tree.Remove(node);
                Debug.Log($"<color=red>- {node}</color>\nTreeCount: {_tree.Count()}\n{_tree}");
                _visual.DisableNode(node);
            }
            else
            {
                _tree.Add(node);
                Debug.Log($"<color=green>+ {node}</color>\nTreeCount: {_tree.Count()}\n{_tree}");
                _visual.EnableNode(node);
            }
        }
    }

    private int CalculateDistanceCost(PathNode a, PathNode b)
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    //private List<PathNode> GetNeighbourList(PathNode currentNode)
    //{
    //    List<PathNode> neighbourList = new();

    //    if (currentNode.x - 1 >= 0)
    //    {
    //        // Left
    //        neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
    //        // Left Down
    //        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
    //        // Left Up
    //        if (currentNode.y + 1 < Grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
    //    }
    //    if (currentNode.x + 1 < Grid.GetWidth())
    //    {
    //        // Right
    //        neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
    //        // Right Down
    //        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
    //        // Right Up
    //        if (currentNode.y + 1 < Grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
    //    }
    //    // Down
    //    if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
    //    // Up
    //    if (currentNode.y + 1 < Grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

    //    return neighbourList;
    //}
}
