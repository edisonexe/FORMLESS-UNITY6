using Formless.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _howToPlayPanel;

    public void StartTheRun()
    {
        MenuAudioManager.Instance.PlaySelectSound();
        SceneManager.LoadScene("Game");
        Debug.Log("StartTheRun");
    }

    public void OpenSettings()
    {
        MenuAudioManager.Instance.PlaySelectSound();
        _settingsPanel.SetActive(true);
        Debug.Log("Settings opened");
    }

    public void CloseSettings()
    {
        MenuAudioManager.Instance.PlayBackSound();
        _settingsPanel.SetActive(false);
    }

    public void OpenAuthors()
    {
        MenuAudioManager.Instance.PlaySelectSound();
        _howToPlayPanel.SetActive(true);
        Debug.Log("Authors opened");
    }

    public void CloseHowToPlay()
    {
        MenuAudioManager.Instance.PlayBackSound();
        _howToPlayPanel.SetActive(false);
    }

    public void QuitGame()
    {
        MenuAudioManager.Instance.PlayQuitSound();
        Application.Quit();
        Debug.Log("QuitGame");
    }
}
