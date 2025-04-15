using UnityEngine;
using UnityEngine.Audio;

namespace Formless.Audio
{
    public class MenuAudioManager : MonoBehaviour
    {
        public static MenuAudioManager Instance{ get; private set; }

        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _SFXSource;

        [Header("-----------MUSIC------------")]
        public AudioClip menuMusic;


        [Header("--------MENU SOUNDS-----------")]
        public AudioClip select;
        public AudioClip back;
        public AudioClip quit;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _musicSource.clip = menuMusic;
            _musicSource.Play();
        }

        public void PlaySound(AudioClip clip)
        {
            _SFXSource.PlayOneShot(clip);
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

