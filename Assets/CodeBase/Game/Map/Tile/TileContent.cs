using UnityEngine;

namespace CodeBase.Game.Map
{
    public class TileContent : MonoBehaviour
    {
        [field: SerializeField] public TileType Type { get; private set; }
    }
}