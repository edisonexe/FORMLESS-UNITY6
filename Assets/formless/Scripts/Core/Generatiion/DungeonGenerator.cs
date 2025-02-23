using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Formless.Core.Managers;
using Formless.Room;
using System.Linq;

public class DungeonGenerator : MonoBehaviour
{
    public static DungeonGenerator Instance { get; private set; }

    [Header("Rooms Data")]
    [SerializeField] private GameObject[] _topRooms;
    [SerializeField] private GameObject[] _bottomRooms;
    [SerializeField] private GameObject[] _leftRooms;
    [SerializeField] private GameObject[] _rightRooms;
    [SerializeField] private GameObject _closedRoom;
    [SerializeField] private GameObject _mainRoomPrefab;

    public static Action OnDungeonGenerationCompleted;
    private List<GameObject> _generatedRooms = new List<GameObject>();
    private float _checkDelay = 1.5f;
    private float _timeSinceLastRoom = 0;

    public GameObject LastRoom { get; private set; }
    public GameObject PenultimateRoom { get; private set; }
    public int KeysSpawned { get; private set; }
    public int MaxCountKeys { get; private set; }
    public int HeartsSpawned { get; private set; }
    public int MaxCountHearts { get; private set; }
    public int MaxCountKeyRequiredDoors { get; private set; }
    public GameObject[] TopRooms => _topRooms;
    public GameObject[] BottomRooms => _bottomRooms;
    public GameObject[] LeftRooms => _leftRooms;
    public GameObject[] RightRooms => _rightRooms;
    public GameObject ClosedRoom => _closedRoom;
    public GameObject MainRoomPrefab => _mainRoomPrefab;

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
        GameplayManager.Instance.SetDungeonGenerator(this);
        SpawnMainRoom();
        StartCoroutine(CheckDungeonGenerationCompleteness());
    }
    
    public void RegisterKey()
    {
        KeysSpawned++;
    }

    public void RegisterHeart()
    {
        HeartsSpawned++;
    }

    public void RegisterRoom(GameObject room)
    {
        _generatedRooms.Add(room);

        _timeSinceLastRoom = 0;
    }

    public bool CanSpawnHeart()
    {
        return HeartsSpawned < MaxCountHearts;
    }

    public bool CanSpawnKey()
    {
        return KeysSpawned < MaxCountKeys;
    }

    private void SetMaxCountKeys()
    {
        MaxCountKeys = _generatedRooms.Count / 2;
        Debug.Log("Макс. кол-во ключей " + MaxCountKeys);
    }

    private void SetMaxCountHearts()
    {
        MaxCountHearts = _generatedRooms.Count / 3;
        Debug.Log("Макс. кол-во сердец " + MaxCountHearts);
    }

    private void SetMaxCountKeyRequiredDoors()
    {
        MaxCountKeyRequiredDoors = _generatedRooms.Count / 3;
        Debug.Log("Макс. кол-во KRдверей = " + MaxCountKeyRequiredDoors);
    }

    private void SetLastRoom()
    {
        if (_generatedRooms.Count > 0)
        {
            LastRoom = _generatedRooms[_generatedRooms.Count - 1];
        }
        else
        {
            LastRoom= null;
        }
    }

    private void SetPenultimateRoom()
    {
        if (_generatedRooms.Count > 1)
        {
            PenultimateRoom = _generatedRooms[_generatedRooms.Count - 2];
        }
        else
        {
            PenultimateRoom = null;
        }
    }

    private IEnumerator CheckDungeonGenerationCompleteness()
    {
        while (true)
        {
            yield return new WaitForSeconds(_checkDelay);

            _timeSinceLastRoom += _checkDelay;

            if (_timeSinceLastRoom > 1.5f)
            {
                Debug.Log("* [DUNGEON_GENERATOR] Генерация подземелья завершена!");
                GameplayManager.Instance.SetRoomsList(_generatedRooms);
                SetMaxCountHearts();
                SetMaxCountKeys();
                SetMaxCountKeyRequiredDoors();
                SetLastRoom();
                SetPenultimateRoom();

                OnDungeonGenerationCompleted?.Invoke();

                AssignKeyRequiredDoors();
                SpawnKeyInAccessibleRooms();
                SpawnHearstInRooms();
                yield break;
            }
        }
    }

    private void AssignKeyRequiredDoors()
    {
        List<Door> allDoors = new List<Door>();

        foreach (GameObject room in _generatedRooms)
        {
            DoorsController doorsController = room.GetComponent<DoorsController>();
        
            // Если компонента нет, выводим предупреждение
            if (doorsController == null)
            {
                //Debug.LogWarning($"Комната {room.name} не имеет компонента DoorsController!");
                continue;
            }

            // Если у комнаты нет дверей, выводим сообщение и продолжаем
            if (doorsController.Doors == null || doorsController.Doors.Length == 0)
            {
                //Debug.LogWarning($"Комната {room.name} не имеет дверей!");
                continue;
            }

            allDoors.AddRange(doorsController.Doors
                .Where(d => d != null && d.gameObject != null) // Проверяем, что объект не null
                .Select(d => d.GetComponent<Door>())
                .Where(d => d != null && d.DoorType != DoorType.Boss)); // Убираем null-объекты и двери Boss
        }

        if (allDoors.Count == 0) return;

        System.Random rng = new System.Random();
        allDoors = allDoors.OrderBy(d => rng.Next()).ToList();

        for (int i = 0; i < MaxCountKeyRequiredDoors && i < allDoors.Count; i++)
        {
            allDoors[i].SetKeyRequired();
        }
    }

    private void SpawnMainRoom()
    {
        GameObject mainRoom = Instantiate(_mainRoomPrefab, transform.position, Quaternion.identity);
    }

    private void SpawnHearstInRooms()
    {
        foreach (GameObject room in _generatedRooms)
        {
            if (room == null || !room.activeInHierarchy) continue;
            RoomController roomController = room.GetComponent<RoomController>();
            if (roomController == null) continue;
            bool itemSpawned = roomController.ItemWasSpawned;
            if (!itemSpawned)
            {
                int rnd = UnityEngine.Random.Range(1, 3);
                ItemSpawner itemSpawner = room.GetComponent<ItemSpawner>();
                if (itemSpawner == null) continue;
                if (rnd == 1) itemSpawner.SpawnHeart();
            }
        }
    }

    private void SpawnKeyInAccessibleRooms()
    {
        int keyCount = 0;

        // Списки для комнат с дверями типа Opened, Regular и KeyRequired
        List<GameObject> regularRooms = new List<GameObject>();
        List<GameObject> keyRequiredRooms = new List<GameObject>();

        // Проходим по всем сгенерированным комнатам
        foreach (GameObject room in _generatedRooms)
        {
            // Проверяем, что объект активен и существует
            if (room == null || !room.activeInHierarchy) continue;

            // Получаем компонент с предметами (ключами)
            ItemSpawner itemSpawner = room.GetComponent<ItemSpawner>();
            if (itemSpawner == null) continue;

            // Получаем все двери в комнате
            DoorsController doorsController = room.GetComponent<DoorsController>();
            if (doorsController == null) continue;

            bool hasKeyRequiredDoor = false;
            bool hasRegularDoor = false;

            // Проверяем все двери в комнате
            foreach (GameObject doorObj in doorsController.Doors)
            {
                if (doorObj == null || !doorObj.activeInHierarchy) continue;

                Door door = doorObj.GetComponent<Door>();
                if (door == null) continue;

                // Если дверь типа KeyRequired, добавляем в keyRequiredRooms
                if (door.DoorType == DoorType.KeyRequired)
                {
                    hasKeyRequiredDoor = true;
                }
                // Если дверь типа Regular или Opened, добавляем в regularRooms
                else if (door.DoorType == DoorType.Regular || door.DoorType == DoorType.Opened)
                {
                    hasRegularDoor = true;
                }
            }

            // Добавляем комнату в соответствующие списки
            if (hasRegularDoor)
            {
                regularRooms.Add(room); // Даже если есть KeyRequired, регулярная дверь всё равно добавит в regularRooms
            }
            if (hasKeyRequiredDoor)
            {
                keyRequiredRooms.Add(room);
            }
        }

        // Спавним ключи сначала в комнатах с дверями типа Opened или Regular
        foreach (GameObject room in regularRooms)
        {
            if (room == null || !room.activeInHierarchy) continue;

            ItemSpawner itemSpawner = room.GetComponent<ItemSpawner>();
            itemSpawner?.SpawnKey();
            keyCount++;
            if (keyCount >= MaxCountKeyRequiredDoors) return; // Останавливаем, когда достигнут лимит ключей
        }

        // Спавним оставшиеся ключи в комнатах с дверями типа KeyRequired
        foreach (GameObject room in keyRequiredRooms)
        {
            if (room == null || !room.activeInHierarchy) continue;

            ItemSpawner itemSpawner = room.GetComponent<ItemSpawner>();
            itemSpawner?.SpawnKey();
            keyCount++;
            if (keyCount >= MaxCountKeyRequiredDoors) return; // Останавливаем, когда достигнут лимит ключей
        }
    }
}
