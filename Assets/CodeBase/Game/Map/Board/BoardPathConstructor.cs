using System.Collections.Generic;
using UnityEngine;

using static Lean.Pool.LeanPool;

namespace CodeBase.Game.Map
{
    public class BoardPathConstructor
    {
        private readonly Vector2Int _size;
        private readonly ITile[] _tiles;
        private readonly Queue<ITile> _serchFrontier = new();
        private readonly TileContentFactorySO _tileContentFactory;

        public BoardPathConstructor(Vector2Int size, Transform tileParent, Tile tilePrefab, TileContentFactorySO tileContentFactory)
        {
            _size = size;
            _tileContentFactory = tileContentFactory;

            _tiles = new ITile[size.x * size.y];

            var offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);
            for (int i = 0, y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++, i++)
                {
                    var tile = Spawn(tilePrefab);
                    tile.transform.SetParent(tileParent, false);
                    tile.transform.localPosition = new Vector3(x - offset.x, 0f, y - offset.y);
                    _tiles[i] = tile;

                    if (x > 0)
                        tile.MakeWestNeibour(_tiles[i - 1]);
                    if (y > 0)
                        tile.MakeSouthNeibour(_tiles[i - size.x]);

                    tile.IsAlternative = (x & 1) == 0;
                    if ((y & 1) == 0)
                        tile.IsAlternative = !tile.IsAlternative;

                    tile.Content = _tileContentFactory.Spawn(TileType.Empty);
                }
            }
        }

        public ITile GetTile(Ray ray)
        {
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                int x = (int)(hit.point.x + _size.x * 0.5f);
                int y = (int)(hit.point.z + _size.y * 0.5f);
                if (x >= 0 && x < _size.x
                    && y >= 0 && y < _size.y)
                    return _tiles[x + y * _size.x];
            }

            return null;
        }

        public bool FindPaths()
        {
            foreach (var tile in _tiles)
            {
                if(tile.Content.Type == TileType.Destination)
                {
                    tile.BecameDestination();
                    _serchFrontier.Enqueue(tile);
                }
                else 
                    tile.ClearPath();
            }

            if (_serchFrontier.Count == 0)
                return false;

            while (_serchFrontier.Count > 0)
            {
                var tile = _serchFrontier.Dequeue();
                if (tile != null)
                {
                    if (tile.IsAlternative)
                    {
                        _serchFrontier.Enqueue(tile.GrowPathNorth());
                        _serchFrontier.Enqueue(tile.GrowPathSouth());
                        _serchFrontier.Enqueue(tile.GrowPathEast());
                        _serchFrontier.Enqueue(tile.GrowPathWest());
                    }
                    else
                    {
                        _serchFrontier.Enqueue(tile.GrowPathWest());
                        _serchFrontier.Enqueue(tile.GrowPathEast());
                        _serchFrontier.Enqueue(tile.GrowPathSouth());
                        _serchFrontier.Enqueue(tile.GrowPathNorth());
                    }
                }
            }

            foreach (var tile in _tiles)
                tile.ShowPath();

            return true;
        }

        public void ToggleDestination(ITile tile)
        {
            var currentContentType = tile.Content.Type;

            if (currentContentType == TileType.Destination)
            {
                tile.Content = _tileContentFactory.Spawn(TileType.Destination);
                if (FindPaths() == false)
                    tile.Content = _tileContentFactory.Spawn(currentContentType);
            }
            else if (currentContentType == TileType.Empty)
                tile.Content = _tileContentFactory.Spawn(TileType.Destination);

            FindPaths();
        }
    }
}