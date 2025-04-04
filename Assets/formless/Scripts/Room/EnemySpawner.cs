using UnityEngine;
using System.Collections.Generic;
using Formless.Core.Managers;
using System;

namespace Formless.Room
{
    public class EnemySpawner : MonoBehaviour
    {
        public event Action<List<Enemy.Enemy>> OnEnemiesSpawned;
        
        [SerializeField] private Transform[] _enemySpawners;
        [SerializeField] private EnemySpawnData[] _enemySpawnData;

        private List<Enemy.Enemy> _spawnedEnemies = new List<Enemy.Enemy>();

        private void Start()
        {
            _enemySpawnData = PrefabManager.Instance.EnemyPrefabs;
        }

        public void Spawn()
        {
            _spawnedEnemies.Clear();
            
            foreach (Transform spawner in _enemySpawners)
            {
                if (spawner == null) continue;

                GameObject enemyType = GetRandomEnemyByProbability();
                GameObject enemyObj = Instantiate(enemyType, spawner.position, Quaternion.identity);
                enemyObj.transform.parent = transform;

                Enemy.Enemy enemy = enemyObj.GetComponent<Enemy.Enemy>();
                if (enemy != null)
                {
                    _spawnedEnemies.Add(enemy);
                }
            }
            
            OnEnemiesSpawned?.Invoke(_spawnedEnemies);
        }

        private GameObject GetRandomEnemyByProbability()
        {
            float totalWeight = 0f;

            foreach (var enemyData in _enemySpawnData)
            {
                if (enemyData.enemyPrefab == null)
                {
                    //Debug.LogWarning("Enemy prefab is null in EnemySpawnDataList!");
                    continue;
                }
                totalWeight += enemyData.spawnProbability;
            }


            float randomValue = UnityEngine.Random.Range(0f, totalWeight);
            //Debug.Log($"Сгенерированное случайное число: {randomValue}");

            foreach (var enemyData in _enemySpawnData)
            {
                if (randomValue < enemyData.spawnProbability)
                {
                    //Debug.Log($"Выбран враг: {enemyData.enemyPrefab.name}");
                    return enemyData.enemyPrefab;
                }

                randomValue -= enemyData.spawnProbability;
            }

            //Debug.LogError("Failed to select an enemy by probability. Returning null.");
            return null;
        }
    }
}
