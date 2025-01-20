using UnityEngine;
using UnityEngine.AI;
using Formless.Utils;
using System;
using TMPro;
using Unity.IO.LowLevel.Unsafe;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private State _startState;

    [Header("Roaming")]
    [SerializeField] private float _roamingDistanceMax = 7f;
    [SerializeField] private float _roamingDistanceMin = 5f;
    [SerializeField] private float _roamingTimerMax = 4.5f;
    private float _roamingSpeed;
    private float _roamingTimer;
    private Vector3 _roamingPosition;

    [Header("Chasing")]
    [SerializeField] private bool _isChasing = false;
    [SerializeField] private float _chasingDistance = 4f;
    [SerializeField] private float _chasingSpeedMult = 1.5f;
    private float _chasingSpeed;

    [Header("Attacking")]
    [SerializeField] private bool _isAttackingEnemy = false;
    [SerializeField] private float _attackDistance = 2f;
    [SerializeField] protected float _attackDelay = 3f;
    protected float _nextAttackTime = 0f;

    private NavMeshAgent _navMeshAgent;
    private State _currentState;
    private Vector3 _startPosition;

    private const int COUNT_OF_ATTACKS_STYLE = 2;

    public event EventHandler OnEnemyBasicAttack;
    public event EventHandler OnEnemyStrongAttack;

    public enum State
    {
        Idle,
        Roaming,
        Chasing,
        Attacking,
        Hurt,
        Death
    }

    protected virtual void Awake()
    {
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updateRotation = false;
        _navMeshAgent.updateUpAxis = false;
        _currentState = _startState;
        _roamingSpeed = _navMeshAgent.speed;
        _chasingSpeed = _navMeshAgent.speed * _chasingSpeedMult;
    }

    protected virtual void Update()
    {
        StateHandler();
    }

    protected void InvokeOnEnemyBasicAttack()
    {
        OnEnemyBasicAttack?.Invoke(this, EventArgs.Empty);
    }

    protected void InvokeOnEnemyStrongAttack()
    {
        OnEnemyStrongAttack?.Invoke(this, EventArgs.Empty);
    }


    public void SetDeathState()
    {
        _navMeshAgent.ResetPath();
        _currentState = State.Death;
    }

    public bool IsRoaming()
    {
        if (_navMeshAgent.velocity == Vector3.zero)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public float GetRoamingAnimationSpeed()
    {
        return _navMeshAgent.speed / _roamingSpeed;
    }

    private void StateHandler()
    {
        switch (_currentState)
        {
            case State.Roaming:
                _roamingTimer -= Time.deltaTime;
                if (_roamingTimer < 0)
                {
                    Roaming();
                    _roamingTimer = _roamingTimerMax;
                }
                CheckCurrentState();
                break;
            case State.Chasing:
                ChaseTarget();
                CheckCurrentState();
                break;
            case State.Attacking:
                AttackTarget();
                CheckCurrentState();
                break;
            case State.Hurt:
                break;
            case State.Death:
                break;
            default:
            case State.Idle:
                break;
        }
    }

    private void CheckCurrentState()
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
                if (distanceToPlayer <= _attackDistance)
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

    private void Roaming()
    {
        _startPosition = transform.position;
        _roamingPosition = GetRoamingPosition();
        ChangeFacingDirection(_startPosition, _roamingPosition);
        _navMeshAgent.SetDestination(_roamingPosition);
    }

    private void ChaseTarget()
    {
        // Позиция игрока
        Vector3 playerPosition = Player.Instance.transform.position;

        // Направление взгляда игрока
        Vector3 playerForward = Player.Instance.transform.forward;

        // Векторы смещения влево и вправо
        Vector3 leftOffset = Vector3.Cross(playerForward, Vector3.up).normalized;
        Vector3 rightOffset = -leftOffset;

        // Определяем расстояние для смещения
        float offsetDistance = 0.7f; // Расстояние от игрока
        Vector3 leftTarget = playerPosition + leftOffset * offsetDistance;
        Vector3 rightTarget = playerPosition + rightOffset * offsetDistance;

        // Определяем текущую позицию врага
        Vector3 enemyPosition = transform.position;

        // Сравниваем расстояния от врага до левой и правой позиций
        float distanceToLeft = Vector3.Distance(enemyPosition, leftTarget);
        float distanceToRight = Vector3.Distance(enemyPosition, rightTarget);

        // Выбираем ближнюю позицию
        Vector3 targetPosition = distanceToLeft < distanceToRight ? leftTarget : rightTarget;

        // Проверяем, находится ли враг на одной линии с игроком по высоте
        //float verticalTolerance = 0.1f; // Допустимое отклонение по высоте
        //if (Mathf.Abs(enemyPosition.y - playerPosition.y) > verticalTolerance)
        //{
        //    // Если враг выше или ниже игрока, корректируем высоту целевой позиции
        //    targetPosition.y = playerPosition.y;
        //}

        // Дополнительное смещение по оси Y
        float heightOffset = 0.5f; // Смещение по оси Y
        targetPosition.y = playerPosition.y - heightOffset; // Пониже на heightOffset

        // Устанавливаем целевую позицию для NavMeshAgent
        _navMeshAgent.SetDestination(targetPosition);

        ChangeFacingDirectionToPlayer();
    }

    protected virtual void AttackTarget()
    {
        if (Time.time > _nextAttackTime)
        {
            ChangeFacingDirectionToPlayer();

            int attackType = UnityEngine.Random.Range(1, COUNT_OF_ATTACKS_STYLE + 1);

            switch (attackType)
            {
                default:
                case 1:
                    OnEnemyBasicAttack?.Invoke(this, EventArgs.Empty);
                    break;
                case 2:
                    OnEnemyStrongAttack?.Invoke(this, EventArgs.Empty);
                    break;
            }

            _nextAttackTime = Time.time + _attackDelay;
        }
    }

    private Vector2 GetRoamingPosition()
    {
        return _startPosition + FormlessUtils.GetRandomDirection() * UnityEngine.Random.Range(_roamingDistanceMin, _roamingDistanceMax);
    }

    private void ChangeFacingDirection(Vector3 currentPosition, Vector3 targetPosition)
    {
        if (currentPosition.x > targetPosition.x)
        {
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
    }

    protected void ChangeFacingDirectionToPlayer()
    {
        if (Player.Instance == null) return;

        // Если игрок слева от врага
        if (Player.Instance.transform.position.x < transform.position.x)
        {
            transform.rotation = Quaternion.Euler(0, 180, 0); // Смотрим влево
        }
        else
        {
            transform.rotation = Quaternion.Euler(0, 0, 0); // Смотрим вправо
        }
    }

}
