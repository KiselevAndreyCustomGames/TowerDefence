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
    [SerializeField] private CharacterPathfindingMovementHandler characterPathfinding;
    [SerializeField, Range(1, 100)] private int wigth = 20;
    [SerializeField, Range(1, 100)] private int heigth = 20;
    [SerializeField, Range(0, 1)] private float wallChance = 0.3f;
    
    private Pathfinding pathfinding;
    private bool _showTimeBetweenFrame;
    private float _time;

    private void Start() {
        pathfinding = new Pathfinding(wigth, heigth);
        pathfindingDebugStepVisual.Setup(pathfinding.GetGrid());
        pathfindingVisual.SetGrid(pathfinding.GetGrid());

        var binaryTree = new BinaryTree<int>();

        binaryTree.Add(2);
        Debug.Log(binaryTree.ToString());
        binaryTree.Add(3);
        Debug.Log(binaryTree.ToString());
        binaryTree.Add(10);
        Debug.Log(binaryTree.ToString());
        binaryTree.Add(1);
        Debug.Log(binaryTree.ToString());
        binaryTree.Add(6);
        Debug.Log(binaryTree.ToString());
        binaryTree.Add(4);
        Debug.Log(binaryTree.ToString());
        binaryTree.Add(7);
        Debug.Log(binaryTree.ToString());
        binaryTree.Add(14);
        Debug.Log(binaryTree.ToString());
        binaryTree.Add(16);
        Debug.Log(binaryTree.ToString());

        binaryTree.Remove(3);
        Debug.Log(binaryTree.ToString());

        binaryTree.Remove(8);
        Debug.Log(binaryTree.ToString());
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            _time = Time.realtimeSinceStartup;
            StartCoroutine(FindPath(true));
            //characterPathfinding.SetTargetPosition(mouseWorldPosition);
        }

        if (Input.GetMouseButtonDown(1)) {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x, y).isWalkable);
        }

        if(Input.GetMouseButtonUp(2))
        {
            _time = Time.realtimeSinceStartup;
            StartCoroutine(FindPath(false));
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
                Debug.Log(time - _time);
            _time = time;
        }
    }

    private IEnumerator FindPath(bool useSimple)
    {
        Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
        pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
        List<PathNode> path = pathfinding.FindPath(0, 0, x, y, useSimple);
        if (path != null)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f, Color.green, 5f);
            }
        }

        Debug.Log(Time.realtimeSinceStartup - _time);
        yield return null;
    }

    private void ConstructRandom()
    {
        ClearWalls();
        ConstructRandomWalls();
    }

    private void ClearWalls()
    {
        for (int x = 0; x < wigth; x++)
        {
            for (int y = 0; y < heigth; y++)
            {
                pathfinding.GetNode(x, y).SetIsWalkable(true);
            }
        }
    }

    private void ConstructRandomWalls()
    {
        for (int x = 1; x < wigth - 1; x++)
        {
            for (int y = 1; y < heigth - 1; y++)
            {
                if (Random.value <= wallChance)
                {
                    pathfinding.GetNode(x, y).SetIsWalkable(false);
                }
            }
        }
    }
}
