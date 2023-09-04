using CodeBase.Game.Character.Enemy;
using CodeBase.Utility;
using System;
using UnityEngine;

namespace CodeBase.Game.Projectiles
{
    public class Shell : Projectile
    {
        private readonly float g = PhysicsConstants.Gravity;

        private Vector3 _launchPoint, _targetPoint, _launchVelocity;

        private float _age, _shellBlastRadius, _damage;

        private Action<Projectile> OnHitGround;

        public void Init(Vector3 launchPoint, Vector3 targetPoint, Vector3 launchVelocity, Action<Projectile> despawn, float shellBlastRadius, float damage)
        {
            _launchPoint = launchPoint;
            _targetPoint = targetPoint;
            _launchVelocity = launchVelocity;
            _shellBlastRadius = shellBlastRadius;
            _damage = damage;

            OnHitGround = despawn;

            _age = 0f;
            transform.localPosition = _launchPoint;
            CanUpdate = true;
        }

        public override bool GameUpdate()
        {
            _age += Time.deltaTime;

            Vector3 p = _launchPoint + _launchVelocity * _age;
            p.y -= 0.5f * g * _age * _age;
            transform.localPosition = p;

            Vector3 d = _launchVelocity;
            d.y -= g * _age;
            transform.localRotation = Quaternion.LookRotation(d);

            if(p.y < 0)
            {
                EnemyTarger.FillOverlap(_targetPoint, _shellBlastRadius);
                for (int i = 0; i < EnemyTarger.OverlapCount; i++)
                {
                    EnemyTarger.GetOverlapTarget(i).Enemy.TakeDamage(_damage);
                }
                Explosion(_targetPoint, _shellBlastRadius, _damage);
                OnHitGround.Invoke(this);
                CanUpdate = false;
            }

            return CanUpdate;
        }

        public void Explosion(Vector3 position, float radius, float damage)
        {

        }
    }
}