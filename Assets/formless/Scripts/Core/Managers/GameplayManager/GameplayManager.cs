using System.Collections;
using System.Collections.Generic;
using Formless.Boss;
using UnityEngine;
using UnityEngine.UI;
using Formless.Room;

namespace Formless.Core.Managers
{
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager Instance { get; private set; }

        public RoomController CurrentRoom { get; private set; }
        public GameStats Stats { get; private set; } = new GameStats();
        public EnemyData LastKilledEnemyData { get; private set; }
        public bool HasBossKey { get; private set; } = false;

        [Header("Rooms Data")]
        [SerializeField] public GameObject[] topRooms;
        [SerializeField] public GameObject[] bottomRooms;
        [SerializeField] public GameObject[] leftRooms;
        [SerializeField] public GameObject[] rightRooms;
        [SerializeField] public GameObject closedRoom;
        [SerializeField] private GameObject _mainRoomPrefab;

        [SerializeField] private Image _fadeScreen;
        public List<GameObject> rooms;

        [Header("Boss Data")]
        [SerializeField] private GameObject bossPrefab;
        [SerializeField] private GameObject teleportPrefab;

        private BossSpawner _bossSpawner;

        private int _heartsSpawned = 0;
        private int _keysSpawned = 0;
        private const int MAX_HEARTS = 2;
        private const int MAX_KEYS = 2;
        private int _keys = 0;
        public GameObject LastRoom => rooms.Count > 0 ? rooms[rooms.Count - 1] : null;
        public GameObject PenultimateRoom => rooms.Count > 1 ? rooms[rooms.Count - 2] : null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            Stats.StartTrackingTime();
            _bossSpawner = new BossSpawner(bossPrefab, teleportPrefab);
        }

        private void Update()
        {
            Stats.UpdateTime(Time.deltaTime);
        }

        //public void TrySpawnBoss(GameObject room)
        //{
        //    _bossSpawner.TrySpawnBoss(room);
        //}

        //public void SpawnTeleport(Boss.Boss boss)
        //{
        //    _bossSpawner.SpawnTeleport(boss);
        //}

        public void LoadNextDungeon()
        {
            StartCoroutine(DungeonTransition());
        }

        private IEnumerator DungeonTransition()
        {
            // 1. Затемняем экран
            yield return StartCoroutine(FadeToBlack());

            // 2. Удаляем старые комнаты
            GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None); // Получаем все объекты в сцене
            foreach (GameObject obj in allObjects)
            {
                if (obj.name.StartsWith("Room"))
                {
                    Destroy(obj);
                }
            }

            // 3. Очищаем список комнат
            rooms.Clear();

            yield return new WaitForSeconds(2f);


            // 4. Спавним новую комнату в центре
            GameObject newRoom = Instantiate(_mainRoomPrefab, Vector3.zero, Quaternion.identity);
            newRoom.name = "RoomMain";
            rooms.Add(newRoom);

            // 5. Перемещаем игрока в центр
            Player.Player.Instance.transform.position = Vector3.zero;

            // 6. Перемещаем камеру в центр
            Camera.main.transform.position = new Vector3(0, 1, -10);

            // 7. Убираем затемнение
            yield return StartCoroutine(FadeToClear());
        }


        private IEnumerator FadeToBlack()
        {
            float duration = 1f;
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                _fadeScreen.color = new Color(0, 0, 0, t / duration);
                yield return null;
            }
            _fadeScreen.color = Color.black;
        }

        private IEnumerator FadeToClear()
        {
            float duration = 1f;
            for (float t = 0; t < duration; t += Time.deltaTime)
            {
                _fadeScreen.color = new Color(0, 0, 0, 1 - (t / duration));
                yield return null;
            }
            _fadeScreen.color = new Color(0, 0, 0, 0);
        }

        public void EnemyKilled()
        {
            Stats.EnemyKilled();
        }

        public void RoomCleared()
        {
            Stats.RoomCleared();
        }

        public void HeartCollected()
        {
            Stats.HeartCollected();
        }

        public void KeyCollected()
        {
            Stats.KeyCollected();
            _keys += 1;
        }

        public bool CanSpawnHeart()
        {
            return _heartsSpawned < MAX_HEARTS;
        }

        public bool CanSpawnKey()
        {
            return _keysSpawned < MAX_KEYS;
        }

        public bool CanSetKeyRequiredDoor()
        {
            return _keysSpawned >= 1;
        }

        //public bool CanSpawnKey()
        //{
        //    return true;
        //    /*return _keysSpawned < _closedDoors;*/ // Ключей не должно быть больше, чем закрытых дверей
        //}

        public void RegisterHeart()
        {
            _heartsSpawned++;
        }

        public void RegisterKey()
        {
            _keysSpawned++;
        }

        public void SetLastKilledEnemy(Enemy.Enemy enemy)
        {
            LastKilledEnemyData = new EnemyData(enemy);
        }

        public void PrintStats()
        {
            Debug.LogFormat("Время забега: {0}", Stats.PlayTime);
            Debug.LogFormat("Количество убитых врагов: {0}", Stats.EnemiesKilled);
            Debug.LogFormat("Количество защиенных комнат: {0}", Stats.ClearedRooms);
            Debug.LogFormat("Количество поднятых сердец: {0}", Stats.HeartsCollected);
            Debug.LogFormat("Количество поднятых ключей: {0}", Stats.KeysCollected);
        }

        public void PickupBossKey()
        {
            HasBossKey = true;
            UIManager.Instance.HasBossKey();
            UIManager.Instance.UpdateBossKeyUI();
        }

        public void UseBossKey()
        {
            HasBossKey = false;
        }

        public void SetCurrentRoom(RoomController room)
        {
            if (CurrentRoom != room)
            {
                //LastRoom = CurrentRoom;
                CurrentRoom = room;

                Debug.Log($"Игрок вошёл в комнату: {room.gameObject.name}");
            }
        }

        public bool HasKey()
        {
            return _keys > 0;
        }

        public void UseKey()
        {
            if (_keys > 0)
            {
                _keys--;
                _keysSpawned--;
            }
        }
    }

}

