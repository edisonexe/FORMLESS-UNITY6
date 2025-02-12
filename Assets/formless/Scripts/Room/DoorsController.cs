using UnityEngine;
using UnityEngine.Tilemaps;
using Formless.Core.Managers;

namespace Formless.Room
{
    public class DoorsController : MonoBehaviour
    {
        [SerializeField] private GameObject[] _doors;

        public void OpenDoors()
        {
            Debug.Log("Начало удаления замков");
            foreach (GameObject door in _doors)
            {

                Door doorComponent = door.GetComponent<Door>();
                if (doorComponent != null)
                {
                    doorComponent.OpenDoor();
                }

                Transform lockObject = door.transform.Find("Lock");
                if (lockObject != null)
                {
                    // Получаем префаб эффекта из GameManager
                    GameObject lockDestroyEffect = PrefabManager.Instance.LockDestroyEffect;
                    if (lockDestroyEffect != null)
                    {
                        Instantiate(lockDestroyEffect, lockObject.position, Quaternion.identity);
                    }
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
                    collider.enabled = false;
                }
            }

            GameManager.Instance.RoomCleared();
            Debug.Log("Замки открылись");
        }
    }
}
