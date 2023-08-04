using UnityEngine;

namespace CodeBase.Game.Map
{
    public class MapBehaviour : MonoBehaviour
    {
        [SerializeField] private Board _board;
        [SerializeField] private Vector2Int _boardSize;

        private void Start()
        {
            _board.Initialize(_boardSize);
            _board.FindPathToCenter();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
                _board.FindPathToRandomPoint();
        }
    }
}