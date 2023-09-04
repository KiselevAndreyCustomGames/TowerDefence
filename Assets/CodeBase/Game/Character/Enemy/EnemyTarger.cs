using UnityEngine;

namespace CodeBase.Game.Character.Enemy
{
    public class EnemyTarger : MonoBehaviour
    {
        private static readonly Collider[] _overlapResults = new Collider[32];
        private static int _layer;

        private SphereCollider _collider;

        public IEnemy Enemy { get; private set; }
        public float ColliderSize { get; private set; }
        public static int OverlapCount { get; private set; }

        public Vector3 Position => transform.position;

        public static bool FillOverlap(Vector3 position, float range)
        {
            OverlapCount = Physics.OverlapSphereNonAlloc(position, range, _overlapResults, _layer);
            return OverlapCount > 0;
        }

        public static EnemyTarger GetOverlapTarget(int index)
        {
            var target = _overlapResults[index].GetComponent<EnemyTarger>();
            return target;
        }

        private void Awake()
        {
            Enemy = transform.root.GetComponent<IEnemy>();
            _collider = GetComponent<SphereCollider>();
            _layer = 1 << transform.root.gameObject.layer;
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