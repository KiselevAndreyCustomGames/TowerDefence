using UnityEngine;

namespace CodeBase.Game.Map
{
    [System.Serializable]
    public class Board
    {
        [SerializeField] private Transform _ground;

        public void Initialize(Vector2Int size)
        {
            _ground.localScale = new(size.x, size.y, 1f);
        }
    }
}