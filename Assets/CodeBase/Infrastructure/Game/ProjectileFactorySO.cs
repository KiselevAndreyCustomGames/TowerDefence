using CodeBase.Game.Projectiles;
using UnityEngine;

namespace CodeBase.Infrastructure.Game
{
    [CreateAssetMenu(menuName = nameof(CodeBase) + "/" + nameof(Infrastructure) + "/" + nameof(Game) + "/" + nameof(ProjectileFactorySO))]
    public class ProjectileFactorySO : GameObjectFactorySO
    {
        [SerializeField] private Shell _shellPrefab;

        public Shell Shell => SpawnInPool(_shellPrefab);

        public void Despawn(Projectile content) =>
            Lean.Pool.LeanPool.Despawn(content);

        private T SpawnInPool<T>(T prefab) where T : Projectile
        {
            var instance = SpawnGameObject(prefab);
            return instance;
        }
    }
}