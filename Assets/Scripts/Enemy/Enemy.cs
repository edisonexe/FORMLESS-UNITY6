using System;
using UnityEngine;

[SelectionBase]
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemySO _enemySO;
    [SerializeField] private EnemyAI _enemyAI;

    public PolygonCollider2D _attack1PolygonCollider;
    public PolygonCollider2D _attack2PolygonCollider;
    private BoxCollider2D _boxCollider2D;
    private CapsuleCollider2D _capsuleCollider2D;

    public event EventHandler OnTakeHit;
    public event EventHandler OnDie;

    public int _currentHealth;
    protected int _damageAttack1;
    protected int _damageAttack2;

    private bool _isAttack1 = false;
    private bool _isAttack2 = false;
    private bool _isDead = false;

    public virtual void Awake()
    {
        var attack1Object = transform.Find("Attack1Collider");
        var attack2Object = transform.Find("Attack2Collider");

        _attack1PolygonCollider = attack1Object.GetComponent<PolygonCollider2D>();
        _attack2PolygonCollider = attack2Object.GetComponent<PolygonCollider2D>();

        _boxCollider2D = GetComponent<BoxCollider2D>();
        _capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        _currentHealth = _enemySO.enemyHealth;
        _damageAttack1 = _enemySO.enemyDamageAttack1;
        _damageAttack2 = _enemySO.enemyDamageAttack2;
        Attack1ColliderTurnOff();
        Attack2ColliderTurnOff();
    }

    public void Attack1ColliderTurnOn()
    {
        _isAttack1 = true;
        _attack1PolygonCollider.enabled = true;
    }

    public void Attack1ColliderTurnOff()
    {
        _attack1PolygonCollider.enabled = false;
    }

    public void Attack2ColliderTurnOn()
    {
        _isAttack2 = true;
        _attack2PolygonCollider.enabled = true;
    }

    public void Attack2ColliderTurnOff()
    {
        _attack2PolygonCollider.enabled = false;
    }

    public void TakeDamage(int damage)
    {
        if (_currentHealth > 0)
        {
            _currentHealth -= damage;
            OnTakeHit?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnTakeHit?.Invoke(this, EventArgs.Empty);
            _isDead = true;
        }
    }

    public void HandleDeath()
    {
        if (!_isDead) return;

        _boxCollider2D.enabled = false;
        _attack1PolygonCollider.enabled = false;
        _attack2PolygonCollider.enabled = false;
        _capsuleCollider2D.enabled = false;

        _enemyAI.SetDeathState();
        OnDie?.Invoke(this, EventArgs.Empty);
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            if (_isAttack1)
            {
                player.TakeDamage(transform, _damageAttack1);
                _isAttack1 = false;
            }
            else if (_isAttack2)
            {
                player.TakeDamage(transform, _damageAttack2);
                _isAttack2 = false;
            }
        }
    }

}
