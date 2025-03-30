using UnityEngine;
using Formless.SM;
using Formless.Core.Animations;

namespace Formless.Enemy.States
{
    public abstract class AttackState<T> : EnemyState where T : Enemy
    {
        protected T entity;
        protected Animator animator;
        protected float attackRange = 1.8f;
        protected float attackCooldown = 1.5f;
        protected float timeSinceLastAttack;

        public AttackState(T entity, StateMachine stateMachine, Animator animator)
            : base(entity, stateMachine)
        {
            this.entity = entity;
            this.animator = animator;
        }

        public override void Enter()
        {
            //Debug.Log($"{typeof(T).Name} enter [ATTACK]");
            animator.SetBool(AnimationConstants.IS_MOVING, false);
            PerformAttack();
            timeSinceLastAttack = 0f;
        }

        public override void Update()
        {
            timeSinceLastAttack += Time.deltaTime;

            if (Player.Player.Instance != null && Player.Player.Instance.gameObject.activeInHierarchy)
            {
                float distanceToPlayer = Vector2.Distance(entity.transform.position, Player.Player.Instance.transform.position);

                if (distanceToPlayer > (entity.rangeAttacking ? entity.rangeAttackRange : attackRange))
                {
                    ChangerState.ChangeToChasingState(entity, stateMachine, animator);
                }
                else
                {
                    entity.LookAtPlayer();

                    if (timeSinceLastAttack >= attackCooldown)
                    {
                        PerformAttack();
                        timeSinceLastAttack = 0f;
                    }
                }
            }
            else
            {
                ChangerState.ChangeToIdleState(entity, stateMachine, animator);
            }
        }

        //public override void Update()
        //{
        //    timeSinceLastAttack += Time.deltaTime;

        //    if (Player.Player.Instance != null) 
        //    {
        //        if (Vector2.Distance(entity.transform.position, Player.Player.Instance.transform.position) > attackRange)
        //        {
        //            ChangerState.ChangeToChasingState(entity, stateMachine, animator);
        //        }
        //        else
        //        {
        //            entity.LookAtPlayer();

        //            if (timeSinceLastAttack >= attackCooldown)
        //            {
        //                PerformAttack();
        //                timeSinceLastAttack = 0f;
        //            }
        //        }  
        //    }
        //}

        public override void Exit()
        {
            //Debug.Log($"{typeof(T).Name} exit [ATTACK]");
        }

        protected abstract void PerformAttack();
    }
}
