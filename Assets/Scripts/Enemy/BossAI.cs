using UnityEngine;
using System;
public class BossAI : EnemyAI
{
    private const int BOSS_COUNT_OF_ATTACKS_STYLE = 3;
    [SerializeField] private Boss _boss;
    public event EventHandler OnBossUltraAttack;
    private bool _isSecondPhase = false; // Флаг для второй фазы
    [SerializeField] private float secondPhaseHealthThreshold = 0.5f; // Порог здоровья для второй фазы
    [SerializeField] private float ultraAttackProbability = 0.6f; // Вероятность третьего стиля атаки во второй фазе

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
            Debug.Log("Босс переходит во вторую фазу!");
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
                // Логика для второй фазы: выбираем только стили 2 и 3
                float randomValue = UnityEngine.Random.value; // Случайное значение от 0 до 1
                if (randomValue < ultraAttackProbability)
                {
                    attackType = 3; // С большей вероятностью выбираем третий стиль
                }
                else
                {
                    attackType = 2; // Иначе выбираем второй стиль
                }
            }
            else
            {
                // Логика для первой фазы: выбираем из всех стилей
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
                    Debug.LogWarning("Неизвестный тип атаки: " + attackType);
                    break;
            }

            _nextAttackTime = Time.time + _attackDelay;
        }
    }

}
