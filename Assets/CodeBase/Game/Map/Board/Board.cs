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

        private BoardPathConstructor _constructor;

        public void Initialize(Vector2Int size)
        {
            _ground.localScale = new(size.x, size.y, 1f);
            _constructor = new(size, _tileParent, _tilePrefab, _tileContentFactory);
        }

        public bool FindPaths() => _constructor.FindPaths();
        public ITile GetTile(Ray ray) => _constructor.GetTile(ray);
        public void ToggleDestination(ITile tile) => _constructor.ToggleDestination(tile);

        public TileContent Spawn(TileType type) => _tileContentFactory.Spawn(type);
    }
}