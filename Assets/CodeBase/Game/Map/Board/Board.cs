using CodeBase.Infrastructure.Game;
using UnityEngine;


namespace CodeBase.Game.Map
{
    [System.Serializable]
    public class Board : IBoard
    {
        [SerializeField] private Transform _ground;
        [SerializeField] private Transform _tileParent;
        [SerializeField] private Tile _tilePrefab;
        [SerializeField] private TileContentFactorySO _tileContentFactory;

        private IBoardConstructor _constructor;
        private IBoardSwitcher _switcher;

        public void Initialize(Vector2Int size)
        {
            _ground.localScale = new(size.x, size.y, 1f);
            _constructor = new BoardPathConstructor(size, _tileParent, _tilePrefab, ChangeContent);
            _switcher = new BoardSwitcher(FindPaths, ChangeContent);
        }

        public bool FindPaths() => _constructor.FindPaths();
        public ITile GetTile(Ray ray) => _constructor.GetTile(ray);

        public void ToggleDestination(ITile tile) => _switcher.ToggleDestination(tile);
        public void ToggleWall(ITile tile) => _switcher.ToggleWall(tile);
        public void ToggleEnemySpawnPoint(ITile tile) => _switcher.ToggleEnemySpawnPoint(tile);

        private void ChangeContent(ITile tile, TileType newType)
        {
            var content = _tileContentFactory.Spawn(newType);
            if (tile.Content != content)
            {
                _tileContentFactory.Despawn(tile.Content);
                tile.Content = content;
            }
            else
                _tileContentFactory.Despawn(content);
        }
    }
}