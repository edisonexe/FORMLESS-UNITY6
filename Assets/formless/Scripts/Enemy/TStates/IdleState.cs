using UnityEngine;
using Formless.SM;
using Formless.Core.Animations;

namespace Formless.Enemy.States
{
    public abstract class IdleState<T> : EnemyState where T: Enemy
    {
        protected T entity;
        protected Animator animator;
        protected float idleTimer;
        protected static readonly float idleDuration = 1f;

        public IdleState(T entity, StateMachine stateMachine, Animator animator) 
            : base(entity, stateMachine) 
        {
            this.entity = entity;
            this.animator = animator;
        }

        public override void Enter()
        {
            //Debug.Log($"{typeof(T).Name} enter [IDLE]");
            animator.SetBool(AnimationConstants.IS_MOVING, false);
            idleTimer = idleDuration;
        }

        public override void Update()
        {
            idleTimer -= Time.deltaTime;
            
            if (entity.CanSeePlayer())
            {
                ChangerState.ChangeToChasingState(entity, stateMachine, animator);
                return;
            }

            if (idleTimer <= 0)
            {
                ChangerState.ChangeToPatrolState(entity, stateMachine, animator);
            }
        }

        public override void Exit()
        {
            //Debug.Log($"{typeof(T).Name} exit [IDLE]");
        }
    }
}
