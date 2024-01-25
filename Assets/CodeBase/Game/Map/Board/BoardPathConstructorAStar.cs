using System;
using UnityEngine;

namespace CodeBase.Game.Map
{
    public class BoardPathConstructorAStar : ABoardPathConstructor
    {
        public BoardPathConstructorAStar(Vector2Int size, Transform tileParent, Tile tilePrefab, LayerMask boardMask, Action<ITile, TileType> changeContent, Func<bool> useAlternative) 
            : base(size, tileParent, tilePrefab, boardMask, changeContent, useAlternative)
        {
        }

        public override bool FindPaths()
        {
            return true;
        }

        public override void ToggleEnemySpawnPoint(ITile tile)
        {
            throw new NotImplementedException();
        }
    }

}