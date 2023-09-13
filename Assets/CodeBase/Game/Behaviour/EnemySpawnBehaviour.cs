using CodeBase.Game.Character.Enemy;
using CodeBase.Game.Map;
using CodeBase.Infrastructure.Game;
using CodeBase.Utility.Extension;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Game
{
    [Serializable]
    public class EnemySpawnBehaviour : IRestartable
    {
        private readonly GameCollection _collection = new();

        [SerializeField] private GameEnemySpawnScenarioSO _scenario;

        private GameEnemySpawnScenarioSO.State _activeState;
        private List<ITile> _spawnTiles;
        private Action<int> _dealDamage;

        public event Action EnemiesAreOver;

        public void Start(List<ITile> spawnTiles, Action<int> dealDamage)
        {
            _spawnTiles = spawnTiles;
            _dealDamage = dealDamage;
            _scenario.Init(this);
            _activeState = _scenario.Begin();
            _collection.Init(OnEnemyNeedDespawn);
        }

        public void GameUpdate()
        {
            _collection.GameUpdate();
            if (_spawnTiles.Count > 0)
                _activeState.Progress();

            if (_collection.Count == 0
                && _activeState.IsWaveEnd)
                EnemiesAreOver?.Invoke();
        }

        public void SpawnEnemy(EnemyFactorySO factory, EnemyType type)
        {
            var spawnTile = _spawnTiles.Random();
            var enemy = factory.Spawn(type);
            enemy.Init(spawnTile, OnEnemyNeedDespawn, factory.GetRandomParameters(type));
            _collection.Add(enemy);
        }

        public void Restart()
        {
            _collection.Clear();
            _activeState = _scenario.Begin();
        }

        private void OnEnemyNeedDespawn(EnemyBehaviour enemy, bool enemyAlive = false)
        {
            if (enemyAlive)
                _dealDamage(enemy.Damage);

            EnemyFactorySO.Despawn(enemy);
        }

        private void OnEnemyNeedDespawn(IPlayable enemy) =>
            OnEnemyNeedDespawn(enemy as EnemyBehaviour);
    }
}