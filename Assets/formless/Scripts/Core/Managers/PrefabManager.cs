using Formless.Room;
using System.Collections.Generic;
using UnityEngine;

namespace Formless.Core.Managers
{
    public class PrefabManager : MonoBehaviour
    {
        public static PrefabManager Instance { get; private set; }

        [Header("Prefabs")]
        [SerializeField] private GameObject _heartPrefab;
        [SerializeField] private GameObject _keyPrefab;
        [SerializeField] private GameObject[] _enemyPrefabs;
        [SerializeField] private GameObject _bossPrefab;
        [SerializeField] private GameObject _teleportPrefab;
        [SerializeField] private GameObject _lockDestroyEffect;
        [SerializeField] private GameObject _bossLockDestroyEffect;
        [SerializeField] private GameObject _bossKeyPrefab;
        [SerializeField] private GameObject _bossLockPrefab;

        [SerializeField] private GameObject _damageTextPrefab;
        [SerializeField] private GameObject _bombPrefabWith;
        [SerializeField] private GameObject _bombPrefabWithout;
        public GameObject HeartPrefab => _heartPrefab;
        public GameObject KeyPrefab => _keyPrefab;
        public GameObject[] EnemyPrefabs => _enemyPrefabs;
        public GameObject BossPrefab => _bossPrefab;
        public GameObject TeleportPrefab => _teleportPrefab;
        public GameObject LockDestroyEffect => _lockDestroyEffect;
        public GameObject BossLockDestroyEffect => _bossLockDestroyEffect;
        public GameObject BossKeyPrefab => _bossKeyPrefab;
        public GameObject BossLockPrefab => _bossLockPrefab;
        public GameObject DamageTextPrefab => _damageTextPrefab;
        public GameObject BombPrefabWith => _bombPrefabWith;
        public GameObject BombPrefabWithout => _bombPrefabWithout;

        [Header("Door Styles")]
        [SerializeField] private List<DoorStyle> _doorStyles; // ScriptableObject
        private Dictionary<string, DoorStyle> _doorStyleMap;


        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            InitializeDoorStyles();
        }

        private void InitializeDoorStyles()
        {
            _doorStyleMap = new Dictionary<string, DoorStyle>();

            foreach (var style in _doorStyles)
            {
                style.Initialize();
                _doorStyleMap[style.Name] = style;
            }
        }

        public GameObject GetDoorPrefab(string styleName, DoorType type, Direction direction)
        {
            //Debug.Log($"Входные данные: стиль = {styleName}, тип = {type}, направление = {direction}");

            if (_doorStyleMap.TryGetValue(styleName, out var style))
            {
                var door = style.GetDoor(type, direction);
                //Debug.Log($"Получен префаб: {door?.name ?? "null"}");
                return door;
            }

            //Debug.LogWarning($"Не найден стиль: {styleName}");
            return null;
        }
    }
}
