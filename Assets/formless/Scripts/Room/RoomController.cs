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

        private bool _isEnemiesSpawned;
        private bool _itemWasSpawned;
        private void Awake()
        {
            _roomStateChecker = GetComponent<RoomStateChecker>();
            _enemySpawner = GetComponent<EnemySpawner>();
            _itemSpawner = GetComponent<ItemSpawner>();
            _bossSpawner = new BossSpawner(PrefabManager.Instance.BossPrefab, PrefabManager.Instance.TeleportPrefab);
        }

        private void Start()
        {
            GameManager.Instance.rooms.Add(gameObject);

            if (_enemySpawner != null)
            {
                _enemySpawner.OnEnemiesSpawned += OnEnemiesSpawned;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") && !_isEnemiesSpawned)
            {
                _isEnemiesSpawned = true;
                GameManager.Instance.SetCurrentRoom(this);

                if (GameManager.Instance.PenultimateRoom != null && transform == GameManager.Instance.PenultimateRoom.transform)
                {
                    Debug.Log("Игрок вошел в предпоследнюю комнату");
                    _itemSpawner.SpawnKeyForPenultimateRoom();
                    _itemWasSpawned = true;
                }

                if (GameManager.Instance.LastRoom != null && GameManager.Instance.LastRoom.transform == transform)
                {
                    // Спавним босса через BossSpawner
                    _bossSpawner.TrySpawnBoss(gameObject);  // Переносим логику спавна босса сюда
                }
                else
                {
                    _enemySpawner.Spawn();
                    if (!_itemWasSpawned)
                        _itemSpawner.Spawn();
                }
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

