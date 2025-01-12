using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.Tilemaps;

public class RoomController : MonoBehaviour
{
    [Header("Enemies")]
    [SerializeField] private GameObject[] _enemyTypes;
    [SerializeField] private Transform[] _enemySpawners;

    [Header("Items")]
    [SerializeField] private GameObject _heart;
    [SerializeField] private GameObject _key;

    [Header("Doors")]
    [SerializeField] private GameObject[] _doors;

    [Header("Effects")]
    [SerializeField] private GameObject _lockDestroyEffect;

    private List<Enemy> _enemies;
    private RoomVariants _roomVariants;
    private bool _isSpawnedEnemies;

    private void Start()
    {
        _roomVariants = GameObject.FindGameObjectWithTag("Room").GetComponent<RoomVariants>();
        _enemies = new List<Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_isSpawnedEnemies)
        {
            _isSpawnedEnemies = true;

            foreach (Transform spawner in _enemySpawners)
            {
                int rand = Random.Range(0, 11);
                if (rand < 9)
                {
                    GameObject enemyType = _enemyTypes[UnityEngine.Random.Range(0, _enemyTypes.Length)];
                    GameObject enemyObj = Instantiate(enemyType, spawner.position, Quaternion.identity);
                    enemyObj.transform.parent = transform;

                    Enemy enemy = enemyObj.GetComponent<Enemy>();
                    if (enemy != null)
                    {
                        _enemies.Add(enemy);
                        enemy.OnDie += Enemy_OnDie;
                    }
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

    private void Enemy_OnDie(object sender, System.EventArgs e)
    {
        Enemy enemy = sender as Enemy;
        _enemies.Remove(enemy);
        enemy.OnDie -= Enemy_OnDie;
    }

    IEnumerator CheckEnemies()
    {
        yield return new WaitForSeconds(1f);
        yield return new WaitUntil(() => _enemies.Count == 0);
        DestroyLocks();
    }

    private void DestroyLocks()
    {
        foreach (GameObject door in _doors)
        {
            Transform lockObject = door.transform.Find("Lock");
            if (lockObject != null)
            {
                Instantiate(_lockDestroyEffect, lockObject.position, Quaternion.identity);
                Destroy(lockObject.gameObject);
            }

            Transform moverObject = door.transform.Find("Mover");
            if (moverObject != null)
            {
                moverObject.gameObject.SetActive(true);
            }

            TilemapCollider2D collider = door.GetComponent<TilemapCollider2D>();
            if (collider != null)
            {
                collider.enabled = false; // Отключаем коллайдер
            }
        }

        Debug.Log("Замки открылись");
    }
}
