using CodeBase.Game.Projectiles;
using CodeBase.Infrastructure.Game;
using UnityEngine;

namespace CodeBase.Game
{
    [System.Serializable]
    public class ProjectileGameBehaviour : IShellSpawner
    {
        private readonly GameCollection _collection = new();

        [SerializeField] private ProjectileFactorySO _factorySO;

        public void GameUpdate()
        {
            _collection.GameUpdate();
        }

        public Shell SpawnShell()
        {
            var shell = _factorySO.Shell;
            _collection.Add(shell);
            return shell;
        }

        public Explosion SpawnExplosion()
        {
            var explosion = _factorySO.Explosion;
            _collection.Add(explosion);
            return explosion;
        }

        public void Despawn(Projectile projectile)
        {
            _factorySO.Despawn(projectile);
        }
    }

    public interface IProjectileSpawner
    {
        public void Despawn(Projectile projectile);
    }

    public interface IShellSpawner : IExplosionSpawner
    {
        public Shell SpawnShell();
    }

    public interface IExplosionSpawner : IProjectileSpawner
    {
        public Explosion SpawnExplosion();
    }
}