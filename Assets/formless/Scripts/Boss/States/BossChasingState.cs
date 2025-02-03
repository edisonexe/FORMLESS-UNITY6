using UnityEngine;
using Formless.SM;

namespace Formless.Boss.States
{
    public class BossChasingState : ChasingState<Boss>
    {
        public BossChasingState(Boss boss, StateMachine stateMachine, Animator animator)
            : base(boss, stateMachine, animator)
        {
        }
    }
}
