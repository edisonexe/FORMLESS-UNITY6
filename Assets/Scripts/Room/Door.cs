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

    private void Awake()
    {
        _boxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_isProcessed) return;

        if (collision.CompareTag("Wall"))
        {
            ReplaceDoorWithWall();
            _isProcessed = true;
        }
    }

    private void ReplaceDoorWithWall()
    {
        Debug.Log(" олизи€ двери со стеной, замена на стену");

        if (!_isReplaced)
        {
            Instantiate(wallPrefab, transform.position, Quaternion.identity, transform.parent);
            _boxCollider2D.enabled = false;
            _isReplaced = true;
            Destroy(gameObject);
        }
    }
}
