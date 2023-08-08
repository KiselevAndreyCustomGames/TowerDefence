using UnityEngine;

namespace CodeBase.Game.Map
{
    public class MapBehaviour : MonoBehaviour
    {
        [SerializeField] private Board _board;
        [Space]
        [SerializeField] private Vector2Int _boardSize;

        private Camera _camera;

        private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Start()
        {
            _board.Initialize(_boardSize);
            _board.FindPaths();
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
                HandleTouch();
            else if (Input.GetMouseButtonUp(1))
                HandleAlternativeTouch();
        }

        private void HandleTouch()
        {
            var tile = _board.GetTile(TouchRay);
            if (tile != null)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                    Debug.Log(tile.Content.Type);
                else
                    _board.ToggleWall(tile);
            }
        }

        private void HandleAlternativeTouch()
        {
            var tile = _board.GetTile(TouchRay);
            if (tile != null)
            {
                if(Input.GetKey(KeyCode.LeftShift))
                    _board.ToggleDestination(tile);
                else
                    _board.ToggleEnemySpawnPoint(tile);
            }
        }
    }
}