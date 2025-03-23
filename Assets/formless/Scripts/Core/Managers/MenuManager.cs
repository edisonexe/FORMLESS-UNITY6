using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _authorsPanel;

    public void StartTheRun()
    {
        SceneManager.LoadScene("Game");
        Debug.Log("StartTheRun");
    }

    public void OpenSettings()
    {
        _settingsPanel.SetActive(true);
        Debug.Log("Settings opened");
    }

    public void CloseSettings()
    {
        _settingsPanel.SetActive(false);
    }

    public void OpenAuthors()
    {
        _authorsPanel.SetActive(true);
        Debug.Log("Authors opened");
    }

    public void CloseAuthors()
    {
        _authorsPanel.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("QuitGame");
    }
}
