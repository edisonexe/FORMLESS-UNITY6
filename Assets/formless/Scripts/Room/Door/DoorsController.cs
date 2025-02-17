using UnityEngine;
using Formless.Core.Managers;
using System.Linq;

namespace Formless.Room
{
    public class DoorsController : MonoBehaviour
    {
        [SerializeField] public GameObject[] doors;

        private void Start()
        {
            InitializeDoors();
        }

        private void OnEnable()
        {
            Door.OnDoorReplaced += HandleDoorReplaced;
        }

        private void OnDisable()
        {
            Door.OnDoorReplaced -= HandleDoorReplaced;
        }

        private void HandleDoorReplaced(GameObject oldDoor, GameObject newDoor)
        {
            for (int i = 0; i < doors.Length; i++)
            {
                if (doors[i] == oldDoor) // Находим старую дверь
                {
                    doors[i] = newDoor; // Заменяем её на новую
                    return;
                }
            }
        }

        private void InitializeDoors()
        {
            doors = GetComponentsInChildren<Door>().Select(d => d.gameObject).ToArray();
        }

        public void OpenRegularDoors()
        {
            foreach (GameObject door in doors)
            {
                // Проверка на null перед обращением
                if (door == null)
                {
                    continue; // Пропускаем этот объект, если он был уничтожен
                }

                Door doorComponent = door.GetComponent<Door>();
                if (doorComponent != null && doorComponent.doorType == DoorType.Regular)
                {
                    doorComponent.OpenDoor(LockConstants.REGULAR_LOCK);
                }
            }

            GameplayManager.Instance.RoomCleared();
        }
    }
}
