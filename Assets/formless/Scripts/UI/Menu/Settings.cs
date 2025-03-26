using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;
    public bool isFullScreen;
    public AudioMixer audioMixer;

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

    public void BackToMenu()
    {
        _settingsPanel.SetActive(false);
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

        //float savedVolume = PlayerPrefs.GetFloat("Volume", 1.0f);
        //audioMixer.SetFloat("MasterVolume", Mathf.Log10(savedVolume) * 20);
        //if (volumeSlider != null)
        //{
        //    volumeSlider.value = savedVolume;
        //}
    }
}
