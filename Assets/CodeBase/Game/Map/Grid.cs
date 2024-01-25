using UnityEngine;

namespace CodeBase
{
    public class Grid : AGrid
    {
        public Grid(int width, int height, float cellSize, Vector3 originPosition) 
            : base(width, height, cellSize, originPosition)
        {
        }
    }
}