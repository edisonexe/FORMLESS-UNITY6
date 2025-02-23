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

        //public void Spawn()
        //{
        //    foreach (Transform spawner in _itemSpawners)
        //    {
        //        if (spawner == null) continue;

        //        int rand = Random.Range(1, 3);
        //        //Debug.LogFormat("ItemSpawner Rand = {0}", rand);
        //        if (rand == 1 && DungeonGenerator.Instance.CanSpawnHeart())
        //        {
        //            //Debug.Log("Спаун сердечка");
        //            Instantiate(PrefabManager.Instance.HeartPrefab, spawner.position, Quaternion.identity);
        //            DungeonGenerator.Instance.RegisterHeart();
        //        }
        //        //else if (rand == 2 && DungeonGenerator.Instance.CanSpawnKey())
        //        //{
        //        //    //Debug.Log("Спаун ключа");
        //        //    Instantiate(PrefabManager.Instance.KeyPrefab, spawner.position, Quaternion.identity);
        //        //    DungeonGenerator.Instance.RegisterKey();
        //        //}

        //        //Debug.Log("CanSpawnKey "+ DungeonGenerator.Instance.CanSpawnKey() + "CanSpawnHeart " + DungeonGenerator.Instance.CanSpawnHeart());
        //        Destroy(spawner.gameObject);
        //    }
        //}

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
                Instantiate(prefab, spawner.position, Quaternion.identity);
                _roomController.SetItemWasSpawned();
                Destroy(spawner.gameObject);
            }
        }
    }
}
