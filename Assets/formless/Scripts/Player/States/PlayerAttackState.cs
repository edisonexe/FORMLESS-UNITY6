using UnityEngine;
using Formless.Player.States;
using Formless.SM;
using Formless.Player;
using Formless.Core.Animations;

public class PlayerAttackState : PlayerState
{
    private PlayerInputHandler _inputHandler;
    private Animator _animator;
    private bool _isBasicAttack;
    private bool _isStrongAttack;
    public float _attackCooldown = 0.5f;
    private float _attackCooldownTimer;



    public bool IsBasicAttack => _isBasicAttack;
    public bool IsStrongAttack => _isStrongAttack;

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
            _isBasicAttack = true;
        }
        else if (_inputHandler.IsStrongAttackPressed())
        {
            _animator.SetTrigger(AnimationConstants.STRONG_ATTACK);
            _isStrongAttack = true;
        }
    }

    public override void Update()
    {
        Vector2 moveInput = _inputHandler.GetMoveInput();
        if (moveInput.x != 0)
        {
            player.ChangePlayerFacingDirection(moveInput);
        }
        if (moveInput != Vector2.zero)
        {
            player.Move(moveInput);
        }

    }

    public override void Exit()
    {
        _isBasicAttack = false;
        _isStrongAttack = false;
        //Debug.Log("Exit [ATTACK]");
    }
}
