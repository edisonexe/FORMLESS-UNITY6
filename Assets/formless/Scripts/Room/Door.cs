using Formless.Core.Managers;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Formless.Room
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private GameObject wallPrefab;
        private BoxCollider2D _boxCollider2D;
        private DoorType _doorType;
        private bool _isProcessed = false;
        private bool _isReplaced = false;
        private bool _isOpened = false;
        private bool _isBossDoorSet = false;

        public enum DoorType
        {
            Regular,     // ����������� ����� �������� �������
            KeyRequired, // ����������� ������
            Boss         // ����������� ������ ������ �����
        }

        private void Awake()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            if (!_isOpened)
            {
                Invoke("DisableDoorBoxCollider", 2f);
            }
            Invoke("CheckIfDoorTouchesLastRoom", 1f);
        }

        public void OpenDoor()
        {
            _isOpened = true;
            _boxCollider2D.enabled = false;
        }

        public void SetAsBossDoor()
        {
            if (_isBossDoorSet) return;
        
            _isBossDoorSet = true;
            _doorType = DoorType.Boss;

            // ��������� �������� �����
            GetComponent<Tilemap>().color = Color.red;
            Debug.Log("����� � ����� �����������: " + gameObject.name);

            ReplaceLockWithBossLock();
        }

        public void CheckIfDoorTouchesLastRoom()
        {
            BoxCollider2D _lastRoomCollider = GameManager.Instance.LastRoom.GetComponent<BoxCollider2D>(); // �������� ��������� ��������� �������


            if (_lastRoomCollider != null && _boxCollider2D != null && !_isBossDoorSet)
            {
                // �������� ����������� � ��������� ��������
                if (_boxCollider2D.IsTouching(_lastRoomCollider))
                {
                    Debug.Log("����� ������������ � ��������� ��������.");
                    SetAsBossDoor();
                }
                else
                {
                    Debug.Log("����� �� ������������ � ��������� ��������.");
                }
            }
            else
            {
                Debug.LogWarning("���������� �� �������!");
            }
        }


        private void OnTriggerStay2D(Collider2D collision)
        {
            if (_isProcessed || _isOpened) return;

            if (collision.CompareTag("Wall"))
            {
                ReplaceDoorWithWall();
                _isProcessed = true;
            }
        }

        private void ReplaceDoorWithWall()
        {
            Debug.Log("������� ����� �� ������, ������ �� �����");

            if (!_isReplaced)
            {
                _boxCollider2D.enabled = false;
                Instantiate(wallPrefab, transform.position, Quaternion.identity, transform.parent);
                _isReplaced = true;
                Destroy(gameObject);
            }
        }

        private void ReplaceLockWithBossLock()
        {
            // ���� �������� ������ � ��������� "Lock"
            Transform lockTransform = transform.Find("Lock");

            if (lockTransform != null)
            {
                // �������� �� PrefabManager.Instance.BossLock
                GameObject newLock = PrefabManager.Instance.BossLockPrefab;
                if (newLock != null)
                {
                    // �������� ������ ������ �� �����
                    Destroy(lockTransform.gameObject); // ������� ������ ������
                    Instantiate(newLock, lockTransform.position, lockTransform.rotation, transform); // ������� ����� ������
                    Debug.Log("������ Lock �� BossLock ���������.");
                }
                else
                {
                    Debug.LogWarning("BossLock �� ������ � PrefabManager.");
                }
            }
            else
            {
                Debug.LogWarning("�������� ������ Lock �� ������.");
            }
        }

    }
}
