using CodeBase.Game.Map;
using System;
using UnityEngine;

namespace CodeBase.Game.Character.Enemy
{
    public class EnemyMover
    {
        private readonly Transform _transform;
        private readonly Action _onPathEnded;

        private Vector3 _positionFrom, _positionTo;
        private ITile _tileFrom, _tileTo;

        private float _progress;

        public EnemyMover(Transform transform, Action onPathEnded)
        {
            _transform = transform;
            _onPathEnded = onPathEnded;
        }

        public void Init(ITile tileFrom)
        {
            SetNewPathParameters(tileFrom);
            _progress = 0;
        }

        public bool Update()
        {
            _progress += Time.deltaTime;
            while(_progress >= 1)
            {
                _tileFrom = _tileTo;
                _tileTo = _tileFrom.NextTileOnPath;
                if (_tileTo == null)
                {
                    _onPathEnded();
                    return false;
                }
                _positionFrom = _tileFrom.Transform.localPosition;
                _positionTo = _tileTo.Transform.localPosition;
                _progress -= 1;
            }

            _transform.localPosition = Vector3.LerpUnclamped(_positionFrom, _positionTo, _progress);
            return true;
        }

        private void SetNewPathParameters(ITile tileFrom)
        {
            _tileFrom = tileFrom;
            _tileTo = tileFrom.NextTileOnPath;
            _positionFrom = _tileFrom.Transform.localPosition;
            _positionTo = _tileTo.Transform.localPosition;
        }
    }
}