using CodeBase.Game.Character.Enemy;
using System;
using UnityEngine;

namespace CodeBase.Game.Projectiles
{
    public class Explosion : Projectile
    {
        [SerializeField, Range(0f, 1f)] private float _duration = 0.5f;
        [SerializeField] private Color _color = Color.yellow;
        [SerializeField] private AnimationCurve _scaleCurve;
        [SerializeField] private AnimationCurve _colorCurve;

        private MeshRenderer _meshRenderer;
        private MaterialPropertyBlock _propertyBlock;
        private Action<Projectile> OnExpoded;

        private float _scale;

        private static int _colorPropId = Shader.PropertyToID("_BaseColor");

        public void Init(Vector3 position, float radius, float damage, Action<Projectile> despawn)
        {
            if(damage > 0f)
            {
                EnemyTarger.FillOverlap(position, radius);
                for (int i = 0; i < EnemyTarger.OverlapCount; i++)
                {
                    EnemyTarger.GetOverlapTarget(i).Enemy.TakeDamage(damage);
                }
            }

            OnExpoded = despawn;
            Init(position);

            _scale = 2 * radius;
            transform.localScale = Vector3.one * (_scale * _scaleCurve.Evaluate(0));
        }

        public override bool GameUpdate()
        {
            Age += Time.deltaTime;

            if (Age >= _duration)
            {
                OnExpoded(this);
                return false;
            }

            float t = Age / _duration;
            _color.a = _colorCurve.Evaluate(t);
            _propertyBlock.SetColor(_colorPropId, _color);
            _meshRenderer.SetPropertyBlock(_propertyBlock);

            transform.localScale = Vector3.one * (_scale * _scaleCurve.Evaluate(t));

            return true;
        }

        protected override void OnAwake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
            _propertyBlock = new MaterialPropertyBlock();
        }
    }
}