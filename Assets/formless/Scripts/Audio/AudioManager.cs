using UnityEngine;
using UnityEngine.Audio;

namespace Formless.Audio
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance{ get; private set; }

        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _SFXSource;

        [Header("-----------MUSIC------------")]
        public AudioClip gameMusic;
        public AudioClip menuMusic;


        [Header("--------MENU SOUNDS-----------")]
        public AudioClip select;
        public AudioClip back;
        public AudioClip quit;


        [Header("-----------GAME SOUNDS------------")]
        public AudioClip victory;
        public AudioClip openingRegularDoor;
        public AudioClip openingKeyRequiredDoor;
        public AudioClip explosion;
        public AudioClip fuse;
        public AudioClip findItem;

        [Header("---------Player Sounds-----------")]
        public AudioClip basicAttack;
        public AudioClip strongAttack;
        public AudioClip plDie;
        public AudioClip plHurt;
        public AudioClip plUseTeleport;
        public AudioClip plRebirth;

        [Header("---------Enemy Sounds-----------")]
        public AudioClip projectileAttack;
        public AudioClip enAttack;
        public AudioClip enDie;
        public AudioClip enHurt;

        private void Awake()
        {
            Instance = this;
        }

        public void PlayMenuMusic()
        {
            _musicSource.clip = menuMusic;
            _musicSource.Play();
        }

        public void PlayGameMusic()
        {
            _musicSource.clip = gameMusic;
            _musicSource.Play();
        }

        public void PlaySound(AudioClip clip)
        {
            _SFXSource.PlayOneShot(clip);
        }

        public void StopMusic()
        {
            if (_musicSource.isPlaying)
            {
                _musicSource.Stop();
            }
        }

        public void StopSound()
        {
            if (_SFXSource.isPlaying)
            {
                _SFXSource.Stop();
            }
        }

        public void PlaySelectSound()
        {
            PlaySound(select);
        }

        public void PlayBackSound()
        {
            PlaySound(back);
        }

        public void PlayQuitSound()
        {
            PlaySound(quit);
        }
    }
}

