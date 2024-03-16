using System;
using UnityEngine;

namespace CodeBase
{
    public class Grid<TGridObject> : AGrid<TGridObject>
    {
        public Grid(int width, int height, float cellSize, Vector3 originPosition, Func<int, int, TGridObject> createGridObject) 
            : base(width, height, cellSize, originPosition, createGridObject)
        {
        }
    }
}