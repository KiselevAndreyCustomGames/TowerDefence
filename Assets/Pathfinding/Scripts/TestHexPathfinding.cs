using CodeMonkey;
using UnityEngine;

public class TestHexPathfinding : MonoBehaviour
{
    [SerializeField] private PathfindingDebugStepVisual pathfindingDebugStepVisual;
    [SerializeField] private PathfindingVisual pathfindingVisual;
    [SerializeField, Range(1, 100)] private int width = 20;
    [SerializeField, Range(1, 100)] private int height = 20;
    [SerializeField, Range(0, 1)] private float wallChance = 0.3f;

    private HexPathfinding _pathfinding;

    private void Start()
    {
        _pathfinding = new HexPathfinding(width, height, new TernaryTreeDB<PathNode>());
        pathfindingDebugStepVisual.Setup(_pathfinding.GetGrid());
        pathfindingVisual.SetGrid(_pathfinding.GetGrid());

    }
}
