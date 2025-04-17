using UnityEngine;
using Formless.SM;
using Formless.Core.Animations;
using Formless.UI;
using Formless.Core.Managers;
using Formless.Audio;

namespace Formless.Player.States
{
    public class PlayerDeathState : PlayerState
    {
        private Animator _animator;
        public PlayerDeathState(Player player, StateMachine stateMachine, Animator animator)
            : base(player, stateMachine)
        {
            _animator = animator;
        }

        public override void Enter()
        {
            //Debug.Log("Enter [DIE]");
            _animator.SetBool(AnimationConstants.IS_DIE, true);

            player.boxCollider2D.enabled = false;
            player.basicAttackCollider.enabled = false;
            player.strongAttackCollider.enabled = false;
            player.capsuleCollider2D.enabled = false;

            AudioManager.Instance.PlaySound(AudioManager.Instance.plDie);

            GameplayManager.Instance.endPanel.SetupPanel(GameResult.Defeat);

            player.StartFadeAndDestroy();
        }

        public override void Exit()
        {
            //Debug.Log("Exit [DIE]");
        }
    }
}
