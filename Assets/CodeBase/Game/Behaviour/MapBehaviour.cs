using CodeBase.Game.Map;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Game
{
    [System.Serializable]
    public class MapBehaviour
    {
        [SerializeField] private Board _board;
        [Space]
        [SerializeField] private Vector2Int _boardSize;

        private Camera _camera;

        private Ray TouchRay => _camera.ScreenPointToRay(Input.mousePosition);

        public List<ITile> EnemySpawnTiles => _board.EnemySpawnTiles;

        public void Awake()
        {
            _camera = Camera.main;
        }

        public void Start()
        {
            _board.Initialize(_boardSize);
            _board.FindPaths();
        }

        public void GameUpdate()
        {
            if (Input.GetMouseButtonUp(0))
                HandleTouch();
            else if (Input.GetMouseButtonUp(1))
                HandleAlternativeTouch();

            _board.GameUpdate();
        }

        private void HandleTouch()
        {
            var tile = _board.GetTile(TouchRay);
            if (tile != null)
            {
                if (Input.GetKey(KeyCode.LeftShift))
                    _board.ToggleWall(tile);
                else
                    _board.ToggleTower(tile);
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