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
        private static readonly GameCollection _collection = new();
        private static List<ITile> _spawnTiles;

        [SerializeField] private GameEnemySpawnScenarioSO _scenario;

        private GameEnemySpawnScenarioSO.State _activeState;

        public void Init(List<ITile> spawnTiles)
        {
            _spawnTiles = spawnTiles;
            _activeState = _scenario.Begin();
        }

        public void GameUpdate()
        {
            _collection.GameUpdate();
            if(_spawnTiles.Count > 0)
                _activeState.Progress();
        }

        public static void SpawnEnemy(EnemyFactorySO factory, EnemyType type)
        {
            var spawnTile = _spawnTiles.Random();
            var enemy = factory.Spawn(type);
            enemy.Init(spawnTile, OnEnemyNeedDespawn, factory.GetRandomParameters(type));
            _collection.Add(enemy);
        }

        private static void OnEnemyNeedDespawn(EnemyBehaviour enemy) =>
            EnemyFactorySO.Despawn(enemy);
    }
}