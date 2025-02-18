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
        Debug.Log("����. ���-�� ������ " + MaxCountKeys);
    }

    private void SetMaxCountHearts()
    {
        MaxCountHearts = _generatedRooms.Count / 3;
        Debug.Log("����. ���-�� ������ " + MaxCountHearts);
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
                Debug.Log("* [DUNGEON_GENERATOR] ��������� ���������� ���������!");
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
        //Debug.Log("����� AssignKR");

        List<Door> allDoors = new List<Door>();

        // �������� �� ���� ��������
        foreach (GameObject room in _generatedRooms)
        {
            // �������� ��������� DoorsController
            DoorsController doorsController = room.GetComponent<DoorsController>();
        
            // ���� ���������� ���, ������� ��������������
            if (doorsController == null)
            {
                //Debug.LogWarning($"������� {room.name} �� ����� ���������� DoorsController!");
                continue;
            }

            // ���� � ������� ��� ������, ������� ��������� � ����������
            if (doorsController.doors == null || doorsController.doors.Length == 0)
            {
                //Debug.LogWarning($"������� {room.name} �� ����� ������!");
                continue;
            }

            //Debug.Log($"������� {room.name} ����� {doorsController.doors.Length} ������.");

            allDoors.AddRange(doorsController.doors
                .Where(d => d != null && d.gameObject != null) // ���������, ��� ������ �� null
                .Select(d => d.GetComponent<Door>())
                .Where(d => d != null)); // ������� null-�������

        }

        // ������� ���������� ������
        //Debug.Log($"����� ���������� ������: {allDoors.Count}");
        //Debug.Log("����� ���-�� ������" + KeysSpawned + " ���-�� ������ " +  allDoors.Count);
        if (KeysSpawned == 0 || allDoors.Count == 0) return;
        //Debug.Log("����� ���-�� ������" + KeysSpawned + " ���-�� ������ " +  allDoors.Count);

        // ������������ ����� ��������� �������
        System.Random rng = new System.Random();
        allDoors = allDoors.OrderBy(d => rng.Next()).ToList();

        // ��� ������ ����� ���������, ��� ��� ������� �����
        for (int i = 0; i < KeysSpawned && i < allDoors.Count; i++)
        {
            allDoors[i].SetKeyRequired();
            //Debug.Log("�����" + allDoors[i] + "����� KeyRequired!");
        }
    }
}
