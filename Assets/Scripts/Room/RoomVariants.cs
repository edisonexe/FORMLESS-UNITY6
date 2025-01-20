using UnityEngine;
using System.Collections.Generic;


public class RoomVariants : MonoBehaviour
{
    [SerializeField] public GameObject[] topRooms;
    [SerializeField] public GameObject[] bottomRooms;
    [SerializeField] public GameObject[] leftRooms;
    [SerializeField] public GameObject[] rightRooms;

    [SerializeField] public GameObject closedRoom;

    public List<GameObject> rooms;

    [SerializeField] private float _waitTime;
    [SerializeField] private GameObject boss;
    private bool _IsBossSpawned;


    private void Update()
    {
        if (_waitTime<=0 && _IsBossSpawned == false)
        {
            for (int i = 0; i < rooms.Count; i++)
            {
                if (i == rooms.Count - 1)
                {
                    RemoveSpawners(rooms[i]);

                    Instantiate(boss, rooms[i].transform.position, Quaternion.identity);
                    _IsBossSpawned = true;
                }
            }
        }
        else if (_waitTime > 0)
        {
            _waitTime -= Time.deltaTime;
        }
    }

    private void RemoveSpawners(GameObject room)
    {
        // Найти дочерний объект с именем "SpawnEnemy"
        Transform spawnEnemy = room.transform.Find("Spawners");
        if (spawnEnemy != null)
        {
            Destroy(spawnEnemy.gameObject); // Удалить найденный объект
        }
    }

}
