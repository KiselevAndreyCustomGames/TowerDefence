﻿/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using CodeMonkey;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding {

    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static Pathfinding Instance { get; private set; }

    private Grid<PathNode> grid;
    private IPathFindDataBase<PathNode> openNodes;
    private List<PathNode> closedNodes;

    public Pathfinding(int width, int height, IPathFindDataBase<PathNode> changable) {
        Instance = this;
        grid = new Grid<PathNode>(width, height, 10f, Vector3.zero, (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GetNode(x, y).Neibours = GetNeighbourList(GetNode(x, y));
            }
        }

        openNodes = changable;
    }

    public Grid<PathNode> GetGrid() {
        return grid;
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition) {
        grid.GetXY(startWorldPosition, out int startX, out int startY);
        grid.GetXY(endWorldPosition, out int endX, out int endY);

        List<PathNode> path = FindPath(startX, startY, endX, endY);
        if (path == null) {
            return null;
        } else {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (PathNode pathNode in path) {
                vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * grid.CellSize + Vector3.one * grid.CellSize * .5f);
            }
            return vectorPath;
        }
    }

    public List<PathNode> FindPath(int startX, int startY, int endX, int endY) {
        var maxCountIteration = grid.Width * grid.Height;
        var iteration = 0;
        PathNode startNode = grid.GetGridObject(startX, startY);
        PathNode endNode = grid.GetGridObject(endX, endY);

        if (startNode == null || endNode == null) {
            // Invalid Path
            return null;
        }

        if (endNode.isWalkable == false)
            endNode = FindNearestWalkableNode(endNode);

        openNodes.Clear();
        openNodes.Add(startNode);
        closedNodes = new List<PathNode>();
        Debug.Log($"+ {startNode}\n{openNodes}");

        for (int x = 0; x < grid.Width; x++) {
            for (int y = 0; y < grid.Height; y++) {
                PathNode pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = 99999999;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();
        
        PathfindingDebugStepVisual.Instance.ClearSnapshots();
        PathfindingDebugStepVisual.Instance.TakeSnapshot(startNode, openNodes, closedNodes, true);

        PathNode prevNote = null;
        while (openNodes.IsEmpty() == false) {
            PathNode lowestNode = openNodes.GetLowest();
            if(iteration > maxCountIteration)
            {
                Debug.Log($"<color=red>Iteration Error</color>\n" +
                    $"lowestNode: {lowestNode}\n" +
                    $"prevNote: {prevNote}");
                break;
            }
            if(lowestNode.Equals(prevNote))
            {
                Debug.Log($"<color=red>lowestNode is previous. Snapshots: {PathfindingDebugStepVisual.Instance.GetSnapshotsCount()}</color>\n" +
                    $"lowestNode: {lowestNode}");
                break;
            }
            if (lowestNode == endNode) {
                // Reached final node
                PathfindingDebugStepVisual.Instance.TakeSnapshot(lowestNode, openNodes, closedNodes, true);
                var path = CalculatePath(endNode);
                PathfindingDebugStepVisual.Instance.TakeSnapshotFinalPath(grid, path);
                Debug.Log($"path: {path.Count}\nsnapshots: {PathfindingDebugStepVisual.Instance.GetSnapshotsCount()}");
                return path;
            }

            try
            {
                openNodes.Remove(lowestNode);
            }
            finally
            {
                Debug.Log($"- {lowestNode}\n{openNodes}");
            }
            closedNodes.Add(lowestNode);

            PathfindingDebugStepVisual.Instance.TakeSnapshot(lowestNode, openNodes, closedNodes, true);
            foreach (PathNode neighbourNode in lowestNode.Neibours) {
                if (closedNodes.Contains(neighbourNode)) continue;
                if (neighbourNode.isWalkable == false) {
                    closedNodes.Add(neighbourNode);
                    continue;
                }

                int tentativeGCost = lowestNode.gCost + CalculateDistanceCost(lowestNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    if (openNodes.Contains(neighbourNode))
                    {
                        try
                        {
                            openNodes.Remove(neighbourNode);
                        }
                        finally
                        {
                            Debug.Log($"<color=red>- {neighbourNode}</color>\n{openNodes}");
                        }
                    }
                    neighbourNode.cameFromNode = lowestNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();
                    openNodes.Add(neighbourNode);
                    Debug.Log($"+ {neighbourNode}\n{openNodes}");
                }
                PathfindingDebugStepVisual.Instance.TakeSnapshot(neighbourNode, openNodes, closedNodes);
            }

            iteration++;
            prevNote = lowestNode;
        }

        // Out of nodes on the openList
        return null;
    }

    private PathNode FindNearestWalkableNode(PathNode node)
    {
        PathNode resultNode = null;
        // calculate neighbours cost to start node

        return node;
    }

    private List<PathNode> GetNeighbourList(PathNode currentNode) {
        List<PathNode> neighbourList = new List<PathNode>();

        if (currentNode.x - 1 >= 0) {
            // Left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            // Left Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            // Left Up
            if (currentNode.y + 1 < grid.Height) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
        }
        if (currentNode.x + 1 < grid.Width) {
            // Right
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            // Right Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            // Right Up
            if (currentNode.y + 1 < grid.Height) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }
        // Down
        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        // Up
        if (currentNode.y + 1 < grid.Height) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighbourList;
    }

    public PathNode GetNode(int x, int y) {
        return grid.GetGridObject(x, y);
    }

    private List<PathNode> CalculatePath(PathNode endNode) {
        List<PathNode> path = new()
        {
            endNode
        };
        PathNode currentNode = endNode;
        while (currentNode.cameFromNode != null) {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode a, PathNode b) {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }
}