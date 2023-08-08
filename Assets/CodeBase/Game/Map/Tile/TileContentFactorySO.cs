using UnityEngine;

namespace CodeBase.Game.Map
{
    [CreateAssetMenu(menuName = nameof(CodeBase) + "/" + nameof(Game)  + "/" + nameof(Map) + "/" + nameof(TileContentFactorySO))]
    public class TileContentFactorySO : ScriptableObject
    {
        [SerializeField] private TileContent _empty;
        [SerializeField] private TileContent _destination;
        [SerializeField] private TileContent _wall;

        public void Despawn(TileContent content)
        {
            Lean.Pool.LeanPool.Despawn(content);
        }

        public TileContent Spawn(TileType type)
        {
            return type switch
            {
                TileType.Empty => SpawnInPool(_empty),
                TileType.Destination => SpawnInPool(_destination),
                TileType.Wall => SpawnInPool(_wall),
                _ => null,
            };
        }

        private TileContent SpawnInPool(TileContent prefab)
        {
            var instance = Lean.Pool.LeanPool.Spawn(prefab);
            instance.SetFactory(this);
            return instance;
        }
    }
}