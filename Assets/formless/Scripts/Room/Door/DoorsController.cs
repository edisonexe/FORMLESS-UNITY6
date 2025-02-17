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
                if (doors[i] == oldDoor) // ������� ������ �����
                {
                    doors[i] = newDoor; // �������� � �� �����
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
                // �������� �� null ����� ����������
                if (door == null)
                {
                    continue; // ���������� ���� ������, ���� �� ��� ���������
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
