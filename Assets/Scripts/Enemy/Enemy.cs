using System;
using UnityEngine;

[SelectionBase]
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemySO _enemySO;
    [SerializeField] protected EnemyAI _enemyAI;

    public PolygonCollider2D basicAttackPolygonCollider;
    public PolygonCollider2D strongAttackPolygonCollider;
    protected BoxCollider2D _boxCollider2D;
    protected CapsuleCollider2D capsuleCollider2D;

    public event EventHandler OnTakeHit;
    public event EventHandler OnDie;

    public float currentHealth;
    protected float damageBasicAttack;
    protected float damageStrongAttack;

    private bool _isBasicAttack = false;
    private bool _isStrongAttack = false;
    protected bool _isDead = false;

    public float CurrentHealth => currentHealth;

    public virtual void Awake()
    {
        var basicAttackObject = transform.Find("BasicAttack");
        var strongAttackObject = transform.Find("StrongAttack");

        basicAttackPolygonCollider = basicAttackObject.GetComponent<PolygonCollider2D>();
        strongAttackPolygonCollider = strongAttackObject.GetComponent<PolygonCollider2D>();

        _boxCollider2D = GetComponent<BoxCollider2D>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    private void Start()
    {
        currentHealth = _enemySO.enemyHealth;
        damageBasicAttack = _enemySO.enemyBasicAttackDamage;
        damageStrongAttack = _enemySO.enemyStrongAttackDamage;
        BasicAttackColliderDisable();
        StrongAttackColliderDisable();
    }

    public void BasicAttackColliderEnable()
    {
        _isBasicAttack = true;
        basicAttackPolygonCollider.enabled = true;
    }

    public void BasicAttackColliderDisable()
    {
        basicAttackPolygonCollider.enabled = false;
    }

    public void StrongAttackColliderEnable()
    {
        _isStrongAttack = true;
        strongAttackPolygonCollider.enabled = true;
    }

    public void StrongAttackColliderDisable()
    {
        strongAttackPolygonCollider.enabled = false;
    }

    public virtual void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth > 0)
        {
            //currentHealth -= damage;
            OnTakeHit?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            OnTakeHit?.Invoke(this, EventArgs.Empty);
            _isDead = true;
        }
    }

    public virtual void HandleDeath()
    {
        if (!_isDead) return;

        _boxCollider2D.enabled = false;
        basicAttackPolygonCollider.enabled = false;
        strongAttackPolygonCollider.enabled = false;
        capsuleCollider2D.enabled = false;

        _enemyAI.SetDeathState();
        OnDie?.Invoke(this, EventArgs.Empty);
    }

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out Player player))
        {
            if (_isBasicAttack)
            {
                player.TakeDamage(transform, damageBasicAttack);
                _isBasicAttack = false;
            }
            else if (_isStrongAttack)
            {
                player.TakeDamage(transform, damageStrongAttack);
                _isStrongAttack = false;
            }
        }
    }

    protected virtual void InvokeOnTakeHit()
    {
        OnTakeHit?.Invoke(this, EventArgs.Empty);
    }

    protected virtual void InvokeOnDie()
    {
        OnDie?.Invoke(this, EventArgs.Empty);
    }

}
