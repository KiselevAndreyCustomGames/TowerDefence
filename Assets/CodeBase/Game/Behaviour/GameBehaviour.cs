using UnityEngine;

namespace CodeBase.Game
{
    public class GameBehaviour : MonoBehaviour
    {
        [SerializeField] private EnemySpawnBehaviour _enemySpawner;
        [SerializeField] private MapBehaviour _map;

        private void Awake()
        {
            _map.Awake();
        }

        private void Start()
        {
            _map.Start();
            _enemySpawner.Init(_map.EnemySpawnTiles);
        }

        private void Update()
        {
            _enemySpawner.GameUpdate();
            Physics.SyncTransforms();
            _map.GameUpdate();
        }
    }
}