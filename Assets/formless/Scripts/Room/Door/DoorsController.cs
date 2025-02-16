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
            //Debug.Log("Кол-во дверей в комнате " +  doors.Length);
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
            Debug.Log("Замки открылись");
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
                    Debug.LogWarning("Дверь к боссу не может стать KeyRequired!");
                }
            }
            else
            {
                Debug.LogWarning("Не заспавнено ни одного ключа на локации");
            }
        }
    }
}
