using UnityEngine;

[CreateAssetMenu(fileName = "BossSO", menuName = "Scriptable Objects/BossSO")]
public class BossSO : ScriptableObject
{
    public string bossName;
    public float health;
    public float damageBasicAttack;
    public float damageStrongAttack;
    public float damageUltraAttack;
}
