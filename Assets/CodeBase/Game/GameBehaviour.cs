using CodeBase.Game.Map;
using UnityEngine;

namespace CodeBase.Game
{
    public class GameBehaviour : MonoBehaviour
    {
        [SerializeField] private EnemySpawnBehaviour _enemySpawner;
        [SerializeField] private MapBehaviour _map;

        private void Awake()
        {
            _map.OnAwake();
        }

        private void Start()
        {
            _map.OnStart();
            _enemySpawner.Init(_map.EnemySpawnTiles);
        }

        private void Update()
        {
            _map.OnUpdate();
            _enemySpawner.OnUpdate();
        }
    }
}