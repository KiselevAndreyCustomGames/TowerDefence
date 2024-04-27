/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;
using CodeMonkey;
using System.Collections;

public class Testing : MonoBehaviour {
    
    [SerializeField] private PathfindingDebugStepVisual pathfindingDebugStepVisual;
    [SerializeField] private PathfindingVisual pathfindingVisual;
    [SerializeField, Range(1, 100)] private int width = 20;
    [SerializeField, Range(1, 100)] private int height = 20;
    [SerializeField, Range(0, 1)] private float wallChance = 0.3f;
    
    private Pathfinding pathfindingBinaryTree;
    private Pathfinding pathfindingTernaryTree;
    private Pathfinding pathfindingList;
    private bool _showTimeBetweenFrame;
    private float _time;

    private void Start() {
        pathfindingBinaryTree = new Pathfinding(width, height, new BinaryTreeDB<PathNode>());
        pathfindingTernaryTree = new Pathfinding(width, height, new TernaryTreeDB<PathNode>());
        pathfindingList = new Pathfinding(width, height, new PathList<PathNode>());
        pathfindingDebugStepVisual.Setup(pathfindingBinaryTree.GetGrid());
        pathfindingVisual.SetGrid(pathfindingBinaryTree.GetGrid());

    }

    private void Update() {
        if (Input.GetMouseButtonDown(2)) {
            _time = Time.realtimeSinceStartup;
            StartCoroutine(FindPath(pathfindingBinaryTree));
        }

        if (Input.GetMouseButtonUp(0))
        {
            _time = Time.realtimeSinceStartup;
            StartCoroutine(FindPath(pathfindingTernaryTree));
        }

        if (Input.GetMouseButtonDown(1))
        {
            _time = Time.realtimeSinceStartup;
            StartCoroutine(FindPath(pathfindingList));
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            PathfindingDebugStepVisual.Instance.ClearSnapshots();
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            pathfindingBinaryTree.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            var nodeNewWalkable = !pathfindingBinaryTree.GetNode(x, y).isWalkable;
            pathfindingBinaryTree.GetNode(x, y).SetIsWalkable(nodeNewWalkable);
            pathfindingTernaryTree.GetNode(x, y).SetIsWalkable(nodeNewWalkable);
            pathfindingList.GetNode(x, y).SetIsWalkable(nodeNewWalkable);
        }

        if (Input.GetKeyDown(KeyCode.X))
            ConstructRandom();

        if (Input.GetKeyDown(KeyCode.C))
            ClearWalls();

        if (Input.GetKeyDown(KeyCode.T))
            _showTimeBetweenFrame = !_showTimeBetweenFrame;

        if (_showTimeBetweenFrame)
        {
            var time = Time.time;
            if (time - _time > 0.05f)
                Debug.Log(">>-------------");
                Debug.Log(time - _time);
            _time = time;
        }
    }

    private IEnumerator FindPath(Pathfinding pathFinding)
    {
        Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
        pathFinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
        List<PathNode> path;

        path = pathFinding.FindPath(0, 0, x, y);

        if (path != null)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f, Color.green, 5f);
            }
        }

        Debug.Log(Time.realtimeSinceStartup - _time);
        Debug.Log("-------------<<");
        yield return null;
    }

    private void ConstructRandom()
    {
        PathfindingDebugStepVisual.Instance.ClearSnapshots();
        ClearWalls();
        ConstructRandomWalls();
    }

    private void ClearWalls()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                pathfindingBinaryTree.GetNode(x, y).SetIsWalkable(true);
                pathfindingTernaryTree.GetNode(x, y).SetIsWalkable(true);
                pathfindingList.GetNode(x, y).SetIsWalkable(true);
            }
        }
    }

    private void ConstructRandomWalls()
    {
        for (int x = 1; x < width - 1; x++)
        {
            for (int y = 1; y < height - 1; y++)
            {
                if (Random.value <= wallChance)
                {
                    pathfindingBinaryTree.GetNode(x, y).SetIsWalkable(false);
                    pathfindingTernaryTree.GetNode(x, y).SetIsWalkable(false);
                    pathfindingList.GetNode(x, y).SetIsWalkable(false);
                }
            }
        }
    }
}
