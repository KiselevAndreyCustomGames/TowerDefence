/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;
using CodeMonkey;

public class PathfindingDebugStepVisual : MonoBehaviour {

    public static PathfindingDebugStepVisual Instance { get; private set; }

    [SerializeField] private PathfindingDebugStepVisualNode pfPathfindingDebugStepVisualNode;
    [SerializeField, Range(0, 0.1f)] private float autoShowSnapshotsTimerMax = .05f;
    [SerializeField] private Color _baseColor = UtilsClass.GetColorFromString("636363");
    [SerializeField] private Color _currentNodeColor;
    [SerializeField] private Color _processedNodeColor;
    [SerializeField] private Color _inOpenListColor = UtilsClass.GetColorFromString("009AFF");
    [SerializeField] private Color _inCloseListColor = new(1, 0, 0);

    private bool _needShowNode = false;
    private bool autoShowSnapshots;
    private float autoShowSnapshotsTimer;
    private PathfindingDebugStepVisualNode[,] visualNodeArray;

    private readonly List<PathfindingDebugStepVisualNode> visualNodeList = new();
    private readonly List<PathfindingDebugStepVisualNode> _neighbours = new();
    private readonly List<GridSnapshotAction> gridSnapshotActionList = new();
    private readonly List<PathNode> _activedNodes = new();

    private PathNode _prevCurrentNode;
    private PathNode _prevNode;
    private IGrid<PathNode> _grid;
    private PathfindingDebugStepVisualNode _prevSelectedVisualNode;

    private void Awake() {
        Instance = this;
    }

    public void Setup(IGrid<PathNode> grid) {
        _grid = grid;
        visualNodeArray = new PathfindingDebugStepVisualNode[grid.Width, grid.Height];

        for (int x = 0; x < grid.Width; x++) {
            for (int y = 0; y < grid.Height; y++) {
                Vector3 gridPosition = grid.GetWorldPosition(x, y);
                var visualNode = CreateVisualNode(gridPosition);
                visualNodeArray[x, y] = visualNode;
                visualNodeList.Add(visualNode);
            }
        }

        HideNodeVisuals();
    }

