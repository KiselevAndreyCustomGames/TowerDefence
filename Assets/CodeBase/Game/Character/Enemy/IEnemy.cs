namespace CodeBase.Game.Character.Enemy
{
    public interface IEnemy : IDamageable
    {
        public float Scale { get; }
        public bool GameUpdate();
    }
}