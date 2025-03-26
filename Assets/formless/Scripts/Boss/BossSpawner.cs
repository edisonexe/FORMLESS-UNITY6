using Unity.VisualScripting;
using UnityEngine;

namespace Formless.Boss
{
    public class BossSpawner
    {
        private GameObject _teleportPrefab;
        private GameObject _bossPrefab;
        private bool _isBossSpawned;

        public BossSpawner(GameObject bossPrefab, GameObject teleportPrefab)
        {
            _bossPrefab = bossPrefab;
            _teleportPrefab = teleportPrefab;
        }

        public void TrySpawnBoss(GameObject room)
        {
            if (_isBossSpawned) return;

            Debug.Log("������� ����� � �������: " + room.name);
            //RemoveSpawners(room);

            // ������� ����� ������ ������� �� � ������� (��� � ������ ����� ������)
            GameObject bossObj = Object.Instantiate(_bossPrefab, room.transform.position, Quaternion.identity);

            // ������ ����� �������� �������� �������
            bossObj.transform.SetParent(room.transform);

            var boss = bossObj.GetComponent<Boss>();
            if (boss != null)
            {
                boss.OnDie += SpawnTeleport;
            }

            _isBossSpawned = true;
        }

        //private void RemoveSpawners(GameObject room)
        //{
        //    foreach (Transform spawner in room.transform)
        //    {
        //        if (spawner.CompareTag("EnemySpawner") || spawner.CompareTag("ItemSpawner"))
        //        {
        //            Object.Destroy(spawner.gameObject);
        //        }
        //    }
        //}

        public void SpawnTeleport(Boss boss)
        {
            Transform roomTransform = boss.transform.parent;

            if (roomTransform != null)
            {
                Transform floor = roomTransform.Find("Floor");

                if (floor != null)
                {
                    // �������� ������� �����
                    Vector3 bossPosition = boss.transform.position;

                    // ������� �������� �� ������� �����, �� ������ ��� �������� �������� Floor
                    GameObject teleportObj = Object.Instantiate(_teleportPrefab, bossPosition, Quaternion.identity);
                    teleportObj.transform.SetParent(floor);  // ����������� �������� � Floor

                    // ��������, ��� ������� ��������� ����������
                    teleportObj.transform.localScale = Vector3.one; // ��������������� ���������� �������

                    Debug.Log("�������� ������ �� ������� �����, ������ Floor");
                }
                else
                {
                    Debug.LogError("�� ������ ������ Floor � �������");
                }
            }
        }
    }
}
