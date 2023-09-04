using UnityEngine;

namespace CodeBase.Game.Projectiles
{
    public abstract class Projectile : MonoBehaviour, IPlayable
    {
        protected float Age;

        public virtual bool GameUpdate() => true;

        protected void Init(Vector3 position)
        {
            Age = 0f;
            transform.localPosition = position;
        }

        protected virtual void OnAwake() { }

        private void Awake()
        {
            OnAwake();
        }
    }
}