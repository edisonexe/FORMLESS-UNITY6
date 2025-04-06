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
            //if (entity.Health <= entity.MaxHealth * 0.3f)
            //{
            //    Debug.Log($"Здоровье у босса менее 30%");
            //    ChangerState.ChangeToSpecialAttackState(entity, stateMachine, animator);
            //    return;
            //}


            float attackChance = Random.Range(0f, 100f);
            if (attackChance <= 50f)
            {
                animator.SetTrigger(AnimationConstants.BASIC_ATTACK);
            }
            else if (attackChance <= 85f)
            {
                animator.SetTrigger(AnimationConstants.STRONG_ATTACK);
            }
        }
    }
}
