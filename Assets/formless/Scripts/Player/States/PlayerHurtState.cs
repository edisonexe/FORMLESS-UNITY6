using UnityEngine;
using Formless.SM;
using Formless.Core.Animations;
using Formless.Audio;

namespace Formless.Player.States
{
    public class PlayerHurtState : PlayerState
    {
        private PlayerInputHandler _inputHandler;
        private Animator _animator;
        private float _hurtDuration = 0.5f;
        private float _hurtTimer;

        public PlayerHurtState(Player player, StateMachine stateMachine, PlayerInputHandler inputHandler, Animator animator)
            : base(player, stateMachine)
        {
            _inputHandler = inputHandler;
            _animator = animator;
        }

        public override void Enter()
        {
            Debug.Log("Enter [HURT]");

            _animator.SetTrigger(AnimationConstants.HURT);
            _animator.SetBool(AnimationConstants.IS_MOVING, false);
            _hurtTimer = _hurtDuration;
            AudioManager.Instance.PlaySound(AudioManager.Instance.plHurt);
        }
        public override void Update()
        {

            if (player.Health <= 0)
            {
                Debug.Log("Переход в из hurt состояние смерти");
                stateMachine.ChangeState(new PlayerDeathState(player, stateMachine, _animator));
                return;
            }

            _hurtTimer -= Time.deltaTime;
            if (_hurtTimer <= 0)
            {
                stateMachine.ChangeState(new PlayerIdleState(player, stateMachine, _inputHandler, _animator));
            }
        }
        public override void Exit()
        {
            //Debug.Log("Exit [HURT]");
        }
    }
}
