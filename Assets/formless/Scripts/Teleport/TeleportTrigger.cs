using UnityEngine;
using Formless.Core.Managers;
using Formless.Player;
using Formless.Audio;
namespace Formless.UI
{
    public class TeleportTrigger : MonoBehaviour
    {
        [SerializeField] private GameObject _hintUI;  // UI-��������� "������� E"

        private bool _playerInRange = false;

        private void Start()
        {
            if (_hintUI != null)
            {
                _hintUI.SetActive(false); // ��������� ��������� �� ���������
            }
            else
            {
                Debug.LogError("TeleportTrigger: _hintUI �� �������� � ����������!");
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                if (!_playerInRange)
                {
                    //Debug.Log("����� ����� �� ��������");
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
                    //Debug.Log("����� ������� ��������");
                    _playerInRange = false;
                    _hintUI.SetActive(false);
                }
            }
        }

        private void Update()
        {
            if (_playerInRange && Player.Player.Instance.IsInteractionPressed())
            {
                AudioManager.Instance.PlaySound(AudioManager.Instance.plUseTeleport);
                ActivatePortal();
            }
        }

        private void ActivatePortal()
        {
            Debug.Log("����������� ����� ������...");
            Player.Player.Instance.inputHandler.Disable();
            GameplayManager.Instance.LoadNextDungeon();
        }
    }
}
