using CodeBase.Game.Character.Enemy;
using UnityEngine;

namespace CodeBase.Infrastructure.Game
{
    public partial class EnemyFactorySO
    {
        [System.Serializable]
        public class EnemyConfig
        {
            [field: SerializeField] public  EnemyBehaviour Prefab { get; private set; }
            [SerializeField, FloatRangeSlider(0.3f, 2.5f)] private FloatRange _scale;
            [SerializeField, FloatRangeSlider(0.1f, 3f)] private FloatRange _speed;
            [SerializeField, FloatRangeSlider(10, 10000)] private FloatRange _health;

            public EnemySpawnParameters GetRandomParameters()
            {
                var scale = _scale.Random;
                var percentageOfGigantizm = _scale.FindPercentage(scale);
                var speed = _speed.PercentageAt(1 - percentageOfGigantizm);
                var health = _health.PercentageAt(percentageOfGigantizm);

                return new(scale, speed, health);
            }
        }
    }
}