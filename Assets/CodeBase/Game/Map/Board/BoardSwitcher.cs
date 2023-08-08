using System;
using System.Collections.Generic;

namespace CodeBase.Game.Map
{
    public class BoardSwitcher : IBoardSwitcher
    {
        private readonly TileContentFactorySO _tileContentFactory;
        private readonly Func<bool> FindPaths;

        private readonly List<ITile> _enemySpawnTiles = new();

        public BoardSwitcher(TileContentFactorySO tileContentFactory, Func<bool> findPaths)
        {
            _tileContentFactory = tileContentFactory;
            FindPaths = findPaths;
        }

        public void ToggleDestination(ITile tile)
        {
            var currentContentType = tile.Content.Type;

            if (currentContentType == TileType.Destination)
            {
                tile.Content = _tileContentFactory.Spawn(TileType.Empty);
                if (FindPaths() == false)
                    tile.Content = _tileContentFactory.Spawn(currentContentType);
                FindPaths();
            }
            else if (currentContentType == TileType.Empty)
            {
                tile.Content = _tileContentFactory.Spawn(TileType.Destination);
                FindPaths();
            }
        }

        public void ToggleWall(ITile tile)
        {
            if (tile.Content.Type == TileType.Wall)
            {
                tile.Content = _tileContentFactory.Spawn(TileType.Empty);
                FindPaths();
            }
            else if (tile.Content.Type == TileType.Empty)
            {
                tile.Content = _tileContentFactory.Spawn(TileType.Wall);
                if (FindPaths() == false)
                    tile.Content = _tileContentFactory.Spawn(TileType.Empty);
                FindPaths();
            }
        }

        public void ToggleEnemySpawnPoint(ITile tile)
        {
            if (tile.Content.Type == TileType.EnemySpawnPoint
                && _enemySpawnTiles.Count > 1)
            {
                tile.Content = _tileContentFactory.Spawn(TileType.Empty);
                _enemySpawnTiles.Remove(tile);
            }
            else if (tile.Content.Type == TileType.Empty)
            {
                tile.Content = _tileContentFactory.Spawn(TileType.EnemySpawnPoint);
                _enemySpawnTiles.Add(tile);
            }
        }
    }
}