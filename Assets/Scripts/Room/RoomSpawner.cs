using UnityEngine;

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

    // Глобальный счётчик и лимит
    private static int _roomCount = 0;
    private static int _maxRooms = 6;

    private void Start()
    {
        _roomVariants = GameObject.FindGameObjectWithTag("RoomVariants").GetComponent<RoomVariants>();
        Destroy(gameObject, _waitTime);
        Invoke("SpawnRoom", 0.5f);
    }

    private void SpawnRoom()
    {
        //Debug.Log($"Spawning room in direction: {_direction}");

        // Проверяем, не превышен ли лимит комнат
        if (_isSpawned || _roomCount >= _maxRooms)
        {
            Destroy(gameObject); // Удаляем спавнер, если лимит превышен
            return;
        }

        _isSpawned = true;
        _roomCount++; // Увеличиваем счётчик комнат

        // Спавним комнату в зависимости от направления
        if (_direction == Direction.Top)
        {
            _randNumberOfRoom = Random.Range(0, _roomVariants.topRooms.Length);
            Instantiate(_roomVariants.topRooms[_randNumberOfRoom], transform.position, _roomVariants.topRooms[_randNumberOfRoom].transform.rotation);
        }
        else if (_direction == Direction.Bottom)
        {
            _randNumberOfRoom = Random.Range(0, _roomVariants.bottomRooms.Length);
            Instantiate(_roomVariants.bottomRooms[_randNumberOfRoom], transform.position, _roomVariants.bottomRooms[_randNumberOfRoom].transform.rotation);
        }
        else if (_direction == Direction.Left)
        {
            _randNumberOfRoom = Random.Range(0, _roomVariants.leftRooms.Length);
            Instantiate(_roomVariants.leftRooms[_randNumberOfRoom], transform.position, _roomVariants.leftRooms[_randNumberOfRoom].transform.rotation);
        }
        else if (_direction == Direction.Right)
        {
            _randNumberOfRoom = Random.Range(0, _roomVariants.rightRooms.Length);
            Instantiate(_roomVariants.rightRooms[_randNumberOfRoom], transform.position, _roomVariants.rightRooms[_randNumberOfRoom].transform.rotation);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("RoomPoint") && collision.GetComponent<RoomSpawner>()._isSpawned)
        {
            _isSpawned = true;
            _roomCount -= 1;
            Destroy(gameObject);
        }
    }
}
