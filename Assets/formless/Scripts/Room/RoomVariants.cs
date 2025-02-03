using UnityEngine;
using System.Collections.Generic;

namespace Formless.Room
{
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

        private bool _isBossSpawned;

        public GameObject LastRoom => rooms.Count > 0 ? rooms[rooms.Count - 1] : null;

        private void RemoveSpawners(GameObject room)
        {
            Transform spawnEnemy = room.transform.Find("Spawners");
            if (spawnEnemy != null)
            {
                Destroy(spawnEnemy.gameObject);
            }
        }

        public void TrySpawnBoss(GameObject room)
        {
            if (_isBossSpawned)
            {
                return;
            }

            if (room != LastRoom)
            {
                return;
            }

            Debug.Log("Спавним босса в комнате: " + room.name);
            RemoveSpawners(room);
            Instantiate(boss, room.transform.position, Quaternion.identity);
            _isBossSpawned = true;
        }
    }
}
