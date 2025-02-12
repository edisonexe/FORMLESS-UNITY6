using UnityEngine;
using Formless.Core.Managers;

namespace Formless.Room
{
    public class ItemSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] _itemSpawners;

        public void Spawn()
        {
            foreach (Transform spawner in _itemSpawners)
            {
                if (spawner == null) continue;

                int rand = Random.Range(1, 6);

                if (rand == 1 && GameManager.Instance.CanSpawnHeart())
                {
                    Debug.Log("Спаун сердечка");
                    Instantiate(PrefabManager.Instance.HeartPrefab, spawner.position, Quaternion.identity);
                    GameManager.Instance.RegisterHeart();
                }
                else if (rand == 2 && GameManager.Instance.CanSpawnKey())
                {
                    Debug.Log("Спаун ключа");
                    Instantiate(PrefabManager.Instance.KeyPrefab, spawner.position, Quaternion.identity);
                    GameManager.Instance.RegisterKey();
                }
            }
        }

        public void SpawnKeyForPenultimateRoom()
        {
            foreach (Transform spawner in _itemSpawners)
            {
                if (spawner == null) continue;

                Instantiate(PrefabManager.Instance.BossKeyPrefab, spawner.position, Quaternion.identity);
            }
            Debug.Log("Спавн ключа для предпоследней комнаты");
        }
    }
}
