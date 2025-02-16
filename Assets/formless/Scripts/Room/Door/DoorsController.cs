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
            //Debug.Log("���-�� ������ � ������� " +  doors.Length);
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
            Debug.Log("����� ���������");
        }


        public void TrySetKeyRequiredDoor()
        {
            if (GameplayManager.Instance.CanSetKeyRequiredDoor())
            {
                int randDoor = Random.Range(0,doors.Length);
                Door currentDoor = doors[randDoor].GetComponent<Door>();
                if (currentDoor != null && currentDoor.doorType != DoorType.Boss) 
                {
                    currentDoor.doorType = DoorType.KeyRequired;
                    Debug.Log("KeyRequired Door SET");
                }
                else
                {
                    Debug.LogWarning("����� � ����� �� ����� ����� KeyRequired!");
                }
            }
            else
            {
                Debug.LogWarning("�� ���������� �� ������ ����� �� �������");
            }
        }
    }
}
