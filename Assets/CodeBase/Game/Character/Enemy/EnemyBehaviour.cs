using CodeBase.Game.Map;
using System;
using UnityEngine;

namespace CodeBase.Game.Character.Enemy
{
    public class EnemyBehaviour : MonoBehaviour, IEnemy
    {
        [SerializeField] private Transform _model;

        private EnemyMover _mover;
        private Action<EnemyBehaviour> _onEnemyEndedPath;

        public void Init(ITile spawnTile, Action<EnemyBehaviour> onEnemyEndedPath, EnemySpawnParameters parameters)
        {
            _mover.Init(spawnTile, parameters.Speed);
            _onEnemyEndedPath = onEnemyEndedPath;
            _model.localScale = Vector3.one * parameters.Scale;
        }

        public bool GameUpdate()
        {
            return _mover.Update();
        }

        private void Awake()
        {
            _mover = new EnemyMover(transform, _model, OnPathEnded);
        }
        private void OnDrawGizmosSelected()
        {
            _mover.OnDrawGizmosSelected();
        }

        private void OnPathEnded()
        {
            _onEnemyEndedPath?.Invoke(this);
        }
    }
}