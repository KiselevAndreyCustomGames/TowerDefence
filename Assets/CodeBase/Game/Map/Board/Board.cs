using CodeBase.Infrastructure.Game;
using System.Collections.Generic;
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
        [SerializeField] private bool _useAlternative = true;
        [SerializeField] private LayerMask _boardMask;

        private readonly GameCollection _towers = new();

        private IBoardConstructor _constructor;
        private IBoardSwitcher _switcher;
        private ProjectileGameBehaviour _projectiles;

        public List<ITile> EnemySpawnTiles => _switcher.EnemySpawnTiles;

        public void Initialize(Vector2Int size, ProjectileGameBehaviour projectiles)
        {
            _ground.localScale = new(size.x, size.y, 1f);
            _projectiles = projectiles;
            _towers.Init(Despawn);

            _constructor = new BoardPathConstructor(size, _tileParent, _tilePrefab, _boardMask, ChangeTileContent, UseAlternative);
            _switcher = new BoardSwitcher(FindPaths, ChangeTileContent, ChangeTowerContent);
        }

        public void Restart()
        {
            _constructor.Restart();
            _switcher.Restart();
            _projectiles.Restart();

            _towers.Clear();

            FindPaths();
        }

        public void GameUpdate() => _towers.GameUpdate();

        public bool FindPaths() => _constructor.FindPaths();
        public ITile GetTile(Ray ray) => _constructor.GetTile(ray);

        public void ToggleDestination(ITile tile) => _switcher.ToggleDestination(tile);
        public void ToggleWall(ITile tile) => _switcher.ToggleWall(tile);
        public void ToggleEnemySpawnPoint(ITile tile)
        {
            _constructor.ToggleEnemySpawnPoint(tile);
            _switcher.ToggleEnemySpawnPoint(tile);
        }

        public void ToggleTower(ITile tile, TowerType towerType) => _switcher.ToggleTower(tile, towerType);

        private  void ChangeTileContent(ITile tile, TileType newType) => ChangeContent(tile, _tileContentFactory.Spawn(newType));
        private void ChangeTowerContent(ITile tile, TowerType newType)
        {
            var tower = _tileContentFactory.Spawn(newType);
            tower.Init(_projectiles);
            ChangeContent(tile, tower);

            _towers.Add(tower);
        }

        private void ChangeContent(ITile tile, TileContent newContent)
        {
            if (tile.Content != newContent)
            {
                _tileContentFactory.Despawn(tile.Content);
                tile.Content = newContent;
            }
            else
                _tileContentFactory.Despawn(newContent);
        }

        private bool UseAlternative() => _useAlternative;

        private void Despawn(IPlayable playable)
        {
            var tile = playable as Tile;
            if(tile != null)
                ChangeContent(tile, _tileContentFactory.Spawn(TileType.Empty));
        }
    }
}