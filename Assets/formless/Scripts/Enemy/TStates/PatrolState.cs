using UnityEngine;
using Formless.SM;
using Formless.Core.Animations;

namespace Formless.Enemy.States
{
    public abstract class PatrolState<T> : EnemyState where T : Enemy
    {
        protected T entity;
        protected Animator animator;
        protected float _patrolTimer;
        protected Vector2 _patrolPosition;

        public PatrolState(T entity, StateMachine stateMachine, Animator animator)
            : base(entity, stateMachine)
        {
            this.entity = entity;
            this.animator = animator;
        }

        public override void Enter()
        {
            //Debug.Log($"{typeof(T).Name} enter [PATROL]");
            animator.SetBool(AnimationConstants.IS_MOVING, true);
            _patrolTimer = enemy.patrolTimerMax;

            if (entity.startPosition == Vector2.zero)
            {
                entity.startPosition = enemy.transform.position;
            }

            Patrol();
        }

        public override void Update()
        {
            _patrolTimer -= Time.deltaTime;

            if (_patrolTimer <= 0)
            {
                Patrol();
                _patrolTimer = entity.patrolTimerMax;
            }

            if (!entity.navMeshAgent.pathPending && entity.navMeshAgent.remainingDistance <= entity.navMeshAgent.stoppingDistance)
            {
                // Используем ChangerState для перехода в состояние Idle
                ChangerState.ChangeToIdleState(entity, stateMachine, animator);
            }

            if (entity.CanSeePlayer())
            {
                // Используем ChangerState для перехода в состояние Chasing
                ChangerState.ChangeToAttackState(entity, stateMachine, animator);  // Пример для атаки
            }
        }

        public override void Exit()
        {
            //Debug.Log($"{typeof(T).Name} exit [PATROL]");
        }

        protected void Patrol()
        {
            entity.startPosition = entity.transform.position;
            _patrolPosition = entity.GetRandomPatrolPosition();
            entity.ChangeFacingDirection(entity.startPosition, _patrolPosition);
            entity.navMeshAgent.SetDestination(_patrolPosition);
        }
    }
}
