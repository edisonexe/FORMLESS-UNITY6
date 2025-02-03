using UnityEngine;
using Formless.SM;
using Formless.Enemy.States;
using System;

namespace Formless.Boss.States
{
    public class BossDeathState : DeathState<Boss>
    {
        public BossDeathState(Boss boss, StateMachine stateMachine, Animator animator)
            : base(boss, stateMachine, animator)
        {
        }
    }
}