using CodeBase.Game.Map;
using System;
using UnityEngine;

namespace CodeBase.Game.Character.Enemy
{
    public class EnemyBehaviour : MonoBehaviour, IEnemy
    {
        [SerializeField] private Transform _model;

        private EnemyMover _mover;
        private readonly HealthBehaviour _health = new();
        private Action<EnemyBehaviour> _onEnemyEndedPathOrDie;

        public float Scale {get; private set;}

        public bool IsAlive => _health.IsAlive;

        public void Init(ITile spawnTile, Action<EnemyBehaviour> onEnemyEndedPathOrDie, EnemySpawnParameters parameters)
        {
            _mover.Init(spawnTile, parameters.Speed);
            _onEnemyEndedPathOrDie = onEnemyEndedPathOrDie;
            _model.localScale = Vector3.one * parameters.Scale;
            Scale = parameters.Scale;
            _health.Init(parameters.Health);
        }

        public bool GameUpdate()
        {
            if (_health.IsAlive == false)
            {
                OnPathEndedOrDie();
                return false;
            }
            return _mover.Update();
        }

        public void TakeDamage(float damage)
        {
            _health.TakeDamage(damage);
        }

        private void Awake()
        {
            _mover = new EnemyMover(transform, _model, OnPathEndedOrDie);
        }

        private void OnDrawGizmosSelected()
        {
            _mover?.OnDrawGizmosSelected();
        }

        private void OnPathEndedOrDie()
        {
            _onEnemyEndedPathOrDie?.Invoke(this);
        }
    }
}