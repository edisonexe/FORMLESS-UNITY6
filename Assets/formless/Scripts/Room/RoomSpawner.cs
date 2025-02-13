using UnityEngine;
using Formless.Core.Managers;

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

        private int _randNumberOfRoom;
        private bool _isSpawned = false;
        private float _waitTime = 5f;

        private void Start()
        {
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
                        _randNumberOfRoom = Random.Range(0, GameplayManager.Instance.topRooms.Length);
                        Instantiate(GameplayManager.Instance.topRooms[_randNumberOfRoom], transform.position, Quaternion.identity);
                        break;
                    case Direction.Bottom:
                        _randNumberOfRoom = Random.Range(0, GameplayManager.Instance.bottomRooms.Length);
                        Instantiate(GameplayManager.Instance.bottomRooms[_randNumberOfRoom], transform.position, Quaternion.identity);
                        break;
                    case Direction.Left:
                        _randNumberOfRoom = Random.Range(0, GameplayManager.Instance.leftRooms.Length);
                        Instantiate(GameplayManager.Instance.leftRooms[_randNumberOfRoom], transform.position, Quaternion.identity);
                        break;
                    case Direction.Right:
                        _randNumberOfRoom = Random.Range(0, GameplayManager.Instance.rightRooms.Length);
                        Instantiate(GameplayManager.Instance.rightRooms[_randNumberOfRoom], transform.position, Quaternion.identity);
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
                    GameObject closedRoom = Instantiate(GameplayManager.Instance.closedRoom, transform.position, Quaternion.identity);
                    Debug.Log("������ �������");

                    // ������� ������ ����� 5 ������
                    Destroy(closedRoom, 5f);
                }
                _isSpawned = true;
            }
        }
    }
}