using UnityEngine;
using Formless.SM;
using Formless.Core.Animations;
using Formless.Audio;

namespace Formless.Enemy.States
{
    public abstract class HurtState<T> : EnemyState where T: Enemy
    {
        protected T entity;
        protected Animator animator;
        protected float _hurtDuration = 0.5f;
        protected float _hurtTimer;

        public HurtState(T entity, StateMachine stateMachine, Animator animator)
            : base(entity, stateMachine)
        {
            this.entity = entity;
            this.animator = animator;
        }

        public override void Enter()
        {
            //Debug.Log($"{typeof(T).Name} enter [HURT]");
            animator.SetTrigger(AnimationConstants.HURT);
            animator.SetBool(AnimationConstants.IS_MOVING, false);
            entity.navMeshAgent.isStopped = true;
            _hurtTimer = _hurtDuration;
            AudioManager.Instance.PlaySound(AudioManager.Instance.enHurt);
        }

        public override void Update()
        {
            if (entity.Health <= 0)
            {
                ChangerState.ChangeToDeathState(entity, stateMachine, animator);
                return;
            }

            _hurtTimer -= Time.deltaTime;
            if (_hurtTimer <= 0)
            {
                ChangerState.ChangeToChasingState(entity, stateMachine, animator);
            }
        }

        public override void Exit()
        {
            entity.navMeshAgent.isStopped = false;
            //Debug.Log($"{typeof(T).Name} exit [HURT]");
        }
    }
}
