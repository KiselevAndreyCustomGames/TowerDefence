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
            _constructor = new BoardPathConstructor(size, _tileParent, _tilePrefab, _tileContentFactory);
            _switcher = new BoardSwitcher(_tileContentFactory, FindPaths);
        }

        public bool FindPaths() => _constructor.FindPaths();
        public ITile GetTile(Ray ray) => _constructor.GetTile(ray);

        public void ToggleDestination(ITile tile) => _switcher.ToggleDestination(tile);
        public void ToggleWall(ITile tile) => _switcher.ToggleWall(tile);
        public void ToggleEnemySpawnPoint(ITile tile) => _switcher.ToggleEnemySpawnPoint(tile);
    }
}