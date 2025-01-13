using UnityEngine;
using System;

public class Boss : Enemy
{
    [Header("BossSO")]
    [SerializeField] private BossSO _bossSO;

    [Header("Phase health settings")]
    [SerializeField] private float phaseChangeThreshold = 0.5f;
    private bool _isInSecondPhase = false;
    public event EventHandler OnPhaseChange;

    public PolygonCollider2D attack3PolygonCollider;

    private int _damageAttack3;

    private bool _isAttack3 = false;

    public override void Awake()
    {
        base.Awake();
        var attack3Object = transform.Find("Attack3Collider");
        attack3PolygonCollider = attack3Object.GetComponent<PolygonCollider2D>();

    }

    private void Start()
    {
        _currentHealth = _bossSO.health;
        _damageAttack1 = _bossSO.damageAttack1;
        _damageAttack2 = _bossSO.damageAttack2;
        _damageAttack3 = _bossSO.damageAttack3;
        Attack1ColliderTurnOff();
        Attack2ColliderTurnOff();
        Attack3ColliderTurnOff();
    }

    public void Attack3ColliderTurnOn()
    {
        _isAttack3 = true;
        _attack1PolygonCollider.enabled = true;
    }

    public void Attack3ColliderTurnOff()
    {
        _attack1PolygonCollider.enabled = false;
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.TryGetComponent(out Player player))
        {
            if (_isAttack3)
            {
                player.TakeDamage(transform, _damageAttack3);
                _isAttack3 = false;
            }
        }
    }
}
