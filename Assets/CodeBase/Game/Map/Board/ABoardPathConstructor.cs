using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Game.Map
{
    public abstract class ABoardPathConstructor : IBoardConstructor
    {
        private readonly Vector2Int _size;
        private readonly LayerMask _boardMask;
        private readonly Action<ITile, TileType> _changeContent;    // only for testing. For setting Destination to board center

        protected readonly ITile[] Tiles;
        protected readonly Func<bool> UseAlternative;
        protected readonly Dictionary<ITile, IPathFinder> Paths = new();

        public ABoardPathConstructor(Vector2Int size, Transform tileParent, Tile tilePrefab, LayerMask boardMask, Action<ITile, TileType> changeContent, Func<bool> useAlternative)
        {
            _size = size;
            _boardMask = boardMask;
            UseAlternative = useAlternative;
            _changeContent = changeContent;
            Tiles = new ITile[size.x * size.y];

            var offset = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);
            for (int i = 0, y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++, i++)
                {
                    var tile = Lean.Pool.LeanPool.Spawn(tilePrefab);
                    tile.transform.SetParent(tileParent, false);
                    tile.transform.localPosition = new Vector3(x - offset.x, 0f, y - offset.y);
                    Tiles[i] = tile;

                    if (x > 0)
                        tile.MakeWestNeibour(Tiles[i - 1]);
                    if (y > 0)
                        tile.MakeSouthNeibour(Tiles[i - size.x]);

                    changeContent(tile, TileType.Empty);
                }
            }

            SetDefaultDestination();
        }

        public abstract bool FindPaths();
        public abstract void ToggleEnemySpawnPoint(ITile tile);

        public ITile GetTile(Ray ray)
        {
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _boardMask))
            {
                int x = (int)(hit.point.x + _size.x * 0.5f);
                int y = (int)(hit.point.z + _size.y * 0.5f);
                if (x >= 0 && x < _size.x
                    && y >= 0 && y < _size.y)
                    return Tiles[x + y * _size.x];
            }

            return null;
        }

        public void ShowPaths()
        {
            foreach(var path in Paths.Values)
            {
                path.ShowPath();
            }
        }

        public virtual void Restart()
        {
            foreach (var tile in Tiles)
                _changeContent(tile, TileType.Empty);

            Paths.Clear();

            SetDefaultDestination();
        }

        private void SetDefaultDestination() =>
            _changeContent(Tiles[(int)(Tiles.Length * 0.5f)], TileType.Destination);
    }
}