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
            Regular,     // Открывается после зачистки комнаты
            KeyRequired, // Открывается ключом
            Boss         // Открывается только ключом босса
        }

        private void Awake()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
            _interactionCollider = transform.Find("InteractionTrigger")?.GetComponent<BoxCollider2D>();
            if (_interactionCollider != null)
            {
                Debug.Log("InteractionTrigger найден");
            }
            else
            {
                Debug.LogWarning("InteractionTrigger не найден");
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

            // Визуально выделяем дверь
            GetComponent<Tilemap>().color = Color.red;
            Debug.Log("Дверь к боссу установлена: " + gameObject.name);

            ReplaceLockWithBossLock();
        }

        public void CheckIfDoorTouchesLastRoom()
        {
            BoxCollider2D _lastRoomCollider = GameplayManager.Instance.LastRoom.GetComponent<BoxCollider2D>(); // Получаем коллайдер последней комнаты


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

        public void TryUnlockDoor()
        {
            if (GameplayManager.Instance.HasBossKey && doorType == DoorType.Boss)
            {
                OpenDoor("BossLock");
                GameplayManager.Instance.UseBossKey();
                DestroyLockAndActivateMover("BossLock");
                UIManager.Instance.UseBossKey();
                UIManager.Instance.UpdateBossKeyUI();
                Debug.Log("Дверь босса открыта");
            }
            else if (GameplayManager.Instance.HasKey() && doorType == DoorType.KeyRequired)
            {
                OpenDoor("Lock");
                GameplayManager.Instance.UseKey();
                DestroyLockAndActivateMover("Lock");
                UIManager.Instance.UseKey();
                UIManager.Instance.UpdateKeysUI();
                Debug.Log("Обычная дверь открыта");
            }
            else
            {
                Debug.Log("У игрока нет ключей");
            }
        }

       private void DestroyLockAndActivateMover(string lockName)
        {
            Transform lockObject = transform.Find(lockName) ?? transform.Find(lockName + "(Clone)");
            if (lockObject != null)
            {
                GameObject lockDestroyEffect;

                // Выбираем эффект в зависимости от типа замка
                if (lockName == "BossLock")
                {
                    lockDestroyEffect = PrefabManager.Instance.BossLockDestroyEffect;
                }
                else
                {
                    lockDestroyEffect = PrefabManager.Instance.LockDestroyEffect;
                }

                // Если эффект существует, создаём его на позиции замка
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
