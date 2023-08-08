using CodeBase.Game.Character.Enemy;
using UnityEngine;

namespace CodeBase.Infrastructure.Game
{
    [CreateAssetMenu(menuName = nameof(CodeBase) + "/" + nameof(Infrastructure) + "/" + nameof(Game) + "/" + nameof(EnemyFactorySO))]
    public class EnemyFactorySO : GameObjectFactorySO
    {
        [SerializeField] private EnemyBehaviour _baseEnemyPrefab;

        public void Despawn(EnemyBehaviour content) =>
            Lean.Pool.LeanPool.Despawn(content);

        public EnemyBehaviour Spawn()
        {
            return SpawnInPool(_baseEnemyPrefab);
        }

        private EnemyBehaviour SpawnInPool(EnemyBehaviour prefab)
        {
            var instance = SpawnGameObject(prefab);
            return instance;
        }
    }
}