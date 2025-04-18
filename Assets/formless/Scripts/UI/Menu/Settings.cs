using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Formless.UI.Menu
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] private GameObject _settingsPanel;
        public bool isFullScreen;

        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;

        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;

        Resolution[] rsl;
        List<string> resolutions;
        public Dropdown dropdown;
        private bool _isLoadingSettings = true;

        private void Awake()
        {
            LoadSettings();

            resolutions = new List<string>();
            rsl = Screen.resolutions;

            int currentResolutionIndex = 0;

            for (int i = 0; i < rsl.Length; i++)
            {
                int refreshRate = (int)rsl[i].refreshRateRatio.numerator; // Новый способ получения герцовки

                if (refreshRate > 1000) 
                {
                    refreshRate /= 1000;
                }

                string resolutionString = rsl[i].width + "x" + rsl[i].height + "  " + refreshRate + "Hz";
                resolutions.Add(resolutionString);

                // Определяем индекс текущего разрешения с учетом частоты обновления
                if (Screen.width == rsl[i].width && Screen.height == rsl[i].height && Screen.currentResolution.refreshRateRatio.numerator == refreshRate)
                {
                    currentResolutionIndex = i;
                }
            }

            dropdown.ClearOptions();
            dropdown.AddOptions(resolutions);

            _isLoadingSettings = true;
            dropdown.value = currentResolutionIndex;
            dropdown.RefreshShownValue();
            _isLoadingSettings = false;

            dropdown.onValueChanged.AddListener(SetResolution);

            SetupSliders();
        }


        public void SetResolution(int r)
        {
            if (_isLoadingSettings) return;

            if (r >= 0 && r < rsl.Length)
            {
                Screen.SetResolution(rsl[r].width, rsl[r].height, isFullScreen);
                PlayerPrefs.SetInt("ResolutionIndex", r);
                PlayerPrefs.Save();
            }
        }

        private void SetupSliders()
        {
            // Загрузка сохраненных значений громкости
            float savedMusicVolume;
            float savedSfxVolume;

            if (PlayerPrefs.HasKey("MusicVolume"))
            {
                savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume");
            }
            else
            {
                savedMusicVolume = 1.0f; // Значение по умолчанию
            }

            if (PlayerPrefs.HasKey("SfxVolume"))
            {
                savedSfxVolume = PlayerPrefs.GetFloat("SfxVolume");
            }
            else
            {
                savedSfxVolume = 1.0f; // Значение по умолчанию
            }

            // Установка начальных значений слайдеров
            _musicSlider.value = savedMusicVolume;
            _sfxSlider.value = savedSfxVolume;

            // Привязка слайдеров к изменениям громкости
            _musicSlider.onValueChanged.AddListener(SetMusicVolume);
            _sfxSlider.onValueChanged.AddListener(SetSfxVolume);

            // Применение загруженных значений к AudioMixer
            SetMusicVolume(savedMusicVolume);
            SetSfxVolume(savedSfxVolume);
        }

        public void SetMusicVolume(float volume)
        {
            if (_musicSource != null)
            {
                _musicSource.volume = volume;
            }
            PlayerPrefs.SetFloat("MusicVolume", volume);
            PlayerPrefs.Save();
        }

        public void SetSfxVolume(float volume)
        {
            if (_sfxSource != null)
            {
                _sfxSource.volume = volume;
            }
            PlayerPrefs.SetFloat("SfxVolume", volume);
            PlayerPrefs.Save();
        }


        public void FullScreenToggle()
        {
            isFullScreen = !isFullScreen;
            Screen.fullScreen = isFullScreen;
            PlayerPrefs.SetInt("FullScreen", isFullScreen ? 1 : 0);
            PlayerPrefs.Save();
        }

        private void LoadSettings()
        {
            isFullScreen = PlayerPrefs.GetInt("FullScreen", 1) == 1;
            Screen.fullScreen = isFullScreen;

            int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
            if (savedResolutionIndex >= 0 && savedResolutionIndex < Screen.resolutions.Length)
            {
                Screen.SetResolution(Screen.resolutions[savedResolutionIndex].width, Screen.resolutions[savedResolutionIndex].height, isFullScreen);
            }

            // Загрузка громкости музыки
            float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.15f);
            if (_musicSlider != null)
            {
                _musicSlider.value = savedMusicVolume; // Устанавливаем значение слайдера
            }
            SetMusicVolume(savedMusicVolume);

            // Загрузка громкости звуковых эффектов
            float savedSfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0.4f);
            if (_sfxSlider != null)
            {
                _sfxSlider.value = savedSfxVolume; // Устанавливаем значение слайдера
            }
            SetSfxVolume(savedSfxVolume);
        }
    }

}
