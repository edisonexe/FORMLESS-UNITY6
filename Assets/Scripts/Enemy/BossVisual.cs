using UnityEngine;

public class BossVisual : EnemyVisual
{
    private Boss _boss;

    public void TriggerStartAttack3()
    {
        _boss.Attack3ColliderTurnOn();
    }

    public void TriggerEndAttack3()
    {
        _boss.Attack3ColliderTurnOff();
    }
}
