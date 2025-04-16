using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Formless.Player;
using Formless.Audio;
namespace Formless.UI
{
    public class EndPanel : MonoBehaviour
    {
        [SerializeField] private GameObject _panel;
        [SerializeField] private Text enemiesKilledText;
        [SerializeField] private Text clearedRoomsText;
        [SerializeField] private Text heartsCollectedText;
        [SerializeField] private Text keysCollectedText;
        [SerializeField] private Text playTimeText;
        [SerializeField] private Text titleDefeat;
        [SerializeField] private Text titleVictory;

        private GameStats _gameStats;

        public void Initialize(GameStats gameStats)
        {
            _gameStats = gameStats;

            if (_gameStats == null)
            {
                Debug.LogError("GameStats cannot be null!");
            }
            else
            {
                Debug.Log("GameStats successfully initialized.");
            }
        }


        private void EnablePanel()
        {
            InputOverride.DisableInput();
            if (_panel == null)
            {
                Debug.LogError("_panel is not assigned!");
                return;
            }
            _panel.SetActive(true);
            MenuManager.Instance.CursorEnable();
        }

        private void DisablePanel()
        {
            _panel.SetActive(false);
            InputOverride.EnableInput();
            Player.Player.Instance.inputHandler.Disable();
        }

        public void SetupPanel(GameResult result)
        {
            Debug.Log("[END_PANEL] SetupPanel called");

            if (_gameStats == null)
            {
                Debug.LogError("[END_PANEL] GameStats is null! Make sure Initialize is called before SetupPanel.");
                return;
            }

            EnablePanel();
            Debug.Log("[END_PANEL] Panel enabled");

            if (titleDefeat == null || titleVictory == null)
            {
                Debug.LogError("[END_PANEL] Title UI elements are not assigned!");
                return;
            }

            titleDefeat.gameObject.SetActive(result == GameResult.Defeat);
            titleVictory.gameObject.SetActive(result == GameResult.Victory);
            Debug.Log($"[END_PANEL] Result: {result}");

            UpdateUI();
            UpdatePlayTime(_gameStats.PlayTime);
        }



        private void UpdateUI()
        {
            if (_gameStats == null) return;

            enemiesKilledText.text = $"Enemies Killed: {_gameStats.EnemiesKilled}";
            clearedRoomsText.text = $"Cleared Rooms: {_gameStats.ClearedRooms}";
            heartsCollectedText.text = $"Hearts Collected: {_gameStats.HeartsCollected}";
            keysCollectedText.text = $"Keys Collected: {_gameStats.KeysCollected}";
        }

        private void UpdatePlayTime(float time)
        {
            playTimeText.text = $"Play Time: {FormatTime(time)}";
        }

        private string FormatTime(float time)
        {
            int minutes = Mathf.FloorToInt(time / 60);
            int seconds = Mathf.FloorToInt(time % 60);
            return $"{minutes:D2}:{seconds:D2}";
        }

        public void BackToMenu()
        {
            AudioManager.Instance.PlayBackSound();
            SceneManager.LoadScene("Menu");
            DisablePanel();
        }
    }

    public enum GameResult
    {
        Defeat,
        Victory
    }
}

