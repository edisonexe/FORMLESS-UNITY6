using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] private Direction _direction;
    private BoxCollider2D _boxCollider2D;
    private bool _isProcessed = false;
    private bool _isReplaced = false;
    private enum Direction
    {
        Top,
        Bottom,
        Left,
        Right
    }

    [SerializeField] private GameObject wallPrefab;

    private void Start()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isProcessed) return;
        // Проверяем, если объект столкновения имеет тег "Wall" или аналогичный тег для тайлмапа стен
        if (collision.CompareTag("Wall"))
        {
            ReplaceDoorWithWall();
        }
    }

    private void ReplaceDoorWithWall()
    {
        Debug.Log("ФЛИП");

        if (!_isReplaced)
        {
            Instantiate(wallPrefab, transform.position, Quaternion.identity, transform.parent);
            _boxCollider2D.enabled = false;
            _isReplaced = true;
            Destroy(gameObject);
        }
    }
}
