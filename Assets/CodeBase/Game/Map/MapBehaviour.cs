using UnityEngine;

namespace CodeBase.Game.Map
{
    public class MapBehaviour : MonoBehaviour
    {
        [SerializeField] private Board _board;
        [SerializeField] private Vector2Int _boardSize;

        [Space]
        [SerializeField] private TileContentFactorySO _tileContentFactory;

        private Camera _camera;

        private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Start()
        {
            _board.Initialize(_boardSize);
            _board.FindPathToCenter();
        }

        private void Update()
        {
            if (Input.GetMouseButtonUp(1))
                HandleTouch();
        }

        private void HandleTouch()
        {
            var tile = _board.GetTile(TouchRay);
            if (tile != null)
                tile.Content = _tileContentFactory.Spawn(TileType.Destination);
        }
    }
}