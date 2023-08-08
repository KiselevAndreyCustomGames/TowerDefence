using UnityEngine;

namespace CodeBase.Game.Map
{
    [CreateAssetMenu(menuName = nameof(CodeBase) + "/" + nameof(Game)  + "/" + nameof(Map) + "/" + nameof(TileContentFactorySO))]
    public class TileContentFactorySO : ScriptableObject
    {
        [SerializeField] private TileContent _emptyPrefab;
        [SerializeField] private TileContent _destinationPrefab;
        [SerializeField] private TileContent _wallPrefab;
        [SerializeField] private TileContent _spawnPointPrefab;

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

        private TileContent SpawnInPool(TileContent prefab)
        {
            var instance = Lean.Pool.LeanPool.Spawn(prefab);
            instance.SetFactory(this);
            return instance;
        }
    }
}