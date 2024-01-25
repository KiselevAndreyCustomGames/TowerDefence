using CodeBase.Game.Character.Enemy;
using System;
using UnityEngine;

namespace CodeBase.Game.Map
{
    public abstract class TowerTileContent : TileContent, IPlayable
    {
        [SerializeField, Range(1, 5)] protected float Range = 1.5f;
        [SerializeField, Range(1, 100)] protected float Damage = 1.5f;
        [SerializeField] protected Transform Turret;

        protected EnemyTarger Target;

        private bool _isUpdated = true;
        private Quaternion _startTurretRotation;

        public abstract TowerType TowerType { get; }

        protected bool HasTarget => IsTargetTracked() || IsAcquireTarget();

        public virtual void Init(ProjectileGameBehaviour projectiles) 
        {
            _isUpdated = true;
        }

        public virtual bool GameUpdate() => _isUpdated;

        protected virtual void EndTargeting() { }

        protected override void OnAwake()
        {
            base.OnAwake();

            _startTurretRotation = Turret.localRotation;
        }

        protected override void OnDisabling()
        {
            base.OnDisabling();

            _isUpdated = false;
            Turret.localRotation = _startTurretRotation;
        }

        private bool IsAcquireTarget()
        {
            if (EnemyTarger.FillOverlap(transform.localPosition, Range))
            {
                Target = EnemyTarger.GetOverlapTarget(0);
                return true;
            }

            Target = null;
            return false;
        }

        private bool IsTargetTracked()
        {
            if (Target == null)
                return false;

            if(Vector3.Distance(Target.Position, transform.localPosition) > Range + Target.ColliderSize
                || Target.Enemy.IsAlive == false)
            {
                Target = null;
                EndTargeting();
                return false;
            }

            return true;
        }
    }
}