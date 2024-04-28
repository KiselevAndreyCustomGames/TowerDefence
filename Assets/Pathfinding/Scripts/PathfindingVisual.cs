/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using CodeMonkey;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingVisual : MonoBehaviour {

    private IGrid<PathNode> grid;
    private bool _needUpdate;

    public void SetGrid(IGrid<PathNode> grid) {
        this.grid = grid;
        UpdateVisual();

        grid.OnGridObjectChanged += Grid_OnGridValueChanged;
    }

    private void Grid_OnGridValueChanged(object sender, OnGridObjectChangedEventArgs e)
    {
        _needUpdate = true;
        Debug.Log($"<color=yellow>Grid_OnGridValueChanged</color>");
    }

    private void LateUpdate() {
        if (_needUpdate)
        {
            _needUpdate = false;
            UpdateVisual();
        }
    }

    private void UpdateVisual() {
        for (int x = 0; x < grid.Width; x++)
        {
            for (int y = 0; y < grid.Height; y++)
            {
                if (grid.GetGridObject(x, y).isWalkable)
                    PathfindingDebugStepVisual.Instance.UnlockNode(x, y);
                else
                    PathfindingDebugStepVisual.Instance.LockNode(x, y);
            }
        }
    }
}

