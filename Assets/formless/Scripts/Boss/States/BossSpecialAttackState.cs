using Formless.Boss;
using Formless.Core.Animations;
using Formless.Enemy.States;
using Formless.Player;
using UnityEngine;
using Formless.SM;
using Formless.Core;
public class BossSpecialAttackState : EnemyState
{
    private Boss boss;
    private Animator animator;
    private Vector3 targetPosition;
    private float moveSpeed = 2f;
    private float attackDuration = 5f;
    private float timeSinceAttackStarted;
    private bool isMoving;

    public BossSpecialAttackState(Boss boss, StateMachine stateMachine, Animator animator)
        : base(boss, stateMachine)
    {
        this.boss = boss;
        this.animator = animator;
    }

    public override void Enter()
    {
        boss.LookAtPlayer();
        boss.navMeshAgent.ResetPath();
        // Запоминаем где был игрок
        targetPosition = Player.Instance.transform.position;

        animator.SetBool(AnimationConstants.SPECIAL_ATTACK, true);

        isMoving = true;
        timeSinceAttackStarted = 0f;
    }

    public override void Update()
    {
        timeSinceAttackStarted += Time.deltaTime;

        if (isMoving)
        {
            Vector3 dir = (targetPosition - boss.transform.position).normalized;
            boss.transform.position += dir * moveSpeed * Time.deltaTime;

            // Проверка достиг ли цели
            if (Vector3.Distance(boss.transform.position, targetPosition) < 0.2f)
            {
                isMoving = false;
                animator.SetBool(AnimationConstants.SPECIAL_ATTACK, false);
                boss.transform.position = targetPosition;
            }
        }
        else
        {
            Debug.Log("Враг должен стоять на месте");
        }
        // Когда закончилось время атаки — смена состояния
        if (timeSinceAttackStarted >= attackDuration)
        {
            ChangerState.ChangeToIdleState(boss, stateMachine, animator);
        }
    }

    public override void Exit()
    {
        isMoving = false;
    }
}
