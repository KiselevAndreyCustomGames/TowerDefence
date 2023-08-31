using CodeBase.Game.Map;
using UnityEngine;

namespace CodeBase.Infrastructure.Game
{
    [System.Serializable]
    public struct TowerStruct
    {
        [field: SerializeField] public TowerType Type { get; private set; }
        [field: SerializeField] public TowerTileContent Prefab { get; private set; }
    }
}