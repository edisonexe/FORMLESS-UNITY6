using UnityEngine;

namespace Formless.Boss
{
    [CreateAssetMenu(fileName = "BossSO", menuName = "Scriptable Objects/BossSO")]
    public class BossSO : ScriptableObject
    {
        public string bossName;
        public float maxHealth;

        public float damageBasicAttack;
        public float damageStrongAttack;
        public float damageSpecialAttack;

        public float patrolDistanceMin;
        public float patrolDistanceMax;
        public float moveSpeed;
        public float detectionRange;
        public float chasingSpeedMultiplier;
    }
}