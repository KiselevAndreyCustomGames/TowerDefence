using CodeBase.Game.Character.Enemy;
using System;
using UnityEngine;

namespace CodeBase.Game.Map
{
    public abstract class TowerTileContent : TileContent
    {
        [SerializeField, Range(1, 5)] protected float Range = 1.5f;
        [SerializeField, Range(1, 100)] protected float Damage = 1.5f;
        [SerializeField] private LayerMask _targeringMask;
        [SerializeField] protected Transform Turret;

        protected  EnemyTarger Target;
        private readonly Collider[] _overlapResults = new Collider[32];

        public abstract TowerType TowerType { get; }

        public override void GameUpdate()
        {
            if (IsTargetTracked()
               || IsAcquireTarget())
                Shoot();
        }

        protected virtual void Shoot() { }
        protected virtual void EndTargeting() { }

        private bool IsAcquireTarget()
        {
            var overlapCount = Physics.OverlapSphereNonAlloc(transform.localPosition, Range, _overlapResults, _targeringMask.value);
            if(overlapCount > 0 )
            {
                Target = _overlapResults[0].GetComponent<EnemyTarger>();
                return true;
            }

            Target = null;
            return false;
        }

        private bool IsTargetTracked()
        {
            if (Target == null)
                return false;

            if(Vector3.Distance(Target.Position, transform.localPosition) > Range + Target.ColliderSize)
            {
                Target = null;
                EndTargeting();
                return false;
            }

            return true;
        }
    }
}