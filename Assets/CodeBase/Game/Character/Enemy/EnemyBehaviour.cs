using CodeBase.Game.Map;
using UnityEngine;

namespace CodeBase.Game.Character.Enemy
{
    public class EnemyBehaviour : MonoBehaviour
    {
        public void SpawnOn(ITile tile)
        {
            transform.localPosition = tile.Transform.localPosition;
        }
    }
}