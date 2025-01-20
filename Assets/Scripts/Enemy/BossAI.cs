using UnityEngine;
using System;
public class BossAI : EnemyAI
{
    private const int BOSS_COUNT_OF_ATTACKS_STYLE = 3;
    [SerializeField] private Boss _boss;
    public event EventHandler OnBossUltraAttack;
    private bool _isSecondPhase = false; // ���� ��� ������ ����
    [SerializeField] private float secondPhaseHealthThreshold = 0.5f; // ����� �������� ��� ������ ����
    [SerializeField] private float ultraAttackProbability = 0.6f; // ����������� �������� ����� ����� �� ������ ����

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();
        CheckPhaseChange();
    }

    private void CheckPhaseChange()
    {
        if (!_isSecondPhase && _boss.CurrentHealth <= _boss.maxHealth * secondPhaseHealthThreshold)
        {
            _isSecondPhase = true;
            Debug.Log("���� ��������� �� ������ ����!");
        }
    }


    protected override void AttackTarget()
    {
        if (Time.time > _nextAttackTime)
        {
            ChangeFacingDirectionToPlayer();

            int attackType;

            if (_isSecondPhase)
            {
                // ������ ��� ������ ����: �������� ������ ����� 2 � 3
                float randomValue = UnityEngine.Random.value; // ��������� �������� �� 0 �� 1
                if (randomValue < ultraAttackProbability)
                {
                    attackType = 3; // � ������� ������������ �������� ������ �����
                }
                else
                {
                    attackType = 2; // ����� �������� ������ �����
                }
            }
            else
            {
                // ������ ��� ������ ����: �������� �� ���� ������
                attackType = UnityEngine.Random.Range(1, BOSS_COUNT_OF_ATTACKS_STYLE + 1);
            }

            switch (attackType)
            {
                case 1:
                    InvokeOnEnemyBasicAttack();
                    break;
                case 2:
                    InvokeOnEnemyStrongAttack();
                    break;
                case 3:
                    OnBossUltraAttack?.Invoke(this, EventArgs.Empty);
                    break;
                default:
                    Debug.LogWarning("����������� ��� �����: " + attackType);
                    break;
            }

            _nextAttackTime = Time.time + _attackDelay;
        }
    }

}
