using CodeBase.Game.Map;
using System;
using UnityEngine;

namespace CodeBase.Game.Character.Enemy
{
    public class EnemyBehaviour : MonoBehaviour, IEnemy
    {
        private EnemyMover _mover;
        private Action<EnemyBehaviour> _onEnemyEndedPath;

        public void Init(ITile tile, Action<EnemyBehaviour> onEnemyEndedPath)
        {
            transform.localPosition = tile.Transform.localPosition;
            _mover.Init(tile);
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