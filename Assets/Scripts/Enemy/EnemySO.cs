using UnityEngine;

[CreateAssetMenu()]
public class EnemySO : ScriptableObject
{
    public string enemyName;
    public float enemyHealth;
    public float enemyBasicAttackDamage;
    public float enemyStrongAttackDamage;
}
