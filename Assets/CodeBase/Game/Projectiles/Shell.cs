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

        private float _shellBlastRadius, _damage;

        private Action<Projectile> OnHitGround;
        private Func<Explosion> SpawnExplosion;

        public void Init(Vector3 launchPoint, Vector3 targetPoint, Vector3 launchVelocity, Action<Projectile> despawn, Func<Explosion> spawnExplosion, float shellBlastRadius, float damage)
        {
            _launchPoint = launchPoint;
            _targetPoint = targetPoint;
            _launchVelocity = launchVelocity;
            _shellBlastRadius = shellBlastRadius;
            _damage = damage;

            OnHitGround = despawn;
            SpawnExplosion = spawnExplosion;

            Init(launchPoint);
        }

        public override bool GameUpdate()
        {
            Age += Time.deltaTime;

            Vector3 p = _launchPoint + _launchVelocity * Age;
            p.y -= 0.5f * g * Age * Age;
            transform.localPosition = p;

            Vector3 d = _launchVelocity;
            d.y -= g * Age;
            transform.localRotation = Quaternion.LookRotation(d);

            if (p.y < 0)
            {
                Explosion(_targetPoint, _shellBlastRadius, _damage);
                OnHitGround(this);
                return false;
            }
            
            Explosion(transform.localPosition, 0.1f, 0);

            return true;
        }

        public void Explosion(Vector3 position, float radius, float damage)
        {
            SpawnExplosion().Init(position, radius, damage, OnHitGround);
        }
    }
}