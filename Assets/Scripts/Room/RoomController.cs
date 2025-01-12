using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RoomController : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private GameObject[] _enemyTypes;
    [SerializeField] private Transform[] _enemySpawners;

    [Header("Items")]
    [SerializeField] private GameObject _heart;
    [SerializeField] private GameObject _key;

    private List<GameObject> _enemies;
    private RoomVariants _roomVariants;
    private bool _isSpawnedEnemies;

    private void Start()
    {
        _roomVariants = GameObject.FindGameObjectWithTag("Room").GetComponent<RoomVariants>();
        _enemies = new List<GameObject>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_isSpawnedEnemies)
        {
            _isSpawnedEnemies = true;

            foreach (Transform spawner in _enemySpawners)
            {
                Debug.Log($"Спавн по точке: {spawner.name} ({spawner.position})");
                int rand = Random.Range(0, 11);
                if (rand < 9)
                {
                    GameObject enemyType = _enemyTypes[Random.Range(0, _enemyTypes.Length)];
                    GameObject enemy = Instantiate(enemyType, spawner.position, Quaternion.identity) as GameObject;
                    enemy.transform.parent = transform;
                    _enemies.Add(enemy);
                }
                else if (rand == 9)
                {
                    Instantiate(_heart, spawner.position, Quaternion.identity);
                }
                else if (rand == 10)
                {
                    Instantiate(_key, spawner.position, Quaternion.identity);
                }
            }
            StartCoroutine(CheckEnemies());
        }
    }

    IEnumerator CheckEnemies()
    {
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => _enemies.Count == 0);
        DestroyLocks();
    }

    private void DestroyLocks()
    {
        Debug.Log("Замки открылись");
    }
}
