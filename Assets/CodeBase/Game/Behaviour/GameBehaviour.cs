using UnityEngine;

namespace CodeBase.Game
{
    public class GameBehaviour : MonoBehaviour
    {
        [SerializeField] private EnemySpawnBehaviour _enemySpawner;
        [SerializeField] private MapBehaviour _map;
        [SerializeField] private ProjectileGameBehaviour _projectiles;

        private void Awake()
        {
            _map.Awake();
        }

        private void Start()
        {
            _map.Start(_projectiles);
            _enemySpawner.Init(_map.EnemySpawnTiles);
        }

        private void Update()
        {
            _enemySpawner.GameUpdate();
            Physics.SyncTransforms();
            _projectiles.GameUpdate();
            _map.GameUpdate();
        }
    }
}