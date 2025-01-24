using System;
using UnityEngine;

public class BossVisual : EnemyVisual
{
    [SerializeField] private BossAI _bossAI;
    [SerializeField] private Boss _boss;
    private const string IS_ULTRA_ATTACK = "IsUltraAttack";

    protected override void Start()
    {
        base.Start();
    }


    protected override void Update()
    {
        base.Update();
    }


    public void StartUltraAttackAnimation()
    {
        _animator.SetBool(IS_ULTRA_ATTACK, true);
    }

    public void StopUltraAttackAnimation()
    {
        _animator.SetBool(IS_ULTRA_ATTACK, false);
    }

    public void TriggerStartUltraAttack1()
    {
        _boss.UltraAttackCollider1Enable();
    }

    public void TriggerEndUltraAttack1()
    {
        _boss.UltraAttackCollider1Disable();
    }

    public void TriggerStartUltraAttack2()
    {
        _boss.UltraAttackCollider2Enable();
    }

    public void TriggerEndUltraAttack2()
    {
        _boss.UltraAttackCollider2Disable();
    }

    public override void TriggerHandleDeath()
    {
        _boss.HandleDeath();
    }
}
