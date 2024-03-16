using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Game.Map
{
    public class PathFinderAStar : APathFinder
    {
        public override bool TryGeneratePath(ITile[] tiles, ITile spawn)
        {
            _pathTiles.Clear();

            // Find destinations
            List<ITile> destinations = new();
            foreach (var tile in tiles)
            {
                if(tile.Content.Type == TileType.Destination)
                {
                    destinations.Add(tile);
                }
            }

            // просчитать пути для каждого выхода
            // выбрать кратчайший путь

            return false;
        }
    }

    public struct Path
    {
        /// <summary>
        /// Has the path reached its destination?
        /// </summary>
        public bool IsReached;

        public int Length;
    }

    public class PathfindingAStar
    {
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        private readonly GridXZ<PathNode> _grid;
        private readonly GridXZDebugger<PathNode> _debugger;
        private readonly bool _canMoveDiagonally;

        private List<PathNode> _openList = new List<PathNode>();
        private List<PathNode> _closeList = new List<PathNode>();

        public GridXZ<PathNode> Grid => _grid;

        public PathfindingAStar(int width, int height, float cellSize, Vector3 originPosition, bool canMoveDiagonally = false)
        {
            _grid = new GridXZ<PathNode>(width, height, cellSize, originPosition, (int x, int y) => new PathNode(x, y));
            new GridXZDebugger<PathNode>(_grid);

            _canMoveDiagonally = canMoveDiagonally;
        }

        public List<PathNode> FindPath(PathNode startNode, PathNode endNode)
        {
            return FindPath(startNode.X, startNode.Y, endNode.X, endNode.Y);
        }

        private List<PathNode> FindPath(int startX, int startY, int endX, int endY)
        {
            _openList.Clear();
            _closeList.Clear();

            for (int x = 0; x < _grid.Width; x++)
            {
                for (int y = 0; y < _grid.Height; y++)
                {
                    var pathNode = GetNode(x, y);
                    pathNode.GCost = int.MaxValue;
                    pathNode.CalculateFCost();
                    pathNode.CameFromeNode = null;
                }
            }

            var startNode = GetNode(startX, startY);
            var endNode = GetNode(endX, endY);

            _openList.Add(startNode);

            startNode.GCost = 0;
            startNode.HCost = CalculateDistanceCost(startNode, endNode);
            startNode.CalculateFCost();
            _grid.TriggerGridObjectChanged(startNode.X, startNode.Y);

            while (_openList.Count > 0)
            {
                var currentNode = GetLowestFCostNode(_openList);
                
                if(currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                _openList.Remove(currentNode);
                _closeList.Add(currentNode);

                foreach (var neihbourNode in GetNeighbourList(currentNode))
                {
                    if (_closeList.Contains(neihbourNode))
                    {
                        continue;
                    }

                    if(neihbourNode.IsWalkable == false)
                    {
                        _closeList.Add(neihbourNode);
                        continue;
                    }

                    var tentativeGCost = currentNode.GCost + CalculateDistanceCost(currentNode, neihbourNode);
                    if(tentativeGCost < neihbourNode.GCost)
                    {
                        neihbourNode.CameFromeNode = currentNode;
                        neihbourNode.HCost = CalculateDistanceCost(neihbourNode, endNode);
                        neihbourNode.GCost = tentativeGCost;
                        neihbourNode.CalculateFCost();
                    }

                    if(_openList.Contains(neihbourNode) == false)
                    {
                        _openList.Add(neihbourNode);
                    }

                    _grid.TriggerGridObjectChanged(neihbourNode.X, neihbourNode.Y);
                }
            }

            return null;
        }

        private PathNode GetNode(int x, int y)
        {
            return _grid.GetGridObject(x, y);
        }

        private int CalculateDistanceCost(PathNode a, PathNode b)
        {
            var xDistance = Mathf.Abs(a.X - b.X);
            var yDistance = Mathf.Abs(a.Y - b.Y);

            if (_canMoveDiagonally)
            {
                var remaining = Mathf.Abs(xDistance - yDistance);
                return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
            }
            else
            {
                return xDistance + yDistance;
            }
        }

        private PathNode GetLowestFCostNode(List<PathNode> pathNodeList)
        {
            var lowestFCostNode = pathNodeList[0];
            for ( var i = 1; i < pathNodeList.Count; i++)
            {
                if (pathNodeList[i].FCost < lowestFCostNode.FCost
                    || (pathNodeList[i].FCost == lowestFCostNode.FCost
                    && pathNodeList[i].HCost < lowestFCostNode.HCost))
                {
                    lowestFCostNode = pathNodeList[i];
                }
            }
            return lowestFCostNode;
        }

        private List<PathNode> CalculatePath(PathNode endNote)
        {
            var result = new List<PathNode>
            {
                endNote
            };

            var currentNode = endNote;
            while(currentNode.CameFromeNode != null)
            {
                result.Add(currentNode.CameFromeNode);
                currentNode = currentNode.CameFromeNode;
            }

            result.Reverse();

            return result;
        }

        private List<PathNode> GetNeighbourList(PathNode currentNode)
        {
            var result = new List<PathNode>();

            if(currentNode.X - 1 >= 0)
            {
                // Left
                result.Add(GetNode(currentNode.X - 1, currentNode.Y));

                if(_canMoveDiagonally)
                {
                    // Left Down
                    if(currentNode.Y - 1 >= 0) result.Add(GetNode(currentNode.X - 1, currentNode.Y - 1));
                    // Left Up
                    if(currentNode.Y + 1 < _grid.Height) result.Add(GetNode(currentNode.X - 1, currentNode.Y + 1));
                }
            }

            if(currentNode.X + 1 < _grid.Width)
            {
                // Rigth
                result.Add(GetNode(currentNode.X + 1, currentNode.Y));

                if(_canMoveDiagonally)
                {
                    // Rigth Down
                    if (currentNode.Y - 1 >= 0) result.Add(GetNode(currentNode.X + 1, currentNode.Y - 1));
                    // Rigth Up
                    if (currentNode.Y + 1 < _grid.Height) result.Add(GetNode(currentNode.X + 1, currentNode.Y + 1));
                }
            }

            // Down
            if(currentNode.Y - 1 >= 0) result.Add(GetNode(currentNode.X, currentNode.Y - 1));
            // Up
            if (currentNode.Y + 1 < _grid.Height) result.Add(GetNode(currentNode.X, currentNode.Y + 1));

            return result;
        }
    }

    public class PathNode
    {
        public readonly int X, Y;

        /// <summary>
        /// Full node cost
        /// </summary>
        public int FCost;

        /// <summary>
        /// Traveled distance/cost from start
        /// </summary>
        public int GCost;

        /// <summary>
        /// Approximate distance/cost to finish
        /// </summary>
        public int HCost;

        public bool IsWalkable = true;

        public PathNode CameFromeNode;

        public PathNode(int x, int y)
        {
            X = x; Y = y;
        }

        public override string ToString()
        {
            return $"{X}, {Y}\n{GCost} \t {HCost} \n{FCost}";
        }

        public string ToStringShort()
        {
            return $"X: {X}, Y: {Y}";
        }

        public void CalculateFCost()
        {
            FCost = GCost + HCost;
        }
    }
}