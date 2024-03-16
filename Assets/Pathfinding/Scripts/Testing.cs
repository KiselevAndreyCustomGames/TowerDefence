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
    
    private Pathfinding pathfindingTree;
    private Pathfinding pathfindingList;
    private bool _showTimeBetweenFrame;
    private float _time;

    private void Start() {
        pathfindingTree = new Pathfinding(wigth, heigth, new BinaryTreeDB<PathNode>());
        pathfindingList = new Pathfinding(wigth, heigth, new PathList<PathNode>());
        pathfindingDebugStepVisual.Setup(pathfindingTree.GetGrid());
        pathfindingVisual.SetGrid(pathfindingTree.GetGrid());

        //Debug.Log("binaryTreeLeft");
        //var binaryTreeLeft = new BinaryTree<int>();
        //binaryTreeLeft.AddAndReturnNode(3);
        //binaryTreeLeft.AddAndReturnNode(5);
        //binaryTreeLeft.AddAndReturnNode(4);
        //binaryTreeLeft.AddAndReturnNode(6);
        //Debug.Log(binaryTreeLeft.ToString());
        //binaryTreeLeft.Remove(5);
        //Debug.Log(binaryTreeLeft.ToString());

        //Debug.Log("binaryTreeRigth");
        //var binaryTreeRigth = new BinaryTree<int>();
        //binaryTreeRigth.AddAndReturnNode(7);
        //binaryTreeRigth.AddAndReturnNode(5);
        //binaryTreeRigth.AddAndReturnNode(6);
        //binaryTreeRigth.AddAndReturnNode(4);
        //Debug.Log(binaryTreeRigth.ToString());
        //binaryTreeRigth.Remove(5);
        //Debug.Log(binaryTreeRigth.ToString());

        //Debug.Log("binaryTreeNull");
        //var binaryTreeNull = new BinaryTree<int>();
        //binaryTreeNull.AddAndReturnNode(5);
        //binaryTreeNull.AddAndReturnNode(4);
        //binaryTreeNull.AddAndReturnNode(6);
        //Debug.Log(binaryTreeNull.ToString());
        //binaryTreeNull.Remove(5);
        //Debug.Log(binaryTreeNull.ToString());
    }

    private void Update() {
        if (Input.GetMouseButtonDown(0)) {
            _time = Time.realtimeSinceStartup;
            StartCoroutine(FindPath(true));
            //characterPathfinding.SetTargetPosition(mouseWorldPosition);
        }

        if (Input.GetMouseButtonUp(1))
        {
            _time = Time.realtimeSinceStartup;
            StartCoroutine(FindPath(false));
        }

        if (Input.GetMouseButtonDown(2)) {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
            pathfindingTree.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
            pathfindingTree.GetNode(x, y).SetIsWalkable(!pathfindingTree.GetNode(x, y).isWalkable);
            pathfindingList.GetNode(x, y).SetIsWalkable(!pathfindingList.GetNode(x, y).isWalkable);
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

    private IEnumerator FindPath(bool useTree)
    {
        Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();
        pathfindingTree.GetGrid().GetXY(mouseWorldPosition, out int x, out int y);
        List<PathNode> path;

        if(useTree)
            path = pathfindingTree.FindPath(0, 0, x, y);
        else
            path = pathfindingList.FindPath(0, 0, x, y);

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
        ClearWalls();
        ConstructRandomWalls();
    }

    private void ClearWalls()
    {
        for (int x = 0; x < wigth; x++)
        {
            for (int y = 0; y < heigth; y++)
            {
                pathfindingTree.GetNode(x, y).SetIsWalkable(true);
                pathfindingList.GetNode(x, y).SetIsWalkable(true);
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
                    pathfindingTree.GetNode(x, y).SetIsWalkable(false);
                    pathfindingList.GetNode(x, y).SetIsWalkable(false);
                }
            }
        }
    }
}
