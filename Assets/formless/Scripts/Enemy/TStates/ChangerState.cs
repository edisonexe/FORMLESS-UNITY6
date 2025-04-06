using UnityEngine;
using Formless.SM;
using Formless.Boss.States;

namespace Formless.Enemy.States
{
    public static class ChangerState
    {
        public static void ChangeToIdleState(Enemy enemy, StateMachine stateMachine, Animator animator)
        {
            if (enemy is Boss.Boss boss)
            {
                stateMachine.ChangeState(new BossIdleState(boss, stateMachine, animator));  // Приведение типа
            }
            else
            {
                stateMachine.ChangeState(new EnemyIdleState(enemy, stateMachine, animator));
            }
        }

        public static void ChangeToPatrolState(Enemy enemy, StateMachine stateMachine, Animator animator)
        {
            if (enemy is Boss.Boss boss)
            {
                stateMachine.ChangeState(new BossPatrolState(boss, stateMachine, animator));
            }
            else
            {
                stateMachine.ChangeState(new EnemyPatrolState(enemy, stateMachine, animator));
            }
        }

        public static void ChangeToAttackState(Enemy enemy, StateMachine stateMachine, Animator animator)
        {
            if (enemy is Boss.Boss boss)
            {
                stateMachine.ChangeState(new BossAttackState(boss, stateMachine, animator));
            }
            else
            {
                stateMachine.ChangeState(new EnemyAttackState(enemy, stateMachine, animator));
            }
        }

        public static void ChangeToChasingState(Enemy enemy, StateMachine stateMachine, Animator animator)
        {
            if (enemy is Boss.Boss boss)
            {
                stateMachine.ChangeState(new BossChasingState(boss, stateMachine, animator));
            }
            else
            {
                stateMachine.ChangeState(new EnemyChasingState(enemy, stateMachine, animator));
            }
        }

        public static void ChangeToHurtState(Enemy enemy, StateMachine stateMachine, Animator animator)
        {
            if (enemy is Boss.Boss boss)
            {
                stateMachine.ChangeState(new BossHurtState(boss, stateMachine, animator));
            }
            else
            {
                stateMachine.ChangeState(new EnemyHurtState(enemy, stateMachine, animator));
            }
        }

        public static void ChangeToDeathState(Enemy enemy, StateMachine stateMachine, Animator animator)
        {
            if (enemy is Boss.Boss boss)
            {
                stateMachine.ChangeState(new BossDeathState(boss, stateMachine, animator));
            }
            else
            {
                stateMachine.ChangeState(new EnemyDeathState(enemy, stateMachine, animator));
            }
        }

        public static void ChangeToSpecialAttackState(Enemy enemy, StateMachine stateMachine, Animator animator)
        {
            if (enemy is Boss.Boss boss)
            {
                stateMachine.ChangeState(new BossSpecialAttackState(boss, stateMachine, animator));
            }
        }
    }
}