    private void Update() {
        if (Input.GetKey(KeyCode.Space))
            ShowNextSnapshot();

        if (Input.GetKeyDown(KeyCode.N))
            ShowNextSnapshot();

        if (Input.GetKeyDown(KeyCode.Return))
            autoShowSnapshots = true;

        if (Input.GetKeyDown(KeyCode.O))
        {
            _needShowNode = !_needShowNode;
            if(_needShowNode == false)
            {
                if (_prevSelectedVisualNode != null)
                {
                    _prevSelectedVisualNode.ChangeBgColor(_baseColor);

                    foreach (var node in _neighbours)
                        node.ChangeBgColor(_baseColor);
                }
            }
        }

        if (autoShowSnapshots)
        {
            autoShowSnapshotsTimer -= Time.deltaTime;
            if (autoShowSnapshotsTimer <= 0f)
            {
                autoShowSnapshotsTimer += autoShowSnapshotsTimerMax;
                ShowNextSnapshot();
                if (gridSnapshotActionList.Count == 0) 
                    autoShowSnapshots = false;
            }
        }

        if(gridSnapshotActionList.Count == 0 && _needShowNode)
        {
            var mouseWorldPos = UtilsClass.GetMouseWorldPosition();
            _grid.GetXY(mouseWorldPos, out int x, out int y);
            var currentVisualNode = visualNodeArray[x, y];
            if (_prevSelectedVisualNode != currentVisualNode)
            {
                if (_prevSelectedVisualNode != null)
                {
                    _prevSelectedVisualNode.ChangeBgColor(_baseColor);

                    foreach (var node in _neighbours)
                        node.ChangeBgColor(_baseColor);
                }

                currentVisualNode.ChangeBgColor(_currentNodeColor);
                _prevSelectedVisualNode = currentVisualNode;

                _neighbours.Clear();
                var neighbours = _grid.GetGridObject(x, y).Neibours;
                foreach (var neighbour in neighbours)
                {
                    var node = visualNodeArray[neighbour.x, neighbour.y];
                    node.ChangeBgColor(_processedNodeColor);
                    _neighbours.Add(node);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            var mouseWorldPos = UtilsClass.GetMouseWorldPosition();
            _grid.GetXY(mouseWorldPos, out int x, out int y);
            var str = $"<color=red>{mouseWorldPos}</color>";
            str += $"\ncurrent - x: {x}, y: {y}";
            var neighbours = _grid.GetGridObject(x, y).Neibours;
            foreach (var neighbour in neighbours)
                str += $"\n{neighbour}";
            Debug.Log(str);
        }
    }

    private void ShowNextSnapshot() {
        if (gridSnapshotActionList.Count > 0) {
            GridSnapshotAction gridSnapshotAction = gridSnapshotActionList[0];
            gridSnapshotActionList.RemoveAt(0);
            gridSnapshotAction.TriggerAction();
        }
    }

    public void ClearSnapshots() {
        gridSnapshotActionList.Clear();
        _activedNodes.Clear();
        HideNodeVisuals();
    }

    public void TakeSnapshot(PathNode current, IPathFindData<PathNode> nodeList, List<PathNode> closedList, bool isCurrentNode = false)
    {
        GridSnapshotAction gridSnapshotAction = new();

        if (isCurrentNode) 
            TakeSnapshot(ref _prevCurrentNode, nodeList, closedList, gridSnapshotAction);
        
        TakeSnapshot(ref _prevNode, nodeList, closedList, gridSnapshotAction);

        var x = current.x;
        var y = current.y;
        var gCost = current.gCost;
        var hCost = current.hCost;
        var fCost = current.fCost;
        gridSnapshotAction.AddAction(() =>
        {
            var visualNode = visualNodeArray[x, y];
            visualNode.SetCostsTexts(fCost, hCost, gCost);
            visualNode.ChangeBgColor(isCurrentNode ? _currentNodeColor : _processedNodeColor);
        });

        if(isCurrentNode) _prevCurrentNode = current;
        else _prevNode = current;

        if (_activedNodes.Contains(current) == false)
            _activedNodes.Add(current);

        gridSnapshotActionList.Add(gridSnapshotAction);
    }

    public void LockNode(int x, int y)
    {
        visualNodeArray[x, y].Lock(Color.black);
    }

    public void UnlockNode(int x, int y)
    {
        visualNodeArray[x, y].Unlock(_baseColor);
    }

    private void TakeSnapshot(ref PathNode prev, IPathFindData<PathNode> nodeList, List<PathNode> closedList, GridSnapshotAction gridSnapshotAction)
    {
        if (prev != null)
        {
            bool isInOpenList = nodeList.Contains(prev);
            bool isInClosedList = closedList.Contains(prev);
            var tmpX = prev.x;
            var tmpY = prev.y;
            gridSnapshotAction.AddAction(() =>
            {
                var visualNode = visualNodeArray[tmpX, tmpY];

                Color backgroundColor = _baseColor;

                if (isInClosedList)
                {
                    backgroundColor = _inCloseListColor;
                }
                if (isInOpenList)
                {
                    backgroundColor = _inOpenListColor;
                }

                visualNode.ChangeBgColor(backgroundColor);
            });
        }
    }

    public void TakeSnapshotFinalPath(Grid<PathNode> grid, List<PathNode> path) {
        GridSnapshotAction gridSnapshotAction = new();
        gridSnapshotAction.AddAction(HideNodeVisuals);
        
        foreach (var pathNode in _activedNodes)
        {
            int gCost = pathNode.gCost;
            int hCost = pathNode.hCost;
            int fCost = pathNode.fCost;
            bool isInPath = path.Contains(pathNode);
            int tmpX = pathNode.x;
            int tmpY = pathNode.y;

            gridSnapshotAction.AddAction(() => {
                var visualNode = visualNodeArray[tmpX, tmpY];
                visualNode.SetCostsTexts(fCost, hCost, gCost);

                Color backgroundColor = isInPath ? _currentNodeColor : _baseColor;
                visualNode.ChangeBgColor(backgroundColor);
            });
        }

        gridSnapshotActionList.Add(gridSnapshotAction);
    }

    public int GetSnapshotsCount() { return gridSnapshotActionList.Count; }

    private void HideNodeVisuals() {
        foreach (var visualNode in visualNodeList) {
            visualNode.SetCostsTexts(9999, 9999, 9999);
            visualNode.ChangeBgColor(_baseColor);
        }
    }

    private PathfindingDebugStepVisualNode CreateVisualNode(Vector3 position) {
        PathfindingDebugStepVisualNode visualNode = Instantiate(pfPathfindingDebugStepVisualNode, position, Quaternion.identity);
        return visualNode;
    }

    private class GridSnapshotAction {

        private Action action;

        public GridSnapshotAction() {
            action = () => { };
        }

        public void AddAction(Action action) {
            this.action += action;
        }

        public void TriggerAction() {
            action();
        }

    }

}

