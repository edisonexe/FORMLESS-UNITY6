using UnityEngine;
using Formless.Player;

public class TeleportTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _eKeyPrefab;
    private GameObject _hintUI;
    private bool playerInRange = false;

    private void Start()
    {
        if (_eKeyPrefab != null)
        {
            _hintUI = Instantiate(_eKeyPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity);
            _hintUI.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Игрок зашёл на телепорт");
            playerInRange = true;
            _hintUI.SetActive(true);
            Debug.Log(_hintUI.name);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Игрок покинул телепорт");
            playerInRange = false;
            _hintUI.SetActive(false);
        }
    }

    private void Update()
    {
        if (playerInRange && Player.Instance.IsTeleportPressed())
        {
            ActivatePortal();
        }
    }

    private void ActivatePortal()
    {
        // Логика активации портала
        Debug.Log("Перемещение через портал...");
        // Тут можно добавить логику перехода на новый уровень
    }
}
