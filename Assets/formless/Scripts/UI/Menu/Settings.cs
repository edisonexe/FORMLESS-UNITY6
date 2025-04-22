using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

namespace Formless.UI.Menu
{
    public class Settings : MonoBehaviour
    {
        public bool isFullScreen;

        [SerializeField] private Slider _musicSlider;
        [SerializeField] private Slider _sfxSlider;

        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;

        public Dropdown resolutionDropdown;

        private List<Resolution> resolutions = new List<Resolution>();
        private int currentResolutionIndex = 0;

        public string CurrentLanguage { get; private set; } = "en";

        private void Awake()
        {
            LoadSettings();
            InitializeResolutions();
            SetupSliders();
        }

        private void InitializeResolutions()
        {
            // �������� ��� ��������� ����������
            resolutions.AddRange(Screen.resolutions);

            // ������� ������ ����� ��� ����������� � Dropdown
            List<string> options = new List<string>();
            for (int i = 0; i < resolutions.Count; i++)
            {
                // ���������� refreshRateRatio ������ refreshRate
                string option = resolutions[i].width + " x " + resolutions[i].height + " @ " +
                                resolutions[i].refreshRateRatio.value + "Hz";
                options.Add(option);

                // ���������, ����� ���������� �������� �������
                if (resolutions[i].width == Screen.currentResolution.width &&
                    resolutions[i].height == Screen.currentResolution.height &&
                    Mathf.Approximately((float)resolutions[i].refreshRateRatio.value, (float)Screen.currentResolution.refreshRateRatio.value))
                {
                    currentResolutionIndex = i;
                }
            }

            // ������������� ����� � Dropdown
            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(options);

            // ��������� ����������� ���������� �� PlayerPrefs ��� ���������� �������
            int savedResolutionIndex = PlayerPrefs.GetInt("SelectedResolutionIndex", currentResolutionIndex);
            SetResolution(savedResolutionIndex);

            // ��������� ��������� ��� ��������� �������� � Dropdown
            resolutionDropdown.onValueChanged.AddListener(OnResolutionChanged);
        }

        private void OnResolutionChanged(int index)
        {
            SetResolution(index);
        }

        private void SetResolution(int index)
        {
            // ������������� ����������
            Resolution resolution = resolutions[index];
            Screen.SetResolution(
                resolution.width,
                resolution.height,
                FullScreenMode.FullScreenWindow, // ���������� FullScreenMode
                resolution.refreshRateRatio // �������� refreshRateRatio ��������
            );

            // ��������� ������ ���������� ���������� � PlayerPrefs
            PlayerPrefs.SetInt("SelectedResolutionIndex", index);
            PlayerPrefs.Save();

            // ��������� �������� Dropdown
            resolutionDropdown.value = index;
            resolutionDropdown.RefreshShownValue();
        }

        private void SetupSliders()
        {
            // �������� ����������� �������� ���������
            float savedMusicVolume;
            float savedSfxVolume;

            if (PlayerPrefs.HasKey("MusicVolume"))
            {
                savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume");
            }
            else
            {
                savedMusicVolume = 1.0f; // �������� �� ���������
            }

            if (PlayerPrefs.HasKey("SfxVolume"))
            {
                savedSfxVolume = PlayerPrefs.GetFloat("SfxVolume");
            }
            else
            {
                savedSfxVolume = 1.0f; // �������� �� ���������
            }

            // ��������� ��������� �������� ���������
            _musicSlider.value = savedMusicVolume;
            _sfxSlider.value = savedSfxVolume;

            // �������� ��������� � ���������� ���������
            _musicSlider.onValueChanged.AddListener(SetMusicVolume);
            _sfxSlider.onValueChanged.AddListener(SetSfxVolume);

            // ���������� ����������� �������� � AudioMixer
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


            int savedResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", -1);
            Debug.Log($"Loaded resolution index: {savedResolutionIndex}");

            // �������� ��������� ������
            float savedMusicVolume = PlayerPrefs.GetFloat("MusicVolume", 0.12f);
            if (_musicSlider != null)
            {
                _musicSlider.value = savedMusicVolume; // ������������� �������� ��������
            }
            SetMusicVolume(savedMusicVolume);

            // �������� ��������� �������� ��������
            float savedSfxVolume = PlayerPrefs.GetFloat("SfxVolume", 0.4f);
            if (_sfxSlider != null)
            {
                _sfxSlider.value = savedSfxVolume; // ������������� �������� ��������
            }
            SetSfxVolume(savedSfxVolume);

            // �������� ���������� �����
            string savedLanguage = PlayerPrefs.GetString("SelectedLanguage", "en");
            SetLanguage(savedLanguage);
        }

        public void SaveLanguage(string localeCode)
        {
            // ��������� ��������� ����
            PlayerPrefs.SetString("SelectedLanguage", localeCode);
            PlayerPrefs.Save();

            CurrentLanguage = localeCode;
        }

        public void SetLanguage(string localeCode)
        {
            // ������� ������ �� ���� � ������������� �
            var selectedLocale = LocalizationSettings.AvailableLocales.Locales.Find(l => l.Identifier.Code == localeCode);
            if (selectedLocale != null)
            {
                LocalizationSettings.SelectedLocale = selectedLocale;

                CurrentLanguage = localeCode;
            }
        }
    }

}
