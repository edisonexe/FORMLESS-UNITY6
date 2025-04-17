using Formless.Audio;
using UnityEngine;

public class EnemySounds : MonoBehaviour
{
    public void PlayAttackSound()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.enAttack);
    }

    public void PlayProjectileSound()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.projectileAttack);
    }

    public void PlaySpecAttackSound()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.enSpecAttack);
    }

}
