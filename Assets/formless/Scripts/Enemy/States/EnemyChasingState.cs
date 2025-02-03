using UnityEngine;
using Formless.SM;

namespace Formless.Enemy.States
{
    public class EnemyChasingState : ChasingState<Enemy>
    {
        public EnemyChasingState(Enemy enemy, StateMachine stateMachine, Animator animator)
            : base(enemy, stateMachine, animator) { }
    }
    }

