using Formless.Enemy;
using UnityEngine;

namespace Formless.Room.Traps
{
    public class Spike : MonoBehaviour
    {
        private PolygonCollider2D _polygonCollider;
        private float _trapDamage = 1f;
        [SerializeField] private float _heightThreshold = 0.5f;

        private void Awake()
        {
            _polygonCollider = GetComponent<PolygonCollider2D>();
        }

        private void Start()
        {
            _polygonCollider.enabled = false;
        }

        private void EnableCollider()
        {
            _polygonCollider.enabled = true;
        }

        private void DisableCollider()
        {
            _polygonCollider.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.transform.TryGetComponent(out Player.Player player))
            {
                if (Mathf.Abs(player.transform.position.y - transform.position.y) <= _heightThreshold)
                {
                    player.TakeDamage(_trapDamage);
                }
            }
        }
    }
}