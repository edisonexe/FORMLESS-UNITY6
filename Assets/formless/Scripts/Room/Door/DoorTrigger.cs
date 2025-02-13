using UnityEngine;

namespace Formless.Room
{
    public class DoorTrigger : MonoBehaviour
    {
        private bool _playerInRange = false;
        private Door _door;

        private void Start()
        {
            _door = GetComponentInParent<Door>();
            if (_door == null)
            {
                Debug.LogError("DoorTrigger не нашел родительский Door!");
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (!_playerInRange)
                {
                    Debug.Log("Игрок зашёл на дверь");
                    _playerInRange = true;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (_playerInRange)
                {
                    Debug.Log("Игрок покинул дверь");
                    _playerInRange = false;
                }
            }
        }

        private void Update()
        {
            if (_playerInRange && Player.Player.Instance.IsInteractionPressed())
            {
                _door.TryUnlockDoor();
            }
        }
    }
}

