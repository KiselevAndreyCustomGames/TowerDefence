using CodeBase.Game.Map;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CodeBase.Infrastructure.Game
{
    [CreateAssetMenu(menuName = nameof(CodeBase) + "/" + nameof(Infrastructure)  + "/" + nameof(Game) + "/" + nameof(TileContentFactorySO))]
    public class TileContentFactorySO : GameObjectFactorySO
    {
        [SerializeField] private TileContent _emptyPrefab;
        [SerializeField] private TileContent _destinationPrefab;
        [SerializeField] private TileContent _wallPrefab;
        [SerializeField] private TileContent _spawnPointPrefab;
        [SerializeField] private List<TowerStruct> _towers;

        public void Despawn(TileContent content) =>
            Lean.Pool.LeanPool.Despawn(content);

        public TileContent Spawn(TileType type)
        {
            return type switch
            {
                TileType.Empty => SpawnInPool(_emptyPrefab),
                TileType.Destination => SpawnInPool(_destinationPrefab),
                TileType.Wall => SpawnInPool(_wallPrefab),
                TileType.EnemySpawnPoint => SpawnInPool(_spawnPointPrefab),
                _ => null,
            };
        }

        public TowerTileContent Spawn(TowerType type) =>
            SpawnInPool(_towers.First(t => t.Type == type).Prefab);

        private T SpawnInPool<T>(T prefab) where T : TileContent
        {
            var instance = SpawnGameObject(prefab);
            return instance;
        }
    }
}