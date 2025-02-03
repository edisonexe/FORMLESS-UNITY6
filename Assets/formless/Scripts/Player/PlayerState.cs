using Formless.SM;

namespace Formless.Player.States
{
    public abstract class PlayerState : State
    {
        protected Player player;

        public PlayerState(Player player, StateMachine stateMachine) : base(stateMachine)
        {
            this.player = player;
        }
    }
}

