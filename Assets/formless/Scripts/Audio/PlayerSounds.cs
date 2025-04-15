using Formless.Audio;
using UnityEngine;

public class PlayerSounds : MonoBehaviour
{
    public void PlayBasicAttackSound()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.basicAttack);
    }

    public void PlayStrongAttackSound()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.strongAttack);
    }

    public void PlayAttackSound()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.enAttack);
    }

    public void PlayProjectileSound()
    {
        AudioManager.Instance.PlaySound(AudioManager.Instance.projectileAttack);
    }
}
