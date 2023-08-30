using UnityEngine;

namespace CodeBase.Game.Map
{
    public class TileContent : MonoBehaviour
    {
        [field: SerializeField] public TileType Type { get; private set; }

        public bool IsBlockingPath => Type == TileType.Wall || Type == TileType.Tower;
    }
}