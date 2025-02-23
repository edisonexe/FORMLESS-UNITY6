using UnityEngine;
using Formless.Core.Managers;

namespace Formless.Room
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] _itemSpawners;
        private RoomController _roomController;

        private void Awake()
        {
            _roomController = GetComponent<RoomController>();
        }

        public void SpawnKey()
        {
            Debug.Log("Спавн ключа");

            SpawnItemWithPrefab(PrefabManager.Instance.KeyPrefab);
            DungeonGenerator.Instance.RegisterKey();
        }

        public void SpawnHeart()
        {
            Debug.Log("Спавн сердечка");

            SpawnItemWithPrefab(PrefabManager.Instance.HeartPrefab);
            DungeonGenerator.Instance.RegisterHeart();
        }

        public void SpawnKeyForPenultimateRoom()
        {
            Debug.Log("Спавн ключа для комнаты босса");

            SpawnItemWithPrefab(PrefabManager.Instance.BossKeyPrefab);
        }

        public void SpawnItemWithPrefab(GameObject prefab)
        {
            foreach (Transform spawner in _itemSpawners)
            {
                if (spawner == null) continue;
                GameObject itemObj = Instantiate(prefab, spawner.position, Quaternion.identity);
                itemObj.transform.parent = transform;
                _roomController.SetItemWasSpawned();
                Destroy(spawner.gameObject);
            }
        }
    }
}
