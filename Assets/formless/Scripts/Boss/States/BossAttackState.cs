using UnityEngine;
using Formless.SM;
using Formless.Core.Animations;
using Formless.Enemy.States;

namespace Formless.Boss.States
{
    public class BossAttackState : AttackState<Boss>
    {
        public BossAttackState(Boss entity, StateMachine stateMachine, Animator animator)
            : base(entity, stateMachine, animator) {}

        protected override void PerformAttack()
        {
            float attackChance = Random.Range(0f, 100f);
            if (attackChance <= 50f)
            {
                animator.SetTrigger(AnimationConstants.BASIC_ATTACK);
            }
            else if (attackChance <= 85f)
            {
                animator.SetTrigger(AnimationConstants.STRONG_ATTACK);
            }
            else
            {
                animator.SetTrigger(AnimationConstants.SPECIAL_ATTACK);
            }
        }
    }
}
