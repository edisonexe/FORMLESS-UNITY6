using UnityEngine;
using Formless.Player.States;
using Formless.SM;
using Formless.Player;
using Formless.Core.Animations;

public class PlayerAttackState : PlayerState
{
    private PlayerInputHandler _inputHandler;
    private Animator _animator;

    public PlayerAttackState(Player player, StateMachine stateMachine, PlayerInputHandler inputHandler, Animator animator) 
        : base(player, stateMachine)
    {
        _inputHandler = inputHandler;
        _animator = animator;
    }

    public override void Enter()
    {
        //Debug.Log("Enter [ATTACK]");

        if (_inputHandler.IsBasicAttackPressed())
        {
            _animator.SetTrigger(AnimationConstants.BASIC_ATTACK);
            player.BasicAttack();
        }
        else if (_inputHandler.IsStrongAttackPressed())
        {
            _animator.SetTrigger(AnimationConstants.STRONG_ATTACK);
            player.StrongAttack();
        }
    }

    public override void Update()
        {
            if (_inputHandler.GetMoveInput() != Vector2.zero)
            {
                stateMachine.ChangeState(new PlayerMovingState(player, stateMachine, _inputHandler, _animator));
            }
            else
            {
                stateMachine.ChangeState(new PlayerIdleState(player, stateMachine, _inputHandler, _animator));
            }
        }
    
    public override void Exit()
    {
        //Debug.Log("Exit [ATTACK]");
    }
}
