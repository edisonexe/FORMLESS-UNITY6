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
        //_bossAI.OnBossUltraAttack += bossAI_OnBossUltraAttack;
    }


    protected override void Update()
    {
        base.Update();
    }

    //private void bossAI_OnBossUltraAttack(object sender, System.EventArgs e)
    //{
    //    _animator.SetBool(IS_ULTRA_ATTACK);
    //}

    public void StartUltraAttackAnimation()
    {
        _animator.SetBool(IS_ULTRA_ATTACK, true);
        Debug.Log("True");
    }

    public void StopUltraAttackAnimation()
    {
        _animator.SetBool(IS_ULTRA_ATTACK, false);
        Debug.Log("False");
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
}
