using UnityEngine;
using Formless.SM;

namespace Formless.Enemy.States
{
    public class EnemyIdleState : IdleState<Enemy>
    {
        public EnemyIdleState(Enemy enemy, StateMachine stateMachine, Animator animator) 
            : base(enemy, stateMachine, animator) 
        {
        }
    }
}
