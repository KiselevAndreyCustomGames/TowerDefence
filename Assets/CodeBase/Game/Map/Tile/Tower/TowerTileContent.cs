using CodeBase.Game.Character.Enemy;
using System;
using UnityEngine;

namespace CodeBase.Game.Map
{
    public class TowerTileContent : TileContent
    {
        [SerializeField, Range(1, 5)] private float _targetingRange = 1.5f;
        [SerializeField, Range(1, 10)] private float _damagePerSec = 1.5f;
        [SerializeField] private LayerMask _targeringMask;
        [SerializeField] private Transform _turret;
        [SerializeField] private Transform _laserBeam;

        private EnemyTarger _target;
        private Vector3 _laserBeamScale;
        private readonly Collider[] _overlapReults = new Collider[32];

        public override void GameUpdate()
        {
            if (IsTargetTracked()
               || IsAcquireTarget())
                Shoot();
        }

        protected override void OnAwake()
        {
            _laserBeamScale = _laserBeam.localScale;
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Vector3 position = transform.localPosition;
            position.y += 0.5f;
            Gizmos.DrawWireSphere(position, _targetingRange);
            if(_target != null)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(position, _target.Position);
            }
        }

        private bool IsAcquireTarget()
        {
            var overlapCount = Physics.OverlapSphereNonAlloc(transform.localPosition, _targetingRange, _overlapReults, _targeringMask.value);
            if(overlapCount > 0 )
            {
                _target = _overlapReults[0].GetComponent<EnemyTarger>();
                return true;
            }

            _target = null;
            return false;
        }

        private bool IsTargetTracked()
        {
            if (_target == null)
                return false;

            if(Vector3.Distance(_target.Position, transform.localPosition) > _targetingRange + _target.ColliderSize)
            {
                _target = null;
                EndShoot();
                return false;
            }

            return true;
        }

        private void Shoot()
        {
            var point = _target.Position;
            _turret.LookAt(point);
            _laserBeam.localRotation = _turret.localRotation;

            var distance = Vector3.Distance(_turret.position, point);
            _laserBeamScale.z = distance;
            _laserBeam.localScale = _laserBeamScale;
            _laserBeam.localPosition = _turret.localPosition + 0.5f * distance * _laserBeam.forward;

            _target.Enemy.TakeDamage(Time.deltaTime * _damagePerSec);
        }

        private void EndShoot()
        {
            _laserBeam.localScale = Vector3.zero;
        }
    }
}