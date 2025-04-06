using Formless.Core.Animations;
using Formless.Enemy.States;
using Formless.Enemy;
using Formless.Player;
using UnityEngine;
using Formless.SM;
using Formless.Boss;

public abstract class ChasingState<T> : EnemyState where T : Enemy
{
    protected T entity;
    protected Animator animator;

    public ChasingState(T entity, StateMachine stateMachine, Animator animator)
        : base(entity, stateMachine)
    {
        this.entity = entity;
        this.animator = animator;
    }

    public override void Enter()
    {
        //Debug.Log($"{typeof(T).Name} enter [CHASING]");
        animator.SetBool(AnimationConstants.IS_MOVING, true);
        entity.navMeshAgent.speed = entity.chasingSpeed;
    }

    //public override void Update()
    //{
    //    if (entity.CanSeePlayer())
    //    {
    //        entity.LookAtPlayer();
    //        Vector3 targetPos = GetChaseTargetPosition();
    //        entity.navMeshAgent.SetDestination(targetPos);

    //        float distanceToPlayer = Vector2.Distance(entity.transform.position, Player.Instance.transform.position);

    //        if (entity.rangeAttacking)
    //        {
    //            // Для врагов дальнего боя
    //            if (distanceToPlayer <= entity.attackRange)
    //            {
    //                ChangerState.ChangeToRangedAttackState(entity, stateMachine, animator);
    //            }
    //        }
    //        else
    //        {
    //            // Для врагов ближнего боя
    //            if (distanceToPlayer <= entity.attackRange)
    //            {
    //                ChangerState.ChangeToAttackState(entity, stateMachine, animator);
    //            }
    //        }
    //    }
    //    else
    //    {
    //        // Если игрок вне зоны видимости, переходим в состояние покоя
    //        ChangerState.ChangeToIdleState(entity, stateMachine, animator);
    //    }
    //}


    public override void Update()
    {
        if (entity is Boss boss && boss.CanUseSpecialAttack)
        {
            stateMachine.ChangeState(new BossSpecialAttackState(boss, stateMachine, animator));
            return;
        }

        if (entity.CanSeePlayer())
        {
            entity.LookAtPlayer();
            Vector3 targetPos = GetChaseTargetPosition();
            entity.navMeshAgent.SetDestination(targetPos);

            float distanceToPlayer = Vector3.Distance(entity.transform.position, Player.Instance.transform.position);

            if (distanceToPlayer <= (entity.rangeAttacking ? entity.rangeAttackRange : entity.attackRange))
            {
                ChangerState.ChangeToAttackState(entity, stateMachine, animator);
            }

            //if (Vector2.Distance(entity.transform.position, Player.Instance.transform.position) <= entity.attackRange)
            //{
            //    ChangerState.ChangeToAttackState(entity, stateMachine, animator);
            //}
        }
        else
        {
            ChangerState.ChangeToIdleState(entity, stateMachine, animator);
        }
    }

    public override void Exit()
    {
        //Debug.Log($"{typeof(T).Name} exit [CHASING]");
        entity.navMeshAgent.speed = entity.MovingSpeed;
    }

    private Vector3 GetChaseTargetPosition()
    {
        Vector3 playerPos = Player.Instance.transform.position;
        float offsetX = 0.5f;
        Vector3 targetPos = playerPos;

        if (entity.transform.position.x < playerPos.x)
        {
            targetPos.x -= offsetX;
        }
        else
        {
            targetPos.x += offsetX;
        }

        return targetPos;
    }
}
