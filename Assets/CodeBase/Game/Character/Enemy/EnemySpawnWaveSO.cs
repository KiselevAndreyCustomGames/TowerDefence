using UnityEngine;

namespace CodeBase.Game.Character.Enemy
{
    [CreateAssetMenu(menuName = nameof(CodeBase) + "/" + nameof(Game) + "/" + nameof(Character) + "/" + nameof(EnemySpawnWaveSO))]
    public class EnemySpawnWaveSO : ScriptableObject
    {
        [SerializeField] private EnemySpawnSequence[] _sequences;

        public void Init(EnemySpawnBehaviour enemySpawnBehaviour)
        {
            foreach (var s in _sequences)
            {
                s.Init(enemySpawnBehaviour);
            }
        }

        public State Begin() => new(this);

        [System.Serializable]
        public struct State
        {
            private readonly EnemySpawnWaveSO _wave;
            private EnemySpawnSequence.State _sequence;
            private int _index;

            public bool IsWaveEnd => _index >= _wave._sequences.Length
                && _sequence.IsSequenceEnd;

            public State(EnemySpawnWaveSO wave)
            {
                _wave = wave;
                _sequence = _wave._sequences[0].Begin();
                _index = 0;
            }

            public float Progress(float deltaTime)
            {
                deltaTime = _sequence.Progress(deltaTime);
                while(deltaTime > 0)
                {
                    if (++_index >= _wave._sequences.Length)
                        return deltaTime;

                    _sequence = _wave._sequences[_index].Begin();
                    deltaTime = _sequence.Progress(deltaTime);
                }
                return -1f;
            }
        }
    }
}