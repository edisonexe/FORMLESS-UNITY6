using Formless.Audio;
using Formless.Player;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance { get; private set; }

    [SerializeField] private GameObject _escMenu;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _howToPlayPanel;

    private bool _escMenuOpened = false;
    private bool _settingsOpened = false;
    private bool _howToPlayeOpened = false;
    private bool _isPaused = false;

    private SceneChecker _sceneChecker;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SetAllFlagsFalse();
        _sceneChecker = new SceneChecker();
        if (_sceneChecker.IsCurrentScene("Menu"))
        {
            AudioManager.Instance.PlayMenuMusic();
            CursorEnable();
            InputOverride.EnableInput();
        }
        else if (_sceneChecker.IsCurrentScene("Game"))
        {
            CursorDisable();
            InputOverride.DisableInput();
        }
    }

    private void Update()
    {
        if (InputOverride.GetKeyUp(KeyCode.Escape))
        {
            if (_sceneChecker.IsCurrentScene("Menu"))
            {
                if (_settingsOpened)
                {
                    CloseSettings();
                }
                else if (_howToPlayeOpened)
                {
                    CloseHowToPlay();
                }
            }
            else if (_sceneChecker.IsCurrentScene("Game"))
            {
                //Debug.Log("ESC pressed");
                if (_escMenuOpened)
                {
                    if (_settingsOpened)
                    {
                        CloseSettings();
                    }
                    else
                    {
                        CloseESCMenu();
                    }
                }
                else
                {
                    OpenESCMenu();
                }
            }
        }
    }

    private void SetAllFlagsFalse()
    {
        _escMenuOpened = false;
        _settingsOpened = false;
        _howToPlayeOpened = false;
    }

    public void StartTheRun()
    {
        AudioManager.Instance.PlaySelectSound();
        SceneManager.LoadScene("Game");
    }

    public void OpenSettings()
    {
        _settingsOpened = true;
        AudioManager.Instance.PlaySelectSound();
        _settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        _settingsOpened = false;
        AudioManager.Instance.PlayBackSound();
        _settingsPanel.SetActive(false);
    }

    public void OpenHowToPlay()
    {
        _howToPlayeOpened = true;
        AudioManager.Instance.PlaySelectSound();
        _howToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        _howToPlayeOpened = false;
        AudioManager.Instance.PlayBackSound();
        _howToPlayPanel.SetActive(false);
    }

    public void QuitGame()
    {
        CursorEnable();
        AudioManager.Instance.PlayQuitSound();
        Application.Quit();
    }

    public void BackToMenu()
    {
        Player.Instance.inputHandler.Disable();
        SetAllFlagsFalse();
        DungeonGenerator.Instance.ResetDungeon();
        AudioManager.Instance.PlayBackSound();
        SceneManager.LoadScene("Menu");
    }

    private void OpenESCMenu()
    {
        TogglePause();
        _escMenuOpened = true;
        AudioManager.Instance.PlaySelectSound();
        _escMenu.SetActive(true);
    }

    public void CloseESCMenu()
    {
        TogglePause();
        _escMenuOpened = false;
        AudioManager.Instance.PlayBackSound();
        _escMenu.SetActive(false);
    }

    private void TogglePause()
    {
        _isPaused = !_isPaused;

        if (_isPaused)
        {
            CursorEnable();
            Time.timeScale = 0f;
            Player.Instance.inputHandler.Disable();
        }
        else
        {
            CursorDisable();
            Time.timeScale = 1f;
            Player.Instance.inputHandler.Enable();
        }
    }

    public void CursorEnable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CursorDisable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
