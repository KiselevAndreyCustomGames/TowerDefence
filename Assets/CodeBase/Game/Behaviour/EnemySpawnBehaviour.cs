using CodeBase.Game.Character.Enemy;
using CodeBase.Game.Map;
using CodeBase.Infrastructure.Game;
using CodeBase.Utility.Extension;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Game
{
    [System.Serializable]
    public class EnemySpawnBehaviour
    {
        private readonly EnemyCollection _collection = new();

        [SerializeField] private EnemyFactorySO _enemyFactory;
        [SerializeField, Range(0.1f, 5f)] private float _enemySpawnSpeed;
        [SerializeField, Range(1, 100)] private int _enemyMaxCount;

        private List<ITile> _spawnTiles;

        private float _spawnProgress;
        private bool _canSpawn = false;

        public void Init(List<ITile> spawnTiles)
        {
            _spawnTiles = spawnTiles;
        }

        public void OnUpdate()
        {
            if(_canSpawn == false && _spawnProgress < 1f)
                _spawnProgress += Time.deltaTime * _enemySpawnSpeed;
            else
            {
                _canSpawn = true;
                SpawnEnemy();
            }

            _collection.GameUpdate();
        }

        private void SpawnEnemy()
        {
            if (_spawnTiles.Count == 0
                || _collection.Count >= _enemyMaxCount)
                return;

            var spawnTile = _spawnTiles.Random();
            var enemy = _enemyFactory.Spawn();
            enemy.Init(spawnTile, OnEnemyEndedPath, _enemyFactory.GetRandomParameters());
            _collection.Add(enemy);

            _spawnProgress -= 1f;
        }

        private void OnEnemyEndedPath(EnemyBehaviour enemy)
        {
            _enemyFactory.Despawn(enemy);
        }
    }
}