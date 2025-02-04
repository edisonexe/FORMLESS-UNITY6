using System.Collections;
using System.Collections.Generic;
using Formless.Boss;
using UnityEngine;
using UnityEngine.UI;
using Formless.Player;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
        Player.Instance.transform.position = Vector3.zero;

        // 6. Убираем затемнение
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
}
