using UnityEngine;
using System;

public class Boss : Enemy
{
    [Header("BossSO")]
    [SerializeField] private BossSO _bossSO;

    public PolygonCollider2D ultraAttackPolygonCollider1;
    public PolygonCollider2D ultraAttackPolygonCollider2;

    private int _damageUltraAttack;
    public int maxHealth;
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
        damageBasicAttack = _bossSO.damageAttack1;
        damageStrongAttack = _bossSO.damageAttack2;
        _damageUltraAttack = _bossSO.damageAttack3;
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
