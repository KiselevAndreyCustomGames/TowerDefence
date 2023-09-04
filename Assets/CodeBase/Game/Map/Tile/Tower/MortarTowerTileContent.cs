using CodeBase.Utility;
using UnityEngine;

namespace CodeBase.Game.Map
{
    public class MortarTowerTileContent : TowerTileContent
    {
        [SerializeField, Range(0.5f, 2f)] private float _shootsPesSeconds = 1f;
        [SerializeField, Range(0.5f, 3f)] private float _shellBlastRadius = 1f;

        private bool _canShoot = false;
        private float _shootProgress = 1f;
        private float _launchSpeed;
        private Vector3 _shellLaunchVelocity = new();
        private IShellSpawner _projectiles;

        private readonly float g = PhysicsConstants.Gravity;

        public override TowerType TowerType => TowerType.Mortar;

        public override void Init(ProjectileGameBehaviour projectiles)
        {
            _projectiles = projectiles;

            base.Init(projectiles);
        }

        public override bool GameUpdate()
        {
            if (HasTarget)
                LookAtTarget();

            if (_canShoot == false)
            {
                _shootProgress -= Time.deltaTime * _shootsPesSeconds;
                _canShoot = _shootProgress <= 0f;
            }

            return base.GameUpdate();
        }

        protected override void OnAwake()
        {
            OnValidate();
        }

        private void OnValidate()
        {
            var x = Range + 0.251f;
            var y = -Turret.position.y;
            _launchSpeed = Mathf.Sqrt(g * (y + Mathf.Sqrt(x * x + y * y)));
        }

        private void LookAtTarget()
        {
            Vector3 launchPoint = Turret.position;
            Vector3 targetPoint = Target.Position;
            targetPoint.y = 0f;

            Vector3 dir;
            dir.x = targetPoint.x - launchPoint.x;
            dir.y = 0;
            dir.z = targetPoint.z - launchPoint.z;

            float x = dir.magnitude;
            float y = -launchPoint.y;
            dir /= x;

            float s = _launchSpeed;
            float s2 = s * s;

            float r = s2 * s2 - g * (g * x * x + 2f * y * s2);
            r = Mathf.Max(0, r);

            float tanTheta = (s2 + Mathf.Sqrt(r)) / (g * x);
            float cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
            float sinTheta = cosTheta * tanTheta;

            Turret.localRotation = Quaternion.LookRotation(new Vector3(dir.x, tanTheta, dir.z));

            if (_canShoot)
            {
                _shellLaunchVelocity.x = s * cosTheta * dir.x;
                _shellLaunchVelocity.y = s * sinTheta;
                _shellLaunchVelocity.z = s * cosTheta * dir.z;

                _projectiles.SpawnShell().Init(launchPoint, targetPoint,
                    new Vector3(s * cosTheta * dir.x, s * sinTheta, s * cosTheta * dir.z),
                    _projectiles.Despawn, _projectiles.SpawnExplosion,
                    _shellBlastRadius, Damage);

                _canShoot = false;
                _shootProgress = 1f;
            }
        }
    }
}