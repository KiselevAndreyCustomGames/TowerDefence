using CodeMonkey;
using CodeMonkey.Utils;
using Lean.Pool;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestPathfindGridVisual : MonoBehaviour
{
    [SerializeField] private PathfindingDebugStepVisualNode pfPathfindingDebugStepVisualNode;
    [SerializeField] private TextMeshPro _numberTextPrefab;
    [SerializeField] private Color _baseColor = UtilsClass.GetColorFromString("636363");
    [SerializeField] private Color _destinationColor = Color.cyan;
    [SerializeField] private Color _processedNodeColor = Color.yellow;

    private readonly List<PathfindingDebugStepVisualNode> visualNodeList = new();
    private PathfindingDebugStepVisualNode[,] visualNodeArray;

    public void Setup(Grid<PathNode> grid)
    {
        visualNodeArray = new PathfindingDebugStepVisualNode[grid.GetWidth(), grid.GetHeight()];
        visualNodeList.Clear();
        LeanPool.DespawnAll();

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                Vector3 gridPosition = grid.GetCellSize() * (new Vector3(x, y) + Vector3.one * 0.5f);
                var visualNode = LeanPool.Spawn(pfPathfindingDebugStepVisualNode, gridPosition, Quaternion.identity, transform);
                visualNodeArray[x, y] = visualNode;
                visualNodeList.Add(visualNode);
            }
        }

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            var numPosition = grid.GetCellSize() * (new Vector3(x, -1f) + Vector3.one * 0.5f);
            var number = LeanPool.Spawn(_numberTextPrefab, numPosition, Quaternion.identity, transform);
            number.text = x.ToString();
        }

        for(int y = 0; y < grid.GetHeight(); y++)
        {
            var numPosition = grid.GetCellSize() * (new Vector3(-1f, y) + Vector3.one * 0.5f);
            var number = LeanPool.Spawn(_numberTextPrefab, numPosition, Quaternion.identity, transform);
            number.text = y.ToString();
        }

        HideNodeVisuals();
    }

    public void UpdateNodes(Grid<PathNode> grid, PathNode destinationNode)
    {
        HideNodeVisuals();
        visualNodeArray[destinationNode.x, destinationNode.y].ChangeBgColor(_destinationColor);

        for (int x = 0; x < grid.GetWidth(); x++)
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                var nodeVisual = visualNodeArray[x, y];
                var node = grid.GetGridObject(x, y);
                nodeVisual.SetCostsTexts(node.fCost, x, y);
            }
        }
    }

    internal void EnableNode(PathNode node)
    {
        visualNodeArray[node.x, node.y].ChangeBgColor(_processedNodeColor);
    }

    internal void DisableNode(PathNode node)
    {
        var color = node.fCost > 0 ? _baseColor : _destinationColor;
        visualNodeArray[node.x, node.y].ChangeBgColor(color);
    }

    private void HideNodeVisuals()
    {
        foreach (var visualNode in visualNodeList)
        {
            visualNode.SetCostsTexts(9999, 9999, 9999);
            visualNode.ChangeBgColor(_baseColor);
        }
    }
}
