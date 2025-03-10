using UnityEngine;
using Formless.SM;
using System.Collections;

namespace Formless.Core
{
    public abstract class Entity : MonoBehaviour
    {
        public float Health { get; protected set; }
        public StateMachine StateMachine { get; private set; }
        protected Rigidbody2D _rb;
        
        [SerializeField] private float knockBackForce = 5f; // Сила откидывания
        [SerializeField] private float knockBackDuration = 0.2f; // Длительность откидывания

        protected virtual void Awake()
        {
            StateMachine = new StateMachine();
            _rb = GetComponent<Rigidbody2D>();
        }

        protected virtual void Update()
        {
            StateMachine.Update();
        }

        public virtual void TakeDamage(Transform damageSource, float damage)
        {
            Health -= damage;
            //Debug.Log($"{name} получил урон. Текущее здоровье: {Health}");

            ApplyKnockBack(damageSource);
        }

        protected virtual void ApplyKnockBack(Transform damageSource)
        {
            if (_rb == null) return;

            Vector2 direction = (transform.position - damageSource.position).normalized;
            StartCoroutine(KnockBackCoroutine(direction));
        }

        private IEnumerator KnockBackCoroutine(Vector2 direction)
        {
            _rb.linearVelocity = direction * knockBackForce;
            yield return new WaitForSeconds(knockBackDuration);
            _rb.linearVelocity = Vector2.zero;
        }
    }
}
