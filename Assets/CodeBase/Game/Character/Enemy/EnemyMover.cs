using CodeBase.Game.Map;
using CodeBase.Utility.Extension;
using System;
using UnityEngine;

namespace CodeBase.Game.Character.Enemy
{
    public class EnemyMover
    {
        private readonly Transform _transform;
        private readonly Transform _model;
        private readonly Action _onPathEnded;
        private readonly float _invertedQuarterPI = 1 / (MathF.PI * 0.25f);

        private Vector3 _positionFrom, _positionTo;
        private ITile _tileFrom, _tileTo;
        private Direction _direction;
        private DirectionChange _directionChange;

        private float _directionAngleFrom, _directionAngleTo;
        private float _progress, _progressFactor;
        private float _speed;

        public EnemyMover(Transform transform, Transform model, Action onPathEnded)
        {
            _transform = transform;
            _model = model;
            _onPathEnded = onPathEnded;
        }

        public void Init(ITile spawnTile, float speed)
        {
            _speed = speed;
            SetNewPathParameters(spawnTile);
            _progress = 0;
        }

        public bool Update()
        {
            _progress += Time.deltaTime * _progressFactor;
            while (_progress >= 1)
            {
                if (_tileTo == null)
                {
                    _onPathEnded();
                    return false;
                }
                _progress = (_progress - 1) / _progressFactor;
                PrepareNextState();
                _progress *= _progressFactor;
            }

            if (_directionChange != DirectionChange.None)
            {
                var angle = Mathf.LerpUnclamped(_directionAngleFrom, _directionAngleTo, _progress);
                _transform.localRotation = Quaternion.Euler(0f, angle, 0f);
            }
            else
                _transform.localPosition = Vector3.LerpUnclamped(_positionFrom, _positionTo, _progress);

            return true;
        }

        private void SetNewPathParameters(ITile spawnTile)
        {
            _tileFrom = spawnTile;
            _tileTo = spawnTile.NextTileOnPath;
            _positionFrom = spawnTile.Transform.localPosition;
            _positionTo = _tileFrom.ExitPoint;
            _direction = _tileFrom.PathDirection;
            _directionChange = DirectionChange.None;
            PrepareForward();
            _directionAngleTo = _directionAngleFrom;
            _progressFactor = 2f * _speed;
        }

        private void PrepareNextState()
        {
            _tileFrom = _tileTo;
            _tileTo = _tileFrom.NextTileOnPath;
            _positionFrom = _positionTo;
            if(_tileTo == null)
            {
                PrepareFinish();
                return;
            }
            _positionTo = _tileFrom.ExitPoint;
            _directionChange = _direction.GetChangeDirectionTo(_tileFrom.PathDirection);
            _direction = _tileFrom.PathDirection;
            _directionAngleFrom = _directionAngleTo;

            switch (_directionChange)
            {
                case DirectionChange.None:
                    PrepareForward();
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

        private void PrepareForward()
        {
            _transform.localRotation = _direction.GetRotation();
            _directionAngleTo = _direction.GetAngle();
            _model.localPosition = Vector3.zero;
            _transform.localPosition = _positionFrom;
            _progressFactor = _speed;
        }

        private void PrepareTurnLeft()
        {
            _directionAngleTo = _directionAngleFrom - 90f;
            _model.localPosition = new Vector3(0.5f, 0f);
            _transform.localPosition = _positionFrom + _direction.GetHalfVector();
            _progressFactor = _speed * _invertedQuarterPI;
        }

        private void PrepareTurnRight()
        {
            _directionAngleTo = _directionAngleFrom + 90f;
            _model.localPosition = new Vector3(-0.5f, 0f);
            _transform.localPosition = _positionFrom + _direction.GetHalfVector();
            _progressFactor = _speed * _invertedQuarterPI;
        }

        private void PrepareTurnAround()
        {
            _directionAngleTo = _directionAngleFrom + 180;
            _model.localPosition = Vector3.zero;
            _transform.localPosition = _positionFrom;
            _progressFactor = 2f * _speed;
        }

        private void PrepareFinish()
        {
            _positionTo = _tileFrom.Transform.localPosition;
            _directionChange = DirectionChange.None;
            _directionAngleTo = _direction.GetAngle();
            _model.localPosition = Vector3.zero;
            _transform.localRotation = _direction.GetRotation();
            _progressFactor = 2f * _speed;
        }
    }
}