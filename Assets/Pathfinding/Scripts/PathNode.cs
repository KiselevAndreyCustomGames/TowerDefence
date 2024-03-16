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
using System.Collections.Generic;

namespace CodeMonkey
{
    public class PathNode : IComparable<PathNode>
    {
        private readonly Grid<PathNode> grid;

        public int x;
        public int y;

        public int gCost;
        public int hCost;
        public int fCost;

        public bool isWalkable;
        public PathNode cameFromNode;
        public List<PathNode> Neibours;

        public PathNode(Grid<PathNode> grid, int x, int y)
        {
            this.grid = grid;
            this.x = x;
            this.y = y;
            isWalkable = true;
        }

        public void CalculateFCost()
        {
            fCost = gCost + hCost;
        }

        public void SetIsWalkable(bool isWalkable)
        {
            this.isWalkable = isWalkable;
            grid.TriggerGridObjectChanged(x, y);
        }

        public override string ToString()
        {
            return $"{x},{y} / f: {fCost}, h: {hCost}, g: {gCost}";
        }

        public int CompareTo(PathNode other)
        {
            if (fCost > other.fCost) return 1;
            if (fCost < other.fCost) return -1;
            if (hCost < other.hCost) return -1;
            if(hCost > other.hCost) return 1;
            return 0;
        }

        public override bool Equals(object obj)
        {
            return obj is PathNode node &&
                   EqualityComparer<Grid<PathNode>>.Default.Equals(grid, node.grid) &&
                   x == node.x &&
                   y == node.y &&
                   gCost == node.gCost &&
                   hCost == node.hCost &&
                   fCost == node.fCost &&
                   isWalkable == node.isWalkable &&
                   EqualityComparer<PathNode>.Default.Equals(cameFromNode, node.cameFromNode) &&
                   EqualityComparer<List<PathNode>>.Default.Equals(Neibours, node.Neibours);
        }

        public override int GetHashCode()
        {
            HashCode hash = new();
            hash.Add(grid);
            hash.Add(x);
            hash.Add(y);
            hash.Add(gCost);
            hash.Add(hCost);
            hash.Add(fCost);
            hash.Add(isWalkable);
            hash.Add(cameFromNode);
            hash.Add(Neibours);
            return hash.ToHashCode();
        }


        public static bool operator ==(PathNode left, PathNode right) => CompareTo(left, right) == 0;
        public static bool operator !=(PathNode left, PathNode right) => CompareTo(left, right) != 0;
        public static bool operator <(PathNode left, PathNode right) => CompareTo(left, right) == -1;
        public static bool operator >(PathNode left, PathNode right) => CompareTo(left, right) == 1;
        public static bool operator <=(PathNode left, PathNode right) => CompareTo(left, right) <= 0;
        public static bool operator >=(PathNode left, PathNode right) => CompareTo(left, right) >= 0;

        private static int CompareTo(PathNode left, PathNode right)
        {
            if (right is null)
            {
                if (left is null) return 0;
                return 1;
            }
            if (left is null) return -1;
            return left.CompareTo(right);
        }
    }
}