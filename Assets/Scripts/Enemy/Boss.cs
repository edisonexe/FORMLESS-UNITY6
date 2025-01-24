using UnityEngine;
using System;

public class Boss : Enemy
{
    [Header("BossSO")]
    [SerializeField] private BossSO _bossSO;
    [SerializeField] private BossAI _bossAI;
    public PolygonCollider2D ultraAttackPolygonCollider1;
    public PolygonCollider2D ultraAttackPolygonCollider2;

    private float _damageUltraAttack;
    public float maxHealth;
    private bool _isUltraAttack = false;

    public override void Awake()
    {
        base.Awake();
        var ultraAttackObject = transform.Find("UltraAttack");

        var ultraAttackObject1 = ultraAttackObject.Find("UltraAttack1");
        var ultraAttackObject2 = ultraAttackObject.Find("UltraAttack2");

        ultraAttackPolygonCollider1 = ultraAttackObject1.GetComponent<PolygonCollider2D>();
        ultraAttackPolygonCollider2 = ultraAttackObject2.GetComponent<PolygonCollider2D>();

    }

    private void Start()
    {
        currentHealth = _bossSO.health;
        maxHealth = _bossSO.health;
        UIManager.Instance.bossHealth = currentHealth;
        UIManager.Instance.bossMaxHealth = maxHealth;
        UIManager.Instance.EnableBossHealthBar();
        damageBasicAttack = _bossSO.damageBasicAttack;
        damageStrongAttack = _bossSO.damageStrongAttack;
        _damageUltraAttack = _bossSO.damageUltraAttack;
        BasicAttackColliderDisable();
        StrongAttackColliderDisable();
        UltraAttackCollider1Disable();
        UltraAttackCollider2Disable();
    }

    public void UltraAttackCollider1Enable()
    {
        _isUltraAttack = true;
        ultraAttackPolygonCollider1.enabled = true;
    }

    public void UltraAttackCollider1Disable()
    {
        ultraAttackPolygonCollider1.enabled = false;
    }

    public void UltraAttackCollider2Enable()
    {
        _isUltraAttack = true;
        ultraAttackPolygonCollider2.enabled = true;
    }

    public void UltraAttackCollider2Disable()
    {
        ultraAttackPolygonCollider2.enabled = false;
    }

    public override void TakeDamage(float damage)
    {
        currentHealth -= damage;
        if (currentHealth > 0)
        {
            //currentHealth -= damage;
            Debug.Log(currentHealth);
            UIManager.Instance.bossHealth = currentHealth;
            UIManager.Instance.UpdateBossHealthtBar();
            InvokeOnTakeHit();
        }
        else
        {
            Debug.Log("Смерть босса");
            UIManager.Instance.bossHealth = currentHealth;
            UIManager.Instance.UpdateBossHealthtBar();
            InvokeOnTakeHit();
            _isDead = true;
        }
    }

    public override void HandleDeath()
    {
        if (!_isDead) return;

        _boxCollider2D.enabled = false;
        basicAttackPolygonCollider.enabled = false;
        strongAttackPolygonCollider.enabled = false;
        UltraAttackCollider1Disable();
        UltraAttackCollider2Disable();
        capsuleCollider2D.enabled = false;

        InvokeOnDie();

        _bossAI.SetDeathState();
        UIManager.Instance.DisableBossHealthBar();
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.TryGetComponent(out Player player))
        {
            if (_isUltraAttack)
            {
                player.TakeDamage(transform, _damageUltraAttack);
                _isUltraAttack = false;
            }
        }
    }
}
