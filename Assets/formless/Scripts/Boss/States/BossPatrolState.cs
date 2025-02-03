using UnityEngine;
using Formless.SM;
using Formless.Enemy.States;

namespace Formless.Boss.States
{
    public class BossPatrolState : PatrolState<Boss>
    {
        public BossPatrolState(Boss boss, StateMachine stateMachine, Animator animator)
            : base(boss, stateMachine, animator)
        {
        }
    }
}

