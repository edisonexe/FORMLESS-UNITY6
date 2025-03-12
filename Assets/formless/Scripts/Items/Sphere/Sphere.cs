using Formless.Enemy;
using UnityEngine;

namespace Formless.Items
{
    public class Sphere : MonoBehaviour
    {
        [SerializeField] private float _sphereDamage;
        [SerializeField] private float _damageCooldown;
        private bool _canDealDamage = true;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.TryGetComponent(out Enemy.Enemy enemy) && _canDealDamage)
            {
                enemy.TakeDamage(transform, _sphereDamage);
                
                _canDealDamage = false;

                StartCoroutine(ResetDamageCooldown());
            }
        }

        private System.Collections.IEnumerator ResetDamageCooldown()
        {
            yield return new WaitForSeconds(_damageCooldown);

            _canDealDamage = true;
        }

        public void SetDamageCooldown(float cooldown)
        {
            _damageCooldown = cooldown;
        }
    }
}
