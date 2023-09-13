using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Game.Map
{
    public class BoardPathConstructor : IBoardConstructor
    {
        private readonly ITile[] _tiles;
        private readonly Vector2Int _size;
        private readonly LayerMask _boardMask;
        private readonly Func<bool> _useAlternative;
        private readonly Queue<ITile> _serchFrontier = new();
        private readonly Action<ITile, TileType> _changeContent;

        public BoardPathConstructor(Vector2Int size, Transform tileParent, Tile tilePrefab, LayerMask boardMask, Action<ITile, TileType> changeContent, Func<bool> useAlternative)
        {
            _size = size;
            _boardMask = boardMask;
            _useAlternative = useAlternative;
            _changeContent = changeContent;
            _tiles = new ITile[size.x * size.y];

            var offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);
            for (int i = 0, y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++, i++)
                {
                    var tile = Lean.Pool.LeanPool.Spawn(tilePrefab);
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

                    changeContent(tile, TileType.Empty);
                }
            }

            SetDefaultDestination();
        }

        public ITile GetTile(Ray ray)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _boardMask))
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
                    if (tile.IsAlternative && _useAlternative())
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
                if(tile.HasPath == false)
                    return false;

            foreach (var tile in _tiles)
                tile.ShowPath();

            return true;
        }

        public void Restart()
        {
            foreach (var tile in _tiles)
                _changeContent(tile, TileType.Empty);

            SetDefaultDestination();
        }

        private void SetDefaultDestination() =>
            _changeContent(_tiles[(int)(_tiles.Length * 0.5f)], TileType.Destination);
    }
}