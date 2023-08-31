using UnityEngine;

namespace CodeBase.Game.Map
{
    public class MortarTowerTileContent : TowerTileContent
    {
        [SerializeField, Range(0.5f, 2f)] private float _shootsPesSeconds = 1f;

        public override TowerType TowerType => TowerType.Mortar;
    }
}