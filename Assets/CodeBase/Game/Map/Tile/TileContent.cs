using UnityEngine;

namespace CodeBase.Game.Map
{
    [SelectionBase]
    public class TileContent : MonoBehaviour
    {
        [field: SerializeField] public TileType Type { get; private set; }

        public bool IsBlockingPath => Type == TileType.Wall || Type == TileType.Tower;

        protected virtual void OnAwake() { }
        protected virtual void OnDisabling() { }

        private void Awake()
        {
            OnAwake();
        }

        private void OnDisable()
        {
            OnDisabling();
        }
    }
}