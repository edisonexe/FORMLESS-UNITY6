using Unity.VisualScripting;
using UnityEngine;

namespace Formless.Boss
{
    public class BossSpawner
    {
        private GameObject _teleportPrefab;
        private GameObject _bossPrefab;
        private bool _isBossSpawned;

        public BossSpawner(GameObject bossPrefab, GameObject teleportPrefab)
        {
            _bossPrefab = bossPrefab;
            _teleportPrefab = teleportPrefab;
        }

        public void TrySpawnBoss(GameObject room)
        {
            if (_isBossSpawned) return;

            Debug.Log("Спавним босса в комнате: " + room.name);
            //RemoveSpawners(room);

            // Спавним босса внутри комнаты на её позиции (или в нужной точке внутри)
            GameObject bossObj = Object.Instantiate(_bossPrefab, room.transform.position, Quaternion.identity);

            // Делаем босса дочерним объектом комнаты
            bossObj.transform.SetParent(room.transform);

            var boss = bossObj.GetComponent<Boss>();
            if (boss != null)
            {
                boss.OnDie += SpawnTeleport;
            }

            _isBossSpawned = true;
        }

        //private void RemoveSpawners(GameObject room)
        //{
        //    foreach (Transform spawner in room.transform)
        //    {
        //        if (spawner.CompareTag("EnemySpawner") || spawner.CompareTag("ItemSpawner"))
        //        {
        //            Object.Destroy(spawner.gameObject);
        //        }
        //    }
        //}

        public void SpawnTeleport(Boss boss)
        {
            Transform roomTransform = boss.transform.parent;

            if (roomTransform != null)
            {
                Transform floor = roomTransform.Find("Floor");

                if (floor != null)
                {
                    // Получаем позицию босса
                    Vector3 bossPosition = boss.transform.position;

                    // Создаем телепорт на позиции босса, но делаем его дочерним объектом Floor
                    GameObject teleportObj = Object.Instantiate(_teleportPrefab, bossPosition, Quaternion.identity);
                    teleportObj.transform.SetParent(floor);  // Привязываем телепорт к Floor

                    // Убедимся, что масштаб телепорта правильный
                    teleportObj.transform.localScale = Vector3.one; // Восстанавливаем нормальный масштаб

                    Debug.Log("Телепорт создан на позиции босса, внутри Floor");
                }
                else
                {
                    Debug.LogError("Не найден объект Floor в комнате");
                }
            }
        }
    }
}
