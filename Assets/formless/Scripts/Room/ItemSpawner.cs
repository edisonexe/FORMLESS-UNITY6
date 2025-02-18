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

                int rand = Random.Range(1, 4);
                //Debug.LogFormat("ItemSpawner Rand = {0}", rand);
                if (rand == 1 && DungeonGenerator.Instance.CanSpawnHeart())
                {
                    //Debug.Log("����� ��������");
                    Instantiate(PrefabManager.Instance.HeartPrefab, spawner.position, Quaternion.identity);
                    DungeonGenerator.Instance.RegisterHeart();
                }
                else if (rand == 2 && DungeonGenerator.Instance.CanSpawnKey())
                {
                    //Debug.Log("����� �����");
                    Instantiate(PrefabManager.Instance.KeyPrefab, spawner.position, Quaternion.identity);
                    DungeonGenerator.Instance.RegisterKey();
                }

                //Debug.Log("CanSpawnKey "+ DungeonGenerator.Instance.CanSpawnKey() + "CanSpawnHeart " + DungeonGenerator.Instance.CanSpawnHeart());
            }
        }

        public void SpawnKeyForPenultimateRoom()
        {
            foreach (Transform spawner in _itemSpawners)
            {
                if (spawner == null) continue;

                Instantiate(PrefabManager.Instance.BossKeyPrefab, spawner.position, Quaternion.identity);
            }
            //Debug.Log("����� ����� ��� ������������� �������");
        }
    }
}
