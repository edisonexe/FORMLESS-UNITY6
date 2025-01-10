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

    private bool _isAttack1 = false;
    private bool _isAttack2 = false;

    public event EventHandler OnTakeHit;
    public event EventHandler OnDie;

    public int _damageAttack1;
    public int _damageAttack2;

    private int _currentHealth;

    private void Awake()
    {
        var attack01Object = transform.Find("Attack1Collider");
        var attack02Object = transform.Find("Attack2Collider");

        _attack1PolygonCollider = attack01Object.GetComponent<PolygonCollider2D>();
        _attack2PolygonCollider = attack02Object.GetComponent<PolygonCollider2D>();

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

    public void TakeDamage(int damage)
    {
        _currentHealth -= damage;
        OnTakeHit?.Invoke(this, EventArgs.Empty);
        DetectDeath();
    }

    private void DetectDeath()
    {
        if (_currentHealth <= 0)
        {
            _boxCollider2D.enabled = false;
            _attack1PolygonCollider.enabled = false;
            _attack2PolygonCollider.enabled = false;
            _capsuleCollider2D.enabled = false;

            _enemyAI.SetDeathState();
            OnDie?.Invoke(this, EventArgs.Empty);
        }
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

    private void OnTriggerEnter2D(Collider2D collision)
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
