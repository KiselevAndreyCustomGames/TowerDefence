using CodeBase.Game.Map;
using UnityEngine;

namespace CodeBase.Game.Character.Enemy
{
    public class EnemyBehaviour : MonoBehaviour, IEnemy
    {
        private EnemyMover _mover;

        public void SpawnOn(ITile tile)
        {
            transform.localPosition = tile.Transform.localPosition;
        }

        public bool GameUpdate()
        {
            return _mover.Update();
        }

        private void Awake()
        {
            _mover = new EnemyMover(transform);
        }
    }
}