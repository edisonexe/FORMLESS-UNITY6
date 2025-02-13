using Formless.Core.Managers;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Formless.Room
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private GameObject wallPrefab;
        private BoxCollider2D _boxCollider2D;
        private BoxCollider2D _interactionCollider;
        public DoorType doorType;
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
            _interactionCollider = transform.Find("InteractionTrigger")?.GetComponent<BoxCollider2D>();
            if (_interactionCollider != null)
            {
                Debug.Log("InteractionTrigger ������");
            }
            else
            {
                Debug.LogWarning("InteractionTrigger �� ������");
            }
        }

        private void Start()
        {
            doorType = DoorType.Regular;
            if (!_isOpened)
            {
                Invoke("DisableDoorBoxCollider", 2f);
            }
            Invoke("CheckIfDoorTouchesLastRoom", 1f);
        }

        public void OpenDoor(string lockName)
        {
            _isOpened = true;
            _boxCollider2D.enabled = false;
            _interactionCollider.enabled = false;
            DestroyLockAndActivateMover(lockName);
        }

        public void SetAsBossDoor()
        {
            if (_isBossDoorSet) return;
        
            _isBossDoorSet = true;
            doorType = DoorType.Boss;

            // ��������� �������� �����
            GetComponent<Tilemap>().color = Color.red;
            Debug.Log("����� � ����� �����������: " + gameObject.name);

            ReplaceLockWithBossLock();
        }

        public void CheckIfDoorTouchesLastRoom()
        {
            BoxCollider2D _lastRoomCollider = GameplayManager.Instance.LastRoom.GetComponent<BoxCollider2D>(); // �������� ��������� ��������� �������


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

        public void TryUnlockDoor()
        {
            if (GameplayManager.Instance.HasBossKey && doorType == DoorType.Boss)
            {
                OpenDoor("BossLock");
                GameplayManager.Instance.UseBossKey();
                DestroyLockAndActivateMover("BossLock");
                UIManager.Instance.UseBossKey();
                UIManager.Instance.UpdateBossKeyUI();
                Debug.Log("����� ����� �������");
            }
            else if (GameplayManager.Instance.HasKey() && doorType == DoorType.KeyRequired)
            {
                OpenDoor("Lock");
                GameplayManager.Instance.UseKey();
                DestroyLockAndActivateMover("Lock");
                UIManager.Instance.UseKey();
                UIManager.Instance.UpdateKeysUI();
                Debug.Log("������� ����� �������");
            }
            else
            {
                Debug.Log("� ������ ��� ������");
            }
        }

       private void DestroyLockAndActivateMover(string lockName)
        {
            Transform lockObject = transform.Find(lockName) ?? transform.Find(lockName + "(Clone)");
            if (lockObject != null)
            {
                GameObject lockDestroyEffect;

                // �������� ������ � ����������� �� ���� �����
                if (lockName == "BossLock")
                {
                    lockDestroyEffect = PrefabManager.Instance.BossLockDestroyEffect;
                }
                else
                {
                    lockDestroyEffect = PrefabManager.Instance.LockDestroyEffect;
                }

                // ���� ������ ����������, ������ ��� �� ������� �����
                if (lockDestroyEffect != null)
                {
                    Instantiate(lockDestroyEffect, lockObject.position, Quaternion.identity);
                }

                Destroy(lockObject.gameObject);
            }

            Transform moverObject = transform.Find("Mover");
            if (moverObject != null)
            {
                moverObject.gameObject.SetActive(true);
            }

            TilemapCollider2D collider = GetComponent<TilemapCollider2D>();
            if (collider != null)
            {
                collider.enabled = false;
            }    
        }
 

        private void DisableDoorBoxCollider()
        {
            _boxCollider2D.enabled = false;
        }

    }
}
