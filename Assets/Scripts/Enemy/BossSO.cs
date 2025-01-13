using UnityEngine;

[CreateAssetMenu(fileName = "BossSO", menuName = "Scriptable Objects/BossSO")]
public class BossSO : ScriptableObject
{
    public string bossName;
    public int health;
    public int damageAttack1;
    public int damageAttack2;
    public int damageAttack3;
}
