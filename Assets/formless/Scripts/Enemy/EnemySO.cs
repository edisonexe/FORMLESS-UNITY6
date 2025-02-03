using UnityEngine;

namespace Formless.Enemy
{
    [CreateAssetMenu(fileName = "EnemySO", menuName = "Scriptable Objects/EnemySO")]
    public class EnemySO : ScriptableObject
    {
        public string enemyName;
        public float maxHealth;
        public float damageBasicAttack;
        public float damageStrongAttack;
        public float patrolDistanceMin;
        public float patrolDistanceMax;
        public float moveSpeed;
        public float detectionRange;
        public float chasingSpeedMultiplier;
    }
}

