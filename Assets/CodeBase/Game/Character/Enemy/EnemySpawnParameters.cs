using UnityEngine;

namespace CodeBase.Game.Character.Enemy
{
    public readonly struct EnemySpawnParameters
    {
        public float Scale { get; }
        public float Speed { get; }

        public EnemySpawnParameters(float scale, float speed)
        {
            Scale = scale;
            Speed = speed;
        }
    }
}