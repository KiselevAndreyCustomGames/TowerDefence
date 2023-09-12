using CodeBase.Infrastructure.Game;
using UnityEngine;

namespace CodeBase.Game.Character.Enemy
{
    [System.Serializable]
    public class EnemySpawnSequence
    {
        [SerializeField] private EnemyFactorySO _factory;
        [SerializeField] private EnemyType _type;
        [SerializeField, Range(1, 100)] private int _amount = 3;
        [SerializeField, Range(0.1f, 10f)] private float _cooldown = 3f;

        public State Begin() => new(this);

        [System.Serializable]
        public struct State
        {
            private readonly EnemySpawnSequence _sequence;
            private int _count;
            private float _cooldown;

            public State(EnemySpawnSequence sequence)
            {
                _sequence = sequence;
                _count = 0;
                _cooldown = sequence._cooldown;
            }

            public float Progress(float deltaTime)
            {
                _cooldown += deltaTime;
                while(_cooldown >= _sequence._cooldown)
                {
                    _cooldown -= _sequence._cooldown;
                    if (_count >= _sequence._amount)
                        return _cooldown;

                    _count++;
                    EnemySpawnBehaviour.SpawnEnemy(_sequence._factory, _sequence._type);
                }

                return -1f;
            }
        }
    }
}