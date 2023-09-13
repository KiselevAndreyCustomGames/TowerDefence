namespace CodeBase.Game.Character.Enemy
{
    public readonly struct EnemySpawnParameters
    {
        public float Scale { get; }
        public float Speed { get; }
        public float Health { get; }
        public int Damage { get; }

        public EnemySpawnParameters(float scale, float speed, float health, int damage)
        {
            Scale = scale;
            Speed = speed;
            Health = health;
            Damage = damage;
        }
    }
}