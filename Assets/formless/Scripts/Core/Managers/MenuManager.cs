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
            //DontDestroyOnLoad(gameObject);
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
                Debug.Log("ESC pressed");
                if (_escMenuOpened)
                {
                    TogglePause();
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
                    TogglePause();
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
        //Debug.Log("StartTheRun");
    }

    public void OpenSettings()
    {
        _settingsOpened = true;
        AudioManager.Instance.PlaySelectSound();
        _settingsPanel.SetActive(true);
        //Debug.Log("Settings opened");
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
        //Debug.Log("Authors opened");
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
        //Debug.Log("QuitGame");
    }

    public void BackToMenu()
    {
        Player.Instance.inputHandler.Disable();
        SetAllFlagsFalse();
        DungeonGenerator.Instance.ResetDungeon();
        AudioManager.Instance.PlayBackSound();
        SceneManager.LoadScene("Menu");
        //Debug.Log("BackToMenu");
    }

    private void OpenESCMenu()
    {
        _escMenuOpened = true;
        AudioManager.Instance.PlaySelectSound();
        _escMenu.SetActive(true);
    }

    private void CloseESCMenu()
    {
        _escMenuOpened = false;
        AudioManager.Instance.PlayBackSound();
        _escMenu.SetActive(false);
    }

    private void TogglePause()
    {
        _isPaused = !_isPaused;

        if (_isPaused)
        {
            Time.timeScale = 0f;
            CursorEnable();
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
        //Debug.Log("CursorDisable");
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
