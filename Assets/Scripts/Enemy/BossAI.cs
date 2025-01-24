using UnityEngine;
using System;
using System.Collections;
using UnityEngine.AI;
public class BossAI : EnemyAI
{
    private const int BOSS_COUNT_OF_ATTACKS_STYLE = 3;
    [SerializeField] private Boss _boss;
    [SerializeField] private BossVisual _bossVisual;
    private bool _isSecondPhase = false; // ���� ��� ������ ����
    [SerializeField] private float secondPhaseHealthThreshold = 0.5f; // ����� �������� ��� ������ ����
    private Coroutine _ultraAttackCoroutine;
    private bool isUltraAttackInProgress = false;
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

    public override void SetDeathState()
    {
        base.SetDeathState();
    }

    protected override void CheckCurrentState()
    {
        if (Player.Instance != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, Player.Instance.transform.position);
            State newState = State.Roaming;

            if (_isChasing)
            {
                if (distanceToPlayer <= _chasingDistance)
                {
                    newState = State.Chasing;
                }
            }

            if (_isAttackingEnemy)
            {
                if (_isSecondPhase)
                {
                    newState = State.Attacking;
                    _navMeshAgent.speed = _chasingSpeed;
                }
                else if (distanceToPlayer <= _attackDistance)
                {
                    newState = State.Attacking;
                }
            }

            if (newState != _currentState)
            {
                if (newState == State.Chasing)
                {
                    _navMeshAgent.ResetPath();
                    _navMeshAgent.speed = _chasingSpeed;
                }
                else if (newState == State.Roaming)
                {
                    _roamingTimer = 0f;
                    _navMeshAgent.speed = _roamingSpeed;
                }
                else if (newState == State.Attacking)
                {
                    _navMeshAgent.ResetPath();
                }
                _currentState = newState;
            }
        }
        else
        {
            _currentState = State.Idle;
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
                attackType = 3;

            }
            else
            {
                attackType = UnityEngine.Random.Range(1, BOSS_COUNT_OF_ATTACKS_STYLE);
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
                    if (_ultraAttackCoroutine != null)
                    {
                        StopCoroutine(_ultraAttackCoroutine);
                    }
                    _ultraAttackCoroutine = StartCoroutine(BossMoveDuringUltraAttack());
                    break;
                default:
                    Debug.LogWarning("����������� ��� �����: " + attackType);
                    break;
            }

            _nextAttackTime = Time.time + _attackDelay;
        }
    }


    private IEnumerator BossMoveDuringUltraAttack()
    {
        // ���������, ���������� �� �����
        if (Player.Instance == null)
        {
            Debug.LogError("Player.Instance �� ������.");
            yield break;
        }

        // ���� �������� ��� �����������, �������
        if (isUltraAttackInProgress)
            yield break;

        // ������������� ����, ��� �������� ��������
        isUltraAttackInProgress = true;

        // ��������� ������� ������
        Vector3 playerPosition = Player.Instance.transform.position;

        // ����������� �������� ����� ����� ������
        Vector3 playerForward = Player.Instance.transform.forward.normalized;

        // �������� ��� ������ � ����� ��������
        Vector3 startOffset = -playerForward * 2f; // ������� ����� �������
        Vector3 endOffset = playerForward * 4f;   // ������� �� �������

        // �������� ����� ��� ��������
        Vector3 _ultraAttackTargetPosition = playerPosition + endOffset;

        // �������� �������� ������-����� (�������� ��� � ��������)
        _bossVisual.StartUltraAttackAnimation();

        // ��������� � �������� �������
        _navMeshAgent.SetDestination(_ultraAttackTargetPosition);

        float maxDuration = 2f; // ������������ ������������ �����
        float elapsedTime = 0f;

        // �������� �� ������� ������ ����������
        while (elapsedTime < maxDuration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ��������� ������-�����
        _bossVisual.StopUltraAttackAnimation();

        // ���������� ����, �������� ���������
        isUltraAttackInProgress = false;
    }

}
