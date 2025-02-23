using UnityEngine;
using Formless.Core.Managers;
using System.Linq;

namespace Formless.Room
{
    public class DoorsController : MonoBehaviour
    {
        [SerializeField] private GameObject[] doors;
        public GameObject[] Doors => doors;

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
                if (door == null) continue;

                Door doorComponent = door.GetComponent<Door>();
                if (doorComponent != null && doorComponent.DoorType == DoorType.Regular)
                {
                    doorComponent.OpenDoor(LockConstants.REGULAR_LOCK);
                }
            }

            GameplayManager.Instance.RoomCleared();
        }
    }
}
