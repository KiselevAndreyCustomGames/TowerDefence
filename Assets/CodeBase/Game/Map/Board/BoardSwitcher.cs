using System;
using System.Collections.Generic;

namespace CodeBase.Game.Map
{
    public class BoardSwitcher : IBoardSwitcher
    {
        private readonly Func<bool> FindPaths;
        private readonly Action<ITile, TileType> ChangeContent;
        private readonly List<ITile> _enemySpawnTiles = new();
        private readonly List<TileContent> _contentToUpdate = new();

        public List<ITile> EnemySpawnTiles => _enemySpawnTiles;

        public BoardSwitcher(Func<bool> findPaths, Action<ITile, TileType> changeContent)
        {
            FindPaths = findPaths;
            ChangeContent = changeContent;
        }

        public void ToggleDestination(ITile tile)
        {
            var currentContentType = tile.Content.Type;

            if (currentContentType == TileType.Destination)
            {
                ChangeContent(tile, TileType.Empty);
                if (FindPaths() == false)
                    ChangeContent(tile, currentContentType);
                FindPaths();
            }
            else if (currentContentType == TileType.Empty)
            {
                ChangeContent(tile, TileType.Destination);
                FindPaths();
            }
        }

        public void ToggleWall(ITile tile)
        {
            if (tile.Content.Type == TileType.Wall)
            {
                ChangeContent(tile, TileType.Empty);
                FindPaths();
            }
            else if (tile.Content.Type == TileType.Empty)
            {
                ChangeContent(tile, TileType.Wall);
                if (FindPaths() == false)
                    ChangeContent(tile, TileType.Empty);
                FindPaths();
            }
        }

        public void ToggleEnemySpawnPoint(ITile tile)
        {
            if (tile.Content.Type == TileType.EnemySpawnPoint
                && _enemySpawnTiles.Count > 1)
            {
                ChangeContent(tile, TileType.Empty);
                _enemySpawnTiles.Remove(tile);
            }
            else if (tile.Content.Type == TileType.Empty)
            {
                ChangeContent(tile, TileType.EnemySpawnPoint);
                _enemySpawnTiles.Add(tile);
            }
        }

        public void ToggleTower(ITile tile)
        {
            if (tile.Content.Type == TileType.Tower)
            {
                _contentToUpdate.Remove(tile.Content);
                ChangeContent(tile, TileType.Empty);
                FindPaths();
            }
            else if (tile.Content.Type == TileType.Empty)
            {
                ChangeContent(tile, TileType.Tower);
                if (FindPaths())
                    _contentToUpdate.Add(tile.Content);
                else
                    ChangeContent(tile, TileType.Empty);
                FindPaths();
            }
            else if (tile.Content.Type == TileType.Wall)
            {
                _contentToUpdate.Add(tile.Content);
                ChangeContent(tile, TileType.Tower);
            }
        }

        public void GameUpdate()
        {
            foreach (var content in _contentToUpdate)
                content.GameUpdate();
        }
    }
}