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
            Regular,     // Открывается после зачистки комнаты
            KeyRequired, // Открывается ключом
            Boss         // Открывается только ключом босса
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

            // Визуально выделяем дверь
            GetComponent<Tilemap>().color = Color.red;
            Debug.Log("Дверь к боссу установлена: " + gameObject.name);

            ReplaceLockWithBossLock();
        }

        public void CheckIfDoorTouchesLastRoom()
        {
            BoxCollider2D _lastRoomCollider = GameManager.Instance.LastRoom.GetComponent<BoxCollider2D>(); // Получаем коллайдер последней комнаты


            if (_lastRoomCollider != null && _boxCollider2D != null && !_isBossDoorSet)
            {
                // Проверка пересечения с последней комнатой
                if (_boxCollider2D.IsTouching(_lastRoomCollider))
                {
                    Debug.Log("Дверь пересекается с последней комнатой.");
                    SetAsBossDoor();
                }
                else
                {
                    Debug.Log("Дверь не пересекается с последней комнатой.");
                }
            }
            else
            {
                Debug.LogWarning("Коллайдеры не найдены!");
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
            Debug.Log("Колизия двери со стеной, замена на стену");

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
            // Ищем дочерний объект с названием "Lock"
            Transform lockTransform = transform.Find("Lock");

            if (lockTransform != null)
            {
                // Заменяем на PrefabManager.Instance.BossLock
                GameObject newLock = PrefabManager.Instance.BossLockPrefab;
                if (newLock != null)
                {
                    // Заменяем старый объект на новый
                    Destroy(lockTransform.gameObject); // Удаляем старый объект
                    Instantiate(newLock, lockTransform.position, lockTransform.rotation, transform); // Создаем новый объект
                    Debug.Log("Замена Lock на BossLock завершена.");
                }
                else
                {
                    Debug.LogWarning("BossLock не найден в PrefabManager.");
                }
            }
            else
            {
                Debug.LogWarning("Дочерний объект Lock не найден.");
            }
        }

    }
}
