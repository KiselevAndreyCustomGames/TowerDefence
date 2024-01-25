using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Game.Map
{
    public class PathFinder : IPathFinder
    {
        private readonly List<ITile> _pathTiles = new();

        public ITile GetNextTile(ITile currentTile)
        {
            return GetNextTile(_pathTiles.IndexOf(currentTile));
        }

        public ITile GetNextTile(int pathTileIndex)
        {
            if(pathTileIndex >= _pathTiles.Count
                || pathTileIndex < 0)
            {
                Debug.LogWarning($"{nameof(PathFinder)}.GetNextTile(pathTileIndex: {pathTileIndex}). pathTileIndex without range (0, {_pathTiles.Count})");
                return null;
            }

            return _pathTiles[pathTileIndex];
        }

        public void ShowPath()
        {
            foreach (var tile in _pathTiles)
            {
                tile.ShowPath();
            }
        }

        public bool TryGeneratePath(ITile[] tiles, ITile spawn)
        {
            _pathTiles.Clear();

            var tile = spawn;
            do
            {
                _pathTiles.Add(tile);
                tile = tile.NextTileOnPath;
            } while (tile != null);

            return true;
        }
    }
}