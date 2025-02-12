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
        
        private List<Enemy.Enemy> _spawnedEnemies = new List<Enemy.Enemy>();

        public void Spawn()
        {
            _spawnedEnemies.Clear();
            
            foreach (Transform spawner in _enemySpawners)
            {
                if (spawner == null) continue;

                GameObject enemyType = PrefabManager.Instance.EnemyPrefabs[UnityEngine.Random.Range(0, PrefabManager.Instance.EnemyPrefabs.Length)];
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
    }
}
