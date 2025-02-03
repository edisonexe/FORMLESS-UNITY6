using UnityEngine;
using Formless.SM;
using Formless.Core.Animations;

namespace Formless.Player.States
{
    public class PlayerHurtState : PlayerState
    {
        private PlayerInputHandler _inputHandler;
        private Animator _animator;
        private float _hurtDuration = 0.5f;
        private float _hurtTimer;
        //private Vector3 _originalScale;

        public PlayerHurtState(Player player, StateMachine stateMachine, PlayerInputHandler inputHandler, Animator animator)
            : base(player, stateMachine)
        {
            _inputHandler = inputHandler;
            _animator = animator;
        }

        public override void Enter()
        {
            //Debug.Log("Enter [HURT]");

             //_originalScale = player.transform.localScale;
            
            // ќпредел€ем направление удара и разворачиваем игрока
            //if (player.LastDamageSource != null) // ѕровер€ем, есть ли источник урона
            //{
            //    float hitDirection = player.LastDamageSource.position.x - player.transform.position.x;
            //    if (hitDirection > 0)
            //        player.transform.localScale = new Vector3(-Mathf.Abs(_originalScale.x), _originalScale.y, _originalScale.z);
            //    else
            //        player.transform.localScale = new Vector3(Mathf.Abs(_originalScale.x), _originalScale.y, _originalScale.z);
            //}


            _animator.SetTrigger(AnimationConstants.HURT);
            _animator.SetBool(AnimationConstants.IS_MOVING, false);
            _hurtTimer = _hurtDuration;
        }
        public override void Update()
        {

            if (player.Health <= 0)
            {
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
            //player.transform.localScale = _originalScale;
            //Debug.Log("Exit [HURT]");
        }
    }
}
