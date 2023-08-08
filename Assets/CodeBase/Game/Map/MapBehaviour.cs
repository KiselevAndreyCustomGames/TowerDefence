using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Game.Map
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

        public void OnAwake()
        {
            _camera = Camera.main;
        }

        public void OnStart()
        {
            _board.Initialize(_boardSize);
            _board.FindPaths();
        }

        public void OnUpdate()
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