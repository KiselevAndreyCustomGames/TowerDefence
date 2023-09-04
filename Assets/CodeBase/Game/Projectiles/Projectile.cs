using UnityEngine;

namespace CodeBase.Game.Projectiles
{
    public abstract class Projectile : MonoBehaviour, IPlayable
    {
        [SerializeField] protected bool CanUpdate = true;

        public virtual bool GameUpdate()
        {
            return CanUpdate;
        }
    }
}