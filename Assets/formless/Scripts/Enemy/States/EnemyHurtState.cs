using UnityEngine;
using Formless.SM;

namespace Formless.Enemy.States
{
    public class EnemyHurtState : HurtState<Enemy>
    {

        public EnemyHurtState(Enemy enemy, StateMachine stateMachine, Animator animator)
            : base(enemy, stateMachine, animator)
        {
        }
    }
}
