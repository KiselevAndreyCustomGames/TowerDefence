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
            return x + "," + y;
        }

        public int CompareTo(PathNode other)
        {
            if (fCost > other.fCost)
                return 1;
            else if (fCost < other.fCost)
                return -1;
            else
            {
                if (hCost < other.hCost)
                    return -1;
                else if(hCost > other.hCost)
                    return 1;
                else
                    return 0;
            } 
        }
    }
}