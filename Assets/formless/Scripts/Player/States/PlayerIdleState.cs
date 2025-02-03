using UnityEngine;
using Formless.SM;
using Formless.Core.Animations;

namespace Formless.Player.States
{
    public class PlayerIdleState : PlayerState
    {
        private PlayerInputHandler _inputHandler;
        private Animator _animator;

        public PlayerIdleState(Player player, StateMachine stateMachine, PlayerInputHandler inputHandler, Animator animator)
            : base(player, stateMachine)
        {
            _inputHandler = inputHandler;
            _animator = animator;
        }

        public override void Enter()
        {
            //Debug.Log("Enter [IDLE]");
            _animator.SetBool(AnimationConstants.IS_MOVING, false);
        }

        public override void Update()
        {

            if (_inputHandler.GetMoveInput() != Vector2.zero)
            {
                stateMachine.ChangeState(new PlayerMovingState(player, stateMachine, _inputHandler, _animator));
            }

            else if (_inputHandler.IsBasicAttackPressed() || _inputHandler.IsStrongAttackPressed())
            {
                stateMachine.ChangeState(new PlayerAttackState(player, stateMachine, _inputHandler, _animator));
            }
        }

         public override void Exit()
         {
            //Debug.Log("Exit [IDLE]");
         }
    }
}


