using System.Collections.Generic;
using Formless.Core.Managers;
using Formless.UI;
using UnityEngine;

namespace Formless.Boss
{
    public class BossSpawner
    {
        private GameObject _teleportPrefab;
        private bool _isBossSpawned;
        private Queue<GameObject> _bossQueue;

        public BossSpawner(GameObject[] bossPrefabs, GameObject teleportPrefab)
        {
            _bossQueue = new Queue<GameObject>(ShuffleArray(bossPrefabs));
            _teleportPrefab = teleportPrefab;
        }

        private GameObject[] ShuffleArray(GameObject[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int randomIndex = Random.Range(i, array.Length);
                (array[i], array[randomIndex]) = (array[randomIndex], array[i]);
            }
            return array;
        }

        public void TrySpawnBoss(GameObject room)
        {
            if (_isBossSpawned || _bossQueue.Count == 0) return;

            GameObject selectedBossPrefab = _bossQueue.Dequeue();
            GameObject bossObj = Object.Instantiate(selectedBossPrefab, room.transform.position, Quaternion.identity);

            Debug.Log("Спавним босса в комнате: " + room.name);

            bossObj.transform.SetParent(room.transform);

            var boss = bossObj.GetComponent<Boss>();
            if (boss != null)
            {
                boss.OnDie += HandleDeathBoss;
            }

            _isBossSpawned = true;
        }

        public void HandleDeathBoss(Boss boss)
        {
            if (DungeonGenerator.Instance.CountDungeons == DungeonGenerator.Instance.DungeonsToVictory)
            {
                GameplayManager.Instance.endPanel.SetupPanel(GameResult.Victory);
                return;
            }

            Transform roomTransform = boss.transform.parent;

            if (roomTransform != null)
            {
                Transform floor = roomTransform.Find("Floor");

                if (floor != null)
                {
                    Vector3 bossPosition = boss.transform.position;

                    GameObject teleportObj = Object.Instantiate(_teleportPrefab, bossPosition, Quaternion.identity);
                    teleportObj.transform.SetParent(floor);  // Привязываем телепорт к Floor

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
