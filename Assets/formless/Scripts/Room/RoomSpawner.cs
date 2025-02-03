using UnityEngine;

namespace Formless.Room
{
    public class RoomSpawner : MonoBehaviour
    {
        [SerializeField] private Direction _direction;
        private enum Direction
        {
            Top,
            Bottom,
            Left,
            Right,
            None
        }

        private RoomVariants _roomVariants;
        private int _randNumberOfRoom;
        private bool _isSpawned = false;
        private float _waitTime = 5f;

        private void Start()
        {
            _roomVariants = GameObject.FindGameObjectWithTag("RoomVariants").GetComponent<RoomVariants>();
            Destroy(gameObject, _waitTime);
            Invoke("SpawnRoom", 0.1f);
        }

        private void SpawnRoom()
        {
            if (!_isSpawned)
            {
                switch (_direction)
                {
                    case Direction.Top:
                        _randNumberOfRoom = Random.Range(0, _roomVariants.topRooms.Length);
                        Instantiate(_roomVariants.topRooms[_randNumberOfRoom], transform.position, Quaternion.identity);
                        break;
                    case Direction.Bottom:
                        _randNumberOfRoom = Random.Range(0, _roomVariants.bottomRooms.Length);
                        Instantiate(_roomVariants.bottomRooms[_randNumberOfRoom], transform.position, Quaternion.identity);
                        break;
                    case Direction.Left:
                        _randNumberOfRoom = Random.Range(0, _roomVariants.leftRooms.Length);
                        Instantiate(_roomVariants.leftRooms[_randNumberOfRoom], transform.position, Quaternion.identity);
                        break;
                    case Direction.Right:
                        _randNumberOfRoom = Random.Range(0, _roomVariants.rightRooms.Length);
                        Instantiate(_roomVariants.rightRooms[_randNumberOfRoom], transform.position, Quaternion.identity);
                        break;
                }

                _isSpawned = true;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.CompareTag("SpawnPointRoom"))
            {
                if (collision.GetComponent<RoomSpawner>()._isSpawned == false && _isSpawned == false)
                {
                    GameObject closedRoom = Instantiate(_roomVariants.closedRoom, transform.position, Quaternion.identity);
                    Debug.Log("Пустая комната");

                    // Удаляем объект через 5 секунд
                    Destroy(closedRoom, 5f);
                }
                _isSpawned = true;
            }
        }
    }
}