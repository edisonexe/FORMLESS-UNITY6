using System.Collections;
using System.Collections.Generic;
using Formless.Boss;
using UnityEngine;
using UnityEngine.UI;
using Formless.Player;
using System;
using Formless.Enemy;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
        Stats.StartTrackingTime();
        _bossSpawner = new BossSpawner(bossPrefab, teleportPrefab);
    }

    private void Update()
    {
        Stats.UpdateTime(Time.deltaTime);
    }

    public void TrySpawnBoss(GameObject room)
    {
        _bossSpawner.TrySpawnBoss(room);
    }

    public void SpawnTeleport(Boss boss)
    {
        _bossSpawner.SpawnTeleport(boss);
    }

    public void LoadNextDungeon()
    {
        StartCoroutine(DungeonTransition());
    }

    private IEnumerator DungeonTransition()
    {
        // 1. ��������� �����
        yield return StartCoroutine(FadeToBlack());

        // 2. ������� ������ �������
        GameObject[] allObjects = FindObjectsByType<GameObject>(FindObjectsSortMode.None); // �������� ��� ������� � �����
        foreach (GameObject obj in allObjects)
        {
            if (obj.name.StartsWith("Room"))
            {
                Destroy(obj);
            }
        }

        // 3. ������� ������ ������
        rooms.Clear();

        yield return new WaitForSeconds(2f);


        // 4. ������� ����� ������� � ������
        GameObject newRoom = Instantiate(_mainRoomPrefab, Vector3.zero, Quaternion.identity);
        newRoom.name = "RoomMain";
        rooms.Add(newRoom);

        // 5. ���������� ������ � �����
        Player.Instance.transform.position = Vector3.zero;

        // 6. ���������� ������ � �����
        Camera.main.transform.position = new Vector3(0, 1, -10);

        // 7. ������� ����������
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
    }

    public void SetLastKilledEnemy(Enemy enemy)
    {
        LastKilledEnemyData = new EnemyData(enemy);
    }

    public void PrintStats()
    {
        Debug.LogFormat("����� ������: {0}", Stats.PlayTime);
        Debug.LogFormat("���������� ������ ������: {0}", Stats.EnemiesKilled);
        Debug.LogFormat("���������� ��������� ������: {0}", Stats.ClearedRooms);
        Debug.LogFormat("���������� �������� ������: {0}", Stats.HeartsCollected);
        Debug.LogFormat("���������� �������� ������: {0}", Stats.KeysCollected);
    }

    public void PickupBossKey()
    {
        HasBossKey = true;
        UIManager.Instance.HasBossKey();
        UIManager.Instance.UpdateBossKeyUI();
    }
}
