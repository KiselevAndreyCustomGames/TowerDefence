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

        public void Despawn(Projectile projectile)
        {
            _factorySO.Despawn(projectile);
        }
    }

    public interface IShellSpawner
    {
        public Shell SpawnShell();
        public void Despawn(Projectile projectile);
    }
}