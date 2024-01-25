using CodeBase.Game.Map;
using CodeBase.Utility.Extension;
using System;
using UnityEngine;

namespace CodeBase.Game.Character.Enemy
{
    public class EnemyMover : AEnemyMover
    {
        public EnemyMover(Transform transform, Transform model, Action onPathEnded) 
            : base(transform, model, onPathEnded)
        {
        }

        public void Init(ITile spawnTile, float speed)
        {
            Speed = speed;
            SpawnOn(spawnTile);
            Progress = 0;
        }

    }

    public abstract class AEnemyMover
    {
        private readonly Transform _model;
        private readonly float _invertedQuarterPI = 1 / (MathF.PI * 0.25f);

        private Vector3 _positionFrom, _positionTo;
        private ITile _tileFrom, _tileTo;
        private Direction _direction;
        private DirectionChange _directionChange;

        private float _directionAngleFrom, _directionAngleTo;

        protected readonly Transform Transform;
        protected readonly Action OnPathEnded;

        protected float Progress, ProgressFactor;
        protected float Speed;

        public AEnemyMover(Transform transform, Transform model, Action onPathEnded)
        {
            Transform = transform;
            _model = model;
            OnPathEnded = onPathEnded;
        }

        public bool Update()
        {
            Progress += Time.deltaTime * ProgressFactor;
            while (Progress >= 1)
            {
                if (_tileTo == null)
                {
                    OnPathEnded.Invoke();
                    return false;
                }
                Progress = (Progress - 1) / ProgressFactor;
                PrepareNextState();
                Progress *= ProgressFactor;
            }

            if (_directionChange != DirectionChange.None)
            {
                var angle = Mathf.LerpUnclamped(_directionAngleFrom, _directionAngleTo, Progress);
                Transform.localRotation = Quaternion.Euler(0f, angle, 0f);
            }
            else
                Transform.localPosition = Vector3.LerpUnclamped(_positionFrom, _positionTo, Progress);

            return true;
        }

        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(_positionTo, 0.1f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(_positionFrom, 0.1f);
        }

        protected void SpawnOn(ITile spawnTile)
        {
            _tileFrom = spawnTile;
            _tileTo = spawnTile.NextTileOnPath;
            _positionFrom = spawnTile.Transform.localPosition;
            _positionTo = _tileFrom.ExitPoint;
            _direction = _tileFrom.PathDirection;
            _directionChange = DirectionChange.None;
            PrepareForward();
            _directionAngleFrom = _directionAngleTo;
            ProgressFactor = 2f * Speed;
        }

        protected void PrepareNextState()
        {
            _tileFrom = _tileTo;
            _tileTo = _tileFrom.NextTileOnPath;
            _positionFrom = _positionTo;
            if (_tileTo == null)
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
            Transform.SetLocalPositionAndRotation(_positionFrom, _direction.GetRotation());
            _directionAngleTo = _direction.GetAngle();
            _model.localPosition = Vector3.zero;
            ProgressFactor = Speed;
        }

        private void PrepareTurnLeft()
        {
            _directionAngleTo = _directionAngleFrom - 90f;
            _model.localPosition = new Vector3(0.5f, 0f);
            Transform.localPosition = _positionFrom + _direction.GetHalfVector();
            ProgressFactor = Speed * _invertedQuarterPI;
        }

        private void PrepareTurnRight()
        {
            _directionAngleTo = _directionAngleFrom + 90f;
            _model.localPosition = new Vector3(-0.5f, 0f);
            Transform.localPosition = _positionFrom + _direction.GetHalfVector();
            ProgressFactor = Speed * _invertedQuarterPI;
        }

        private void PrepareTurnAround()
        {
            _directionAngleTo = _directionAngleFrom + 180;
            _model.localPosition = Vector3.zero;
            Transform.localPosition = _positionFrom;
            ProgressFactor = 2f * Speed;
        }

        private void PrepareFinish()
        {
            _positionTo = _tileFrom.Transform.localPosition;
            _directionChange = DirectionChange.None;
            _directionAngleTo = _direction.GetAngle();
            _model.localPosition = Vector3.zero;
            Transform.localRotation = _direction.GetRotation();
            ProgressFactor = 2f * Speed;
        }
    }
}