namespace CodeBase.Game.Character
{
    public class HealthBehaviour : IDamageable
    {
        private float _health;

        public bool IsAlive => _health > 0;

        public void Init(float health)
        {
            _health = health;
        }

        public void TakeDamage(float damage)
        {
            _health -= damage;
        }

        public void Die()
        {
            _health = 0;
        }
    }
}