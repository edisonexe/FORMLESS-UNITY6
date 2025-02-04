using System.Collections.Generic;
using Formless.Boss;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Rooms Data")]
    [SerializeField] public GameObject[] topRooms;
    [SerializeField] public GameObject[] bottomRooms;
    [SerializeField] public GameObject[] leftRooms;
    [SerializeField] public GameObject[] rightRooms;
    [SerializeField] public GameObject closedRoom;

    public List<GameObject> rooms;

    [Header("Boss Data")]
    [SerializeField] private GameObject bossPrefab;
    [SerializeField] private GameObject teleportPrefab;

    private BossSpawner _bossSpawner;

    public GameObject LastRoom => rooms.Count > 0 ? rooms[rooms.Count - 1] : null;

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
        _bossSpawner = new BossSpawner(bossPrefab, teleportPrefab);
    }

    public void TrySpawnBoss(GameObject room)
    {
        _bossSpawner.TrySpawnBoss(room);
    }

    public void SpawnTeleport(Boss boss)
    {
        _bossSpawner.SpawnTeleport(boss);
    }
}
