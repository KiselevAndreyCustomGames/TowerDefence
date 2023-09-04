using UnityEngine;

namespace CodeBase.Game.Map
{
    public class LaserTowerTileContent : TowerTileContent
    {
        [SerializeField] private Transform _laserBeam;

        private Vector3 _laserBeamScale;

        public override TowerType TowerType => TowerType.Laser;


        public override bool GameUpdate()
        {
            if (HasTarget)
                Shoot();

            return base.GameUpdate();
        }

        protected override void OnAwake()
        {
            _laserBeamScale = _laserBeam.localScale;
        }

        protected override void EndTargeting()
        {
            _laserBeam.localScale = Vector3.zero;
        }

        private void Shoot()
        {
            var point = Target.Position;
            Turret.LookAt(point);
            _laserBeam.localRotation = Turret.localRotation;

            var distance = Vector3.Distance(Turret.position, point);
            _laserBeamScale.z = distance;
            _laserBeam.localScale = _laserBeamScale;
            _laserBeam.localPosition = Turret.localPosition + 0.5f * distance * _laserBeam.forward;

            Target.Enemy.TakeDamage(Time.deltaTime * Damage);
        }
    }
}
