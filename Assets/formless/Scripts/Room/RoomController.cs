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
        private bool _itemWasSpawned = false;

        public bool ItemWasSpawned => _itemWasSpawned;

        private void Awake()
        {
            _roomStateChecker = GetComponent<RoomStateChecker>();
            _enemySpawner = GetComponent<EnemySpawner>();
            _itemSpawner = GetComponent<ItemSpawner>();
            _bossSpawner = new BossSpawner(PrefabManager.Instance.BossPrefab, PrefabManager.Instance.TeleportPrefab);
        }

        private void Start()
        {
            DungeonGenerator dungeonGenerator = GameplayManager.Instance.GetDungeonGenerator();
            if (dungeonGenerator != null)
            {
                dungeonGenerator.RegisterRoom(gameObject);
            }

            if (_enemySpawner != null)
            {
                _enemySpawner.OnEnemiesSpawned += OnEnemiesSpawned;
            }
            DungeonGenerator.OnDungeonGenerationCompleted += HandleRoomAfterDungeonGen;

        }

        private void OnDestroy()
        {
            DungeonGenerator.OnDungeonGenerationCompleted -= HandleRoomAfterDungeonGen;
        }

        public void SetItemWasSpawned()
        {
            _itemWasSpawned = true;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Игрок зашёл в комнату
            if (collision.CompareTag("Player") && !_isEnemiesSpawned)
            {
                _isEnemiesSpawned = true;
                GameplayManager.Instance.SetCurrentRoom(this);

                // Спавн босса в последней комнате
                if (DungeonGenerator.Instance.LastRoom != null && DungeonGenerator.Instance.LastRoom.transform == transform)
                {
                    _bossSpawner.TrySpawnBoss(gameObject);
                }
                // Обычная комната, спавн врагов и предметов
                else
                {
                    _enemySpawner.Spawn();
                }
            }
        }

        private void HandleRoomAfterDungeonGen()
        {
            if (transform == DungeonGenerator.Instance.LastRoom.transform)
            {
                RemoveObjectsWithTagInRoom(gameObject, "EnemySpawner");
                RemoveObjectsWithTagInRoom(gameObject, "ItemSpawner");
            }
            else
            {
                TrySpawnBossKeyInPenultimateRoom();
            }
        }

        private void OnEnemiesSpawned(List<Enemy.Enemy> enemies)
        {
            foreach (var enemy in enemies)
            {
                _roomStateChecker.AddEnemy(enemy);
            }
        }

        private void TrySpawnBossKeyInPenultimateRoom()
        {
            if (DungeonGenerator.Instance.PenultimateRoom != null && transform == DungeonGenerator.Instance.PenultimateRoom.transform)
            {
                _itemSpawner.SpawnKeyForPenultimateRoom();
            }
        }

        // ДОДЕЛАТЬ УДАЛЕНИЕ СПАУНЕРОВ ИЗ BOSS_ROOM
        void RemoveObjectsWithTagInRoom(GameObject room, string tagToRemove)
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tagToRemove);
    
            foreach (GameObject obj in objectsWithTag)
            {
                if (obj.transform.IsChildOf(room.transform))
                {
                    Destroy(obj);
                }
            }
        }

    }
}

