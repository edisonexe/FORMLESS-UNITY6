using UnityEngine;
using Formless.Core.Managers;

namespace Formless.Room
{
    public class DoorsController : MonoBehaviour
    {
        [SerializeField] private GameObject[] _doors;

        public void OpenRegularDoors()
        {
            Debug.Log("������ �������� ������");
            foreach (GameObject door in _doors)
            {

                Door doorComponent = door.GetComponent<Door>(); // ? MissingReferenceException: The object of type 'UnityEngine.GameObject' has been destroyed but you are still trying to access it.
                if (doorComponent != null && doorComponent.doorType == Door.DoorType.Regular)
                {
                    doorComponent.OpenDoor("Lock");
                }
            }

            GameplayManager.Instance.RoomCleared();
            Debug.Log("����� ���������");
        }

        public void TrySetKeyRequiredDoor()
        {
            if (GameplayManager.Instance.CanSetKeyRequiredDoor())
            {
                int randDoor = Random.Range(0,_doors.Length);
                Door currentDoor = _doors[randDoor].GetComponent<Door>();
                if (currentDoor != null && currentDoor.doorType != Door.DoorType.Boss) 
                {
                    currentDoor.doorType = Door.DoorType.KeyRequired;
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
