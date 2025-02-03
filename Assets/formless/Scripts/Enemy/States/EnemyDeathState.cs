using UnityEngine;
using Formless.SM;
using System;

namespace Formless.Enemy.States
{
    public class EnemyDeathState : DeathState<Enemy>
    {
        public EnemyDeathState(Enemy enemy, StateMachine stateMachine, Animator animator)
            : base(enemy, stateMachine, animator)
        {
        }
    }
}
