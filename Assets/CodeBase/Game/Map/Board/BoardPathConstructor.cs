using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Game.Map
{
    public class BoardPathConstructor : ABoardPathConstructor
    {
        private readonly Queue<ITile> _serchFrontier = new();

        public BoardPathConstructor(Vector2Int size, Transform tileParent, Tile tilePrefab, LayerMask boardMask, Action<ITile, TileType> changeContent, Func<bool> useAlternative) 
            : base(size, tileParent, tilePrefab, boardMask, changeContent, useAlternative)
        {
        }

        public override bool FindPaths()
        {
            foreach (var tile in Tiles)
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
                    if (tile.IsAlternative && UseAlternative())
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

            foreach (var tile in Tiles)
            {
                if (tile.HasPath == false)
                    return false;
            }
            
            foreach(var path in Paths)
            {
                path.Value.TryGeneratePath(Tiles, path.Key);
            }
            ShowPaths();

            return true;
        }

        public override void ToggleEnemySpawnPoint(ITile tile)
        {
            if(Paths.Remove(tile) == false)
            {
                var pathFinder = new PathFinder();
                Paths.Add(tile, pathFinder);
            }
        }
    }

}