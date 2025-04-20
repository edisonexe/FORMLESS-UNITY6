using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Formless.Player;
using Formless.Audio;
using Formless.UI.Menu;
using System.Collections;
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
        [SerializeField] private Settings _settings;

        public void Initialize(GameStats gameStats)
        {
            _gameStats = gameStats;

            if (_gameStats == null)
            {
                Debug.LogError("GameStats cannot be null!");
            }
        }


        private void EnablePanel()
        {
            InputOverride.DisableInput();
            if (_panel == null)
            {
                //Debug.LogError("_panel is not assigned!");
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
            //Debug.Log("[END_PANEL] SetupPanel called");

            if (_gameStats == null)
            {
                Debug.LogError("[END_PANEL] GameStats is null! Make sure Initialize is called before SetupPanel.");
                return;
            }

            EnablePanel();
            //Debug.Log("[END_PANEL] Panel enabled");

            if (titleDefeat == null || titleVictory == null)
            {
                Debug.LogError("[END_PANEL] Title UI elements are not assigned!");
                return;
            }

            titleDefeat.gameObject.SetActive(result == GameResult.Defeat);
            titleVictory.gameObject.SetActive(result == GameResult.Victory);

            UpdateUI();
            UpdatePlayTime(_gameStats.PlayTime);
        }



        private void UpdateUI()
        {

            if (_gameStats == null || _settings == null)
            {
                Debug.LogError("_gameStats or _settings is not initialized!");
                return;
            }

            switch (_settings.CurrentLanguage)
            {
                case("ru"):
                    enemiesKilledText.text = $"Врагов убито: {_gameStats.EnemiesKilled}";
                    clearedRoomsText.text = $"Комнат зачищено: {_gameStats.ClearedRooms}";
                    heartsCollectedText.text = $"Собрано сердец: {_gameStats.HeartsCollected}";
                    keysCollectedText.text = $"Собрано ключей: {_gameStats.KeysCollected}";
                    break;
                case("en"):
                    enemiesKilledText.text = $"Enemies killed: {_gameStats.EnemiesKilled}";
                    clearedRoomsText.text = $"Cleared rooms: {_gameStats.ClearedRooms}";
                    heartsCollectedText.text = $"Hearts collected: {_gameStats.HeartsCollected}";
                    keysCollectedText.text = $"Keys collected: {_gameStats.KeysCollected}";
                    break;
            }
        }

        private void UpdatePlayTime(float time)
        {
            switch (_settings.CurrentLanguage)
            {
                case("ru"):
                    playTimeText.text = $"Время игры: {FormatTime(time)}";
                    break;
                case("en"):
                    playTimeText.text = $"Play time: {FormatTime(time)}";
                    break;
            }
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

