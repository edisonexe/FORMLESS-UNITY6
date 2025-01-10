using UnityEngine;

public class DoorCrossing : MonoBehaviour
{
    [SerializeField] private GameObject _leftWallPrefab;  // Префаб стены для двери слева
    [SerializeField] private GameObject _rightWallPrefab; // Префаб стены для двери справа

    [SerializeField] private bool _isLeftDoor; // Флаг для определения стороны двери

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Wall"))
        {
            ReplaceWithWall();
        }
    }

    private void ReplaceWithWall()
    {
        // Выбираем префаб в зависимости от стороны двери
        GameObject selectedWallPrefab = _isLeftDoor ? _leftWallPrefab : _rightWallPrefab;

        // Создаём выбранную стену на месте двери
        Instantiate(selectedWallPrefab, transform.position, Quaternion.identity);

        // Удаляем текущий объект (дверь)
        Destroy(gameObject);
    }
}

