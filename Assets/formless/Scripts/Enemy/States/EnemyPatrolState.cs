using UnityEngine;
using Formless.SM;

namespace Formless.Enemy.States
{
    public class EnemyPatrolState : PatrolState<Enemy>
    {
        public EnemyPatrolState(Enemy enemy, StateMachine stateMachine, Animator animator)
            : base(enemy, stateMachine, animator)
        {
        }
    }
}
