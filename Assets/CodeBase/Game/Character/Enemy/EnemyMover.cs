using CodeBase.Game.Map;
using CodeBase.Utility.Extension;
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
        private Direction _direction;
        private DirectionChange _directionChange;

        private float _directionAngleFrom, _directionAngleTo;
        private float _progress;

        public EnemyMover(Transform transform, Action onPathEnded)
        {
            _transform = transform;
            _onPathEnded = onPathEnded;
        }

        public void Init(ITile spawnTile)
        {
            SetNewPathParameters(spawnTile);
            _progress = 0;
        }

        public bool Update()
        {
            _progress += Time.deltaTime;
            while (_progress >= 1)
            {
                _tileFrom = _tileTo;
                _tileTo = _tileFrom.NextTileOnPath;
                if (_tileTo == null)
                {
                    _onPathEnded();
                    return false;
                }
                PrepareNextState();
                _progress -= 1;
            }

            _transform.localPosition = Vector3.LerpUnclamped(_positionFrom, _positionTo, _progress);
            if (_directionChange != DirectionChange.None)
            {
                var angle = Mathf.LerpUnclamped(_directionAngleFrom, _directionAngleTo, _progress);
                _transform.localRotation = Quaternion.Euler(0f, angle, 0f);
            }

            return true;
        }

        private void SetNewPathParameters(ITile spawnTile)
        {
            _tileFrom = spawnTile;
            _tileTo = spawnTile.NextTileOnPath;
            _transform.localPosition = spawnTile.Transform.localPosition;
            _positionFrom = _transform.localPosition;
            _positionTo = _tileFrom.ExitPoint;
            _direction = _tileFrom.PathDirection;
            _directionChange = DirectionChange.None;
            _directionAngleFrom = _directionAngleTo = _direction.GetAngle();
            _transform.localRotation = _direction.GetRotation();
        }

        private void PrepareNextState()
        {
            _positionFrom = _positionTo;
            _positionTo = _tileFrom.ExitPoint;
            _directionChange = _direction.GetChangeDirectionTo(_tileFrom.PathDirection);
            _direction = _tileFrom.PathDirection;
            _directionAngleFrom = _directionAngleTo;

            switch (_directionChange)
            {
                case DirectionChange.None:
                    PrepareForvard();
                    break;
                case DirectionChange.TurnRight:
                    PrepareTurnRight();
                    break;
                case DirectionChange.TurnLeft:
                    PrepareTurnLeft();
                    break;
                case DirectionChange.TurnArround:
                    PrepareTurnAround();
                    break;
            }
        }

        private void PrepareForvard()
        {
            _transform.localRotation = _direction.GetRotation();
            _directionAngleTo = _direction.GetAngle();
        }

        private void PrepareTurnLeft() =>
            _directionAngleTo = _directionAngleFrom - 90f;

        private void PrepareTurnRight() =>
            _directionAngleTo = _directionAngleFrom + 90f;

        private void PrepareTurnAround() =>
            _directionAngleTo = _directionAngleFrom + 180;
    }
}