using System.Collections;
using System.Collections.Generic;
using Formless.Boss;
using UnityEngine;
using UnityEngine.UI;
using Formless.Room;
using Formless.UI;

namespace Formless.Core.Managers
{
    public class GameplayManager : MonoBehaviour
    {
        public static GameplayManager Instance { get; private set; }

        public RoomController CurrentRoom { get; private set; }
        public GameStats Stats { get; private set; } = new GameStats();
        public EnemyData LastKilledEnemyData { get; private set; }

        public EndPanel endPanel;
        public bool HasBossKey { get; private set; } = false;

        private DungeonGenerator _dungeonGenerator;

        [SerializeField] private Image _fadeScreen;
        private List<GameObject> _rooms;

        //[Header("Boss Data")]
        //[SerializeField] private GameObject[] bossPrefabs;
        //[SerializeField] private GameObject teleportPrefab;

        private BossSpawner _bossSpawner;

        private int _keys = 0;
        private bool _isTimerStarted = false;

        public GameObject LastRoom => _rooms.Count > 0 ? _rooms[_rooms.Count - 1] : null;
        public GameObject PenultimateRoom => _rooms.Count > 1 ? _rooms[_rooms.Count - 2] : null;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }


        private void Start()
        {
            InitializeEndPanel();
            //_bossSpawner = new BossSpawner(bossPrefabs, teleportPrefab);
            DungeonGenerator.OnDungeonFullGenerated += HandleWaitDungeonFullGeneration;
        }

        private void Update()
        {
            Stats.UpdateTime(Time.deltaTime);
        }

         private void InitializeEndPanel()
        {
            endPanel = UIManager.Instance.endPanel.GetComponent<EndPanel>();
            if (endPanel == null)
            {
                Debug.LogError("EndPanel component is missing on the assigned object in UIManager!");
                return;
            }
            endPanel.Initialize(Stats);
        }

        public void SetDungeonGenerator(DungeonGenerator generator)
        {
            _dungeonGenerator = generator;
        }

        public DungeonGenerator GetDungeonGenerator()
        {
            return _dungeonGenerator;
        }

        public void SetRoomsList(List<GameObject> rooms)
        {
            _rooms = rooms;
        }

        public void LoadNextDungeon()
        {
            StartCoroutine(DungeonTransition());
        }

        private IEnumerator DungeonTransition()
        {
            yield return StartCoroutine(FadeToBlack());

            // ∆дЄм один кадр, чтобы сцена успела загрузитьс€
            //yield return null;

            _rooms.Clear();

            DungeonGenerator.OnDungeonFullGenerated += HandleWaitDungeonFullGeneration;
            DungeonGenerator.Instance.LoadNewDungeon();
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

        public void FadeToClear1()
        {
            StartCoroutine(FadeToClear());
        }

        private void HandleWaitDungeonFullGeneration()
        {
            DungeonGenerator.OnDungeonFullGenerated -= HandleWaitDungeonFullGeneration;
    
            if (!_isTimerStarted)
            {
                Stats.StartTrackingTime();
                _isTimerStarted = true; // ”станавливаем флаг
            }

            Player.Player.Instance.transform.position = Vector3.zero;
            Camera.main.transform.position = new Vector3(0, 0.5f, -10);
            FadeToClear1();
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
                CurrentRoom = room;

                Debug.Log($"»грок вошЄл в комнату: {room.gameObject.name}");
            }
        }

        public bool HasKey()
        {
            return _keys > 0;
        }

        public void SetPlayerKeysCount(int count)
        {
            _keys = count;
        }
    }

}

