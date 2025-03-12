using System.Collections.Generic;
using UnityEngine;

namespace Formless.Items
{
    public class Explosion : MonoBehaviour
    {
        private HashSet<Collider2D> _damagedTargets = new HashSet<Collider2D>();
        private CircleCollider2D _collider;
        private Animator _animator;
        [SerializeField] private float _explosionDamage = 10f;

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _collider.enabled = false;
        }

        public void TriggerExplosion()
        {
            float explosionTime = _animator.GetCurrentAnimatorStateInfo(0).length;
            Destroy(gameObject, explosionTime);
        }

        private void EnableCollider()
        {
            _collider.enabled = true;
        }

        private void DisableCollider()
        {
            _collider.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (_damagedTargets.Contains(collision)) return;

            if (collision.transform.TryGetComponent(out Player.Player player))
            {
                player.TakeDamage(transform, _explosionDamage);
                 _damagedTargets.Add(collision);
            }
            if (collision.transform.TryGetComponent(out Enemy.Enemy enemy))
            {
                enemy.TakeDamage(transform, _explosionDamage);
                _damagedTargets.Add(collision);
            }
        }
    }
}
