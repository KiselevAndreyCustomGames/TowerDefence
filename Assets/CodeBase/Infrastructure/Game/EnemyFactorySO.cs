using CodeBase.Game.Character.Enemy;
using UnityEngine;

namespace CodeBase.Infrastructure.Game
{
    [CreateAssetMenu(menuName = nameof(CodeBase) + "/" + nameof(Infrastructure) + "/" + nameof(Game) + "/" + nameof(EnemyFactorySO))]
    public partial class EnemyFactorySO : GameObjectFactorySO
    {
        [SerializeField] private EnemyConfig _large, _medium, _small;

        public void Despawn(EnemyBehaviour content) =>
            Lean.Pool.LeanPool.Despawn(content);

        public EnemyBehaviour Spawn(EnemyType type) =>
            SpawnInPool(GetConfig(type).Prefab);

        public EnemySpawnParameters GetRandomParameters(EnemyType type) =>
            GetConfig(type).GetRandomParameters();

        private EnemyConfig GetConfig(EnemyType type)
        {
            return type switch
            {
                EnemyType.Large => _large,
                EnemyType.Medium => _medium,
                EnemyType.Small => _small,
                _ => _medium,
            };
        }

        private EnemyBehaviour SpawnInPool(EnemyBehaviour prefab)
        {
            var instance = SpawnGameObject(prefab);
            return instance;
        }
    }
}