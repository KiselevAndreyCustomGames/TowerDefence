using System;
using UnityEngine;

namespace CodeBase.Game.Map
{
    [SelectionBase]
    public class TileContent : MonoBehaviour
    {
        [field: SerializeField] public TileType Type { get; private set; }

        public bool IsBlockingPath => Type == TileType.Wall || Type == TileType.Tower;

        public virtual void GameUpdate() { }

        protected virtual void OnAwake() { }

        private void Awake()
        {
            OnAwake();
        }
    }
}