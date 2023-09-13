using Unity.VisualScripting;
using UnityEngine;

namespace CodeBase.Game.Character.Enemy
{
    [CreateAssetMenu(menuName = nameof(CodeBase) + "/" + nameof(Game) + "/" + nameof(Character) + "/" + nameof(GameEnemySpawnScenarioSO))]
    public class GameEnemySpawnScenarioSO : ScriptableObject
    {
        [SerializeField] private EnemySpawnWaveSO[] _waves;

        public void Init(EnemySpawnBehaviour enemySpawnBehaviour)
        {
            foreach (var w in _waves)
            {
                w.Init(enemySpawnBehaviour);
            }
        }

        public State Begin() => new(this);

        [System.Serializable]
        public struct State
        {
            private readonly GameEnemySpawnScenarioSO _scenario;
            private EnemySpawnWaveSO.State _wave;
            private int _index;

            public bool IsWaveEnd => _index >= _scenario._waves.Length
                && _wave.IsWaveEnd;

            public State(GameEnemySpawnScenarioSO wave)
            {
                _scenario = wave;
                _wave = _scenario._waves[0].Begin();
                _index = 0;
            }

            public bool Progress()
            {
                float deltaTime = _wave.Progress(Time.deltaTime);
                while (deltaTime >= 0)
                {
                    if (++_index >= _scenario._waves.Length)
                        return false;

                    _wave = _scenario._waves[_index].Begin();
                    deltaTime = _wave.Progress(deltaTime);
                }
                return true;
            }
        }
    }
}