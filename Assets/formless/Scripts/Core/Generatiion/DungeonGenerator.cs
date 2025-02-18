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

            if (_timeSinceLastRoom > 3f)
            {
                Debug.Log("* [DUNGEON_GENERATOR] Генерация подземелья завершена!");
                GameplayManager.Instance.SetRoomsList(_generatedRooms);
                SetMaxCountHearts();
                SetMaxCountKeys();
                SetLastRoom();
                SetPenultimateRoom();

                OnDungeonGenerationCompleted?.Invoke();

                AssignKeyRequiredDoors();
                yield break;
            }
        }
    }

    private void AssignKeyRequiredDoors()
    {
        //Debug.Log("Вызов AssignKR");

        List<Door> allDoors = new List<Door>();

        // Проходим по всем комнатам
        foreach (GameObject room in _generatedRooms)
        {
            // Получаем компонент DoorsController
            DoorsController doorsController = room.GetComponent<DoorsController>();
        
            // Если компонента нет, выводим предупреждение
            if (doorsController == null)
            {
                //Debug.LogWarning($"Комната {room.name} не имеет компонента DoorsController!");
                continue;
            }

            // Если у комнаты нет дверей, выводим сообщение и продолжаем
            if (doorsController.doors == null || doorsController.doors.Length == 0)
            {
                //Debug.LogWarning($"Комната {room.name} не имеет дверей!");
                continue;
            }

            //Debug.Log($"Комната {room.name} имеет {doorsController.doors.Length} дверей.");

            allDoors.AddRange(doorsController.doors
                .Where(d => d != null && d.gameObject != null) // Проверяем, что объект не null
                .Select(d => d.GetComponent<Door>())
                .Where(d => d != null)); // Убираем null-объекты

        }

        // Выводим количество дверей
        //Debug.Log($"Общее количество дверей: {allDoors.Count}");
        //Debug.Log("Общее кол-во ключей" + KeysSpawned + " Кол-во дверей " +  allDoors.Count);
        if (KeysSpawned == 0 || allDoors.Count == 0) return;
        //Debug.Log("Общее кол-во ключей" + KeysSpawned + " Кол-во дверей " +  allDoors.Count);

        // Перемешиваем двери случайным образом
        System.Random rng = new System.Random();
        allDoors = allDoors.OrderBy(d => rng.Next()).ToList();

        // Для каждой двери назначаем, что она требует ключа
        for (int i = 0; i < KeysSpawned && i < allDoors.Count; i++)
        {
            allDoors[i].SetKeyRequired();
            //Debug.Log("Дверь" + allDoors[i] + "стала KeyRequired!");
        }
    }
}
