namespace CodeBase.Game.Character.Enemy
{
    public interface IEnemy : IDamageable, IPlayable
    {
        public float Scale { get; }
    }
}