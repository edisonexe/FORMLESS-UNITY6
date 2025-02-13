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

        public GameObject HeartPrefab => _heartPrefab;
        public GameObject KeyPrefab => _keyPrefab;
        public GameObject[] EnemyPrefabs => _enemyPrefabs;
        public GameObject BossPrefab => _bossPrefab;
        public GameObject TeleportPrefab => _teleportPrefab;
        public GameObject LockDestroyEffect => _lockDestroyEffect;
        public GameObject BossLockDestroyEffect => _bossLockDestroyEffect;
        public GameObject BossKeyPrefab => _bossKeyPrefab;
        public GameObject BossLockPrefab => _bossLockPrefab;
    }
}
