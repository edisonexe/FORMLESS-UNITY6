using Formless.Player;
using UnityEngine;

namespace Formless.Enemy.Projectile
{
    public class EnemyProjectile : MonoBehaviour
    {
        private float damage;
        private float destroyDelay = 5f;
        private bool _attacked = false;

        public void Initialize(float damage)
        {
            this.damage = damage;

            Destroy(gameObject, destroyDelay);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (!_attacked)
                {
                    Player.Player.Instance.TakeDamage(damage);

                    Destroy(gameObject);
                    _attacked = true;
                }
                
            }
            if (collision.gameObject.layer == LayerMask.NameToLayer("Walls") ||
                collision.gameObject.layer == LayerMask.NameToLayer("Environment"))
            {
                Destroy(gameObject);
            }
        }

        public void SetDirection(Vector2 direction, float speed)
        {
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * speed;
            }
        }
    }
}