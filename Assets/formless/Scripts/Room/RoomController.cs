using UnityEngine;
using System.Collections.Generic;
using Formless.Core.Managers;
using Formless.Boss;

namespace Formless.Room
{
    public class RoomController : MonoBehaviour
    {
        [Header("Components")]
        private RoomStateChecker _roomStateChecker;
        private EnemySpawner _enemySpawner;
        private ItemSpawner _itemSpawner;
        private BossSpawner _bossSpawner;
        private DoorsController _doorsController;

        private bool _isEnemiesSpawned;
        private bool _itemWasSpawned = false;
        private void Awake()
        {
            _roomStateChecker = GetComponent<RoomStateChecker>();
            _enemySpawner = GetComponent<EnemySpawner>();
            _itemSpawner = GetComponent<ItemSpawner>();
            _bossSpawner = new BossSpawner(PrefabManager.Instance.BossPrefab, PrefabManager.Instance.TeleportPrefab);
            _doorsController = GetComponent<DoorsController>();
        }

        private void Start()
        {
            GameplayManager.Instance.rooms.Add(gameObject);

            if (_enemySpawner != null)
            {
                _enemySpawner.OnEnemiesSpawned += OnEnemiesSpawned;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Игрок зашёл в комнату
            if (collision.CompareTag("Player") && !_isEnemiesSpawned)
            {
                _isEnemiesSpawned = true;
                GameplayManager.Instance.SetCurrentRoom(this);

                // Спавн ключа к боссу в предпоследней комнате
                if (GameplayManager.Instance.PenultimateRoom != null && transform == GameplayManager.Instance.PenultimateRoom.transform)
                {
                    Debug.Log("Игрок вошел в предпоследнюю комнату");
                    _itemSpawner.SpawnKeyForPenultimateRoom();
                    _itemWasSpawned = true;
                }

                // Спавн босса в последней комнате
                if (GameplayManager.Instance.LastRoom != null && GameplayManager.Instance.LastRoom.transform == transform)
                {
                    _bossSpawner.TrySpawnBoss(gameObject);
                }
                else
                {
                    // Обычная комната, спавн врагов и предметов
                    _enemySpawner.Spawn();
                    if (!_itemWasSpawned)
                        _itemSpawner.Spawn();
                }
                _doorsController.TrySetKeyRequiredDoor();
            }
        }

        private void OnEnemiesSpawned(List<Enemy.Enemy> enemies)
        {
            foreach (var enemy in enemies)
            {
                _roomStateChecker.AddEnemy(enemy);
            }
        }
    }
}

