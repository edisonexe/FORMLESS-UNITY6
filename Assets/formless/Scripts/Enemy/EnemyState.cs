using Formless.SM;

namespace Formless.Enemy.States
{
    public abstract class EnemyState : State
    {
        protected Enemy enemy;

        public EnemyState(Enemy enemy, StateMachine stateMachine)
            : base(stateMachine)
        {
            this.enemy = enemy;
        }
    }
}
