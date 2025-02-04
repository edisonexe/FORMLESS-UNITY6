using UnityEngine;
using Formless.Player;

public class TeleportTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _hintUI;  // UI-подсказка "Нажмите E"

    private bool _playerInRange = false;

    private void Start()
    {
        if (_hintUI != null)
        {
            _hintUI.SetActive(false); // Подсказка выключена по умолчанию
        }
        else
        {
            Debug.LogError("TeleportTrigger: _hintUI не назначен в инспекторе!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (!_playerInRange)
            {
                Debug.Log("Игрок зашёл на телепорт");
                _playerInRange = true;
                _hintUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (_playerInRange)
            {
                Debug.Log("Игрок покинул телепорт");
                _playerInRange = false;
                _hintUI.SetActive(false);
            }
        }
    }

    private void Update()
    {
        if (_playerInRange && Player.Instance.IsTeleportPressed())
        {
            ActivatePortal();
        }
    }

    private void ActivatePortal()
    {
        Debug.Log("Перемещение через портал...");
        GameManager.Instance.LoadNextDungeon();
    }
}
