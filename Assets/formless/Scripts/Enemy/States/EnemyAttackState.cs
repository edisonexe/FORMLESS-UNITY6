using UnityEngine;
using Formless.SM;
using Formless.Core.Animations;

namespace Formless.Enemy.States
{
    public class EnemyAttackState : AttackState<Enemy>
    {
        public EnemyAttackState(Enemy entity, StateMachine stateMachine, Animator animator)
            : base(entity, stateMachine, animator) 
        {
        }

        protected override void PerformAttack()
        {
            if (entity.rangeAttacking)
            {
                animator.SetTrigger(AnimationConstants.BASIC_ATTACK);
            }
            else
            {
                float attackChance = Random.Range(0f, 100f);
                if (attackChance <= 65f)
                {
                    animator.SetTrigger(AnimationConstants.BASIC_ATTACK);
                }
                else
                {
                    animator.SetTrigger(AnimationConstants.STRONG_ATTACK);
                }
            }
        }
    }
}
