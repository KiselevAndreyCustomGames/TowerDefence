using CodeBase.Game.Map;
using CodeBase.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase
{
    public class Testing : MonoBehaviour
    {
        [SerializeField] private Transform _plane;
        [SerializeField, Range(1, 20)] private int _width;
        [SerializeField, Range(1, 20)] private int _heigth;
        [SerializeField, Range(1, 100)] private float _scale;
        [SerializeField] private Vector3 _originPosition;


        private int _index = 0;

        private AGrid<TestGridObject> _grid;
        private PathfindingAStar _pathfinding;
        private PathNode _startNode = new PathNode(0, 0);
        private PathNode _endNode = new PathNode(0, 0);

        private void Start()
        {
            //_grid = new GridXZ<TestGridObject>(_width, _heigth, _scale, _originPosition, (int x, int y) => new TestGridObject());
            //new GridXZDebugger<TestGridObject>(_grid);

            _pathfinding = new PathfindingAStar(_width, _heigth, _scale, _originPosition, true);

            _plane.localPosition = _originPosition;
            _plane.localScale = new Vector3(_width, 0.1f, _heigth) * _scale;

            //Grid grid = new(5, 2, 10f);
            //new GridXZ<int>(3, 6, 5, new Vector3(-10, 0, -20));
            //new GridXZ<int>(2, 2, 20, new Vector3(10, 0, 20));
            //new GridXZ<int>(6, 3, 15, new Vector3(-100, 0, -20));
        }

        private void Update()
        {
            if(Input.GetMouseButtonUp(0))
            {
                var pointPosition = UtilsClass.GetMouseWorldPosition3D();
                if(pointPosition != null )
                {
                    //_grid.SetValue((Vector3)pointPosition, ++_index);
                    _startNode = _pathfinding.Grid.GetGridObject((Vector3)pointPosition);
                    var path = _pathfinding.FindPath(_startNode, _endNode);
                    ShowPath(path);
                }
            }

            if (Input.GetMouseButtonUp(1))
            {
                var pointPosition = UtilsClass.GetMouseWorldPosition3D();
                if (pointPosition != null)
                {
                    //Debug.Log(_grid.GetGridObject((Vector3)pointPosition));
                    _endNode = _pathfinding.Grid.GetGridObject((Vector3)pointPosition);
                    var path = _pathfinding.FindPath(_startNode, _endNode);
                    ShowPath(path);
                }
            }

            if (Input.GetMouseButtonUp(2))
            {
                var pointPosition = UtilsClass.GetMouseWorldPosition3D();
                if (pointPosition != null)
                {
                    var node = _pathfinding.Grid.GetGridObject((Vector3)pointPosition);
                    node.IsWalkable = !node.IsWalkable;
                }
            }
        }

        private void ShowPath(List<PathNode> pathNodes)
        {
            if(pathNodes != null)
            {
                for(int i = 0; i < pathNodes.Count - 1; i++)
                {
                    Debug.DrawLine(
                        _scale * (new Vector3(pathNodes[i].X, 0, pathNodes[i].Y) + new Vector3(1, 0, 1) * 0.5f) + _originPosition, 
                        _scale * (new Vector3(pathNodes[i + 1].X, 0, pathNodes[i + 1].Y) + new Vector3(1, 0, 1) * 0.5f) + _originPosition, 
                        Color.green, 10f);
                }
            }
        }
    }

    public class TestGridObject
    {
        public int index;

        public void DoSomething()
        {
            Debug.Log(++index);
        }
    }
}