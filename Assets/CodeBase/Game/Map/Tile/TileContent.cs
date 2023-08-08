using UnityEngine;

namespace CodeBase.Game.Map
{
    public enum TileType
    {
        Empty,
        Destination,
        Wall,
        EnemySpawnPoint
    }

    public class TileContent : MonoBehaviour
    {
        [field: SerializeField] public TileType Type { get; private set; }

        private TileContentFactorySO _factory;

        public void SetFactory(TileContentFactorySO factory) => _factory = factory;
        public void Despawn() => _factory.Despawn(this);
    }
}