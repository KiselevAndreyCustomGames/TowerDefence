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

        public void Init(ITile spawnTile, Action<EnemyBehaviour> onEnemyEndedPath)
        {
            _mover.Init(spawnTile);
            _onEnemyEndedPath = onEnemyEndedPath;
        }

        public bool GameUpdate()
        {
            return _mover.Update();
        }

        private void Awake()
        {
            _mover = new EnemyMover(transform, OnPathEnded);
        }

        private void OnPathEnded()
        {
            _onEnemyEndedPath?.Invoke(this);
        }
    }
}