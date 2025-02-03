using UnityEngine;
using Formless.SM;
using Formless.Enemy.States;

namespace Formless.Boss.States
{
    public class BossHurtState : HurtState<Boss>
    {
        public BossHurtState(Boss boss, StateMachine stateMachine, Animator animator)
            : base(boss, stateMachine, animator)
        {
        }
    }
}