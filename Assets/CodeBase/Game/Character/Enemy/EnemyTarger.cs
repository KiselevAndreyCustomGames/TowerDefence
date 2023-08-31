using UnityEngine;

namespace CodeBase.Game.Character.Enemy
{
    public class EnemyTarger : MonoBehaviour
    {
        private SphereCollider _collider;

        public IEnemy Enemy { get; private set; }
        public float ColliderSize { get; private set; }

        public Vector3 Position => transform.position;

        private void Awake()
        {
            Enemy = transform.root.GetComponent<IEnemy>();
            _collider = GetComponent<SphereCollider>();
        }

        private void OnEnable()
        {
            Invoke(nameof(ReCalculate), Time.deltaTime);
        }

        private void ReCalculate()
        {
            ColliderSize = transform.localScale.x * _collider.radius * Enemy.Scale;
        }
    }
}