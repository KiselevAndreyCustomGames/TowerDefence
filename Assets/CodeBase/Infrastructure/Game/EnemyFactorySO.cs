using CodeBase.Game.Character.Enemy;
using UnityEngine;

namespace CodeBase.Infrastructure.Game
{
    [CreateAssetMenu(menuName = nameof(CodeBase) + "/" + nameof(Infrastructure) + "/" + nameof(Game) + "/" + nameof(EnemyFactorySO))]
    public class EnemyFactorySO : GameObjectFactorySO
    {
        [SerializeField] private EnemyBehaviour _baseEnemyPrefab;
        [SerializeField, FloatRangeSlider(0.3f, 2.5f)] private FloatRange _scale;
        [SerializeField, FloatRangeSlider(0f, 1f)] private FloatRange _scaleRange;
        [SerializeField, FloatRangeSlider(0.1f, 3f)] private FloatRange _speed;
        [SerializeField, Range(10, 10000)] private int _initHealth;

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

        public EnemySpawnParameters GetRandomParameters()
        {
            var scaleRange = _scaleRange.Random;
            var scale = _scale.PercentageAt(scaleRange);
            var percentageOfGigantizm = _scale.FindPercentage(scale);
            var speed = _speed.PercentageAt(1 - percentageOfGigantizm);
            var health = _initHealth * scale;

            return new(scale, speed, health);
        }
    }
}