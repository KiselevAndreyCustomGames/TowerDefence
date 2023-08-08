using UnityEngine;

namespace CodeBase.Game.Character.Enemy
{
    //[System.Serializable]
    public class EnemyMover
    {
        private readonly Transform _transform;

        public EnemyMover(Transform transform)
        {
            _transform = transform;
        }

        public bool Update()
        {
            _transform.localPosition += Vector3.forward * Time.deltaTime;
            return true;
        }
    }
}