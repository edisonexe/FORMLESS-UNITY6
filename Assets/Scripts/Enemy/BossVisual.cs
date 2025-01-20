using UnityEngine;

public class BossVisual : EnemyVisual
{
    [SerializeField] private BossAI _bossAI;
    [SerializeField] private Boss _boss;
    private const string ULTRA_ATTACK = "UltraAttack";

    protected override void Start()
    {
        base.Start();
        _bossAI.OnBossUltraAttack += bossAI_OnBossUltraAttack;
    }


    protected override void Update()
    {
        base.Update();
    }

    private void bossAI_OnBossUltraAttack(object sender, System.EventArgs e)
    {
        _animator.SetTrigger(ULTRA_ATTACK);
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
