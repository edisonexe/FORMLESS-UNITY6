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
            Debug.Log("Enter [DIE]");
            _animator.SetBool(AnimationConstants.IS_DIE, true);

            player.boxCollider2D.enabled = false;
            player.basicAttackCollider.enabled = false;
            player.strongAttackCollider.enabled = false;
            player.capsuleCollider2D.enabled = false;

            AudioManager.Instance.PlaySound(AudioManager.Instance.plDie);

            //if (UIManager.Instance == null)
            //{
            //    Debug.LogError("[PLAYER_DEATH] UIManager.Instance is NULL!");
            //    return;
            //}
    
            //if (UIManager.Instance.endPanel == null)
            //{
            //    Debug.LogError("[PLAYER_DEATH] UIManager.Instance.EndPanel is NULL!");
            //    return;
            //}

            GameplayManager.Instance.endPanel.SetupPanel(GameResult.Defeat);

            player.StartFadeAndDestroy();
        }

        public override void Update()
        {
            // После завершения анимации смерти можно либо перезагрузить сцену, либо сделать что-то другое.
            // Например, после завершения анимации переходить в главное меню или перезагружать игру.
        }

        public override void Exit()
        {
            //Debug.Log("Exit [DIE]");
        }
    }
}
