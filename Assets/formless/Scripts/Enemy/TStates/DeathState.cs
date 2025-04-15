using UnityEngine;
using Formless.SM;
using Formless.Core.Animations;
using System;
using Formless.Audio;

namespace Formless.Enemy.States
{
    public class DeathState<T> : EnemyState where T : Enemy
    {
        protected Animator animator;
        protected T entity;

        public DeathState(T entity, StateMachine stateMachine, Animator animator)
            : base(entity, stateMachine)
        {
            this.entity = entity;
            this.animator = animator;
        }

        public override void Enter()
        {
            Debug.Log($"{typeof(T).Name} enter [DEATH]");
            animator.SetBool(AnimationConstants.IS_DIE, true);

            entity.boxCollider2D.enabled = false;
            entity.basicAttackCollider.enabled = false;
            entity.strongAttackCollider.enabled = false;
            entity.capsuleCollider2D.enabled = false;
            entity.navMeshAgent.ResetPath();
            AudioManager.Instance.PlaySound(AudioManager.Instance.enDie);
            entity.StartFadeAndDestroy();
        }

        public override void Exit()
        {
            //Debug.Log($"{typeof(T).Name} exit [DEATH]");
        }
    }
}