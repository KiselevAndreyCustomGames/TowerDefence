namespace CodeBase.Game.Character
{
    public interface IDamageable
    {
        public void TakeDamage(float damage);
        public bool IsAlive {  get; }
    }
}