using UnityEngine;
using Formless.SM;
using Formless.Core.Animations;

namespace Formless.Player.States
{
    public class PlayerMovingState : PlayerState
    {
        private PlayerInputHandler _inputHandler;
        private Vector2 _moveInput;
        private Animator _animator;

        public PlayerMovingState(Player player, StateMachine stateMachine, PlayerInputHandler inputHandler, Animator animator)
            : base(player, stateMachine)
        {
            _inputHandler = inputHandler;
            _animator = animator;
        }

        public override void Enter()
        {
            _animator.SetBool(AnimationConstants.IS_MOVING, true);
        }

        public override void Update()
        {
            _moveInput = _inputHandler.GetMoveInput();

            //if (stateMachine.CurrentState is PlayerAttackState)
            //{
            //    return;
            //}

            if (_moveInput.x != 0) 
            {
                player.ChangePlayerFacingDirection(_moveInput);
            }

            player.Move(_moveInput);

            if (_moveInput == Vector2.zero)
            {
                stateMachine.ChangeState(new PlayerIdleState(player, stateMachine, _inputHandler, _animator));
            }

            if (_inputHandler.IsBasicAttackPressed() || _inputHandler.IsStrongAttackPressed() )
            {
                stateMachine.ChangeState(new PlayerAttackState(player, stateMachine, _inputHandler, _animator));
            }
        }

        public override void FixedUpdate()
        {
            player.Move(_moveInput);
        }
    }
}
