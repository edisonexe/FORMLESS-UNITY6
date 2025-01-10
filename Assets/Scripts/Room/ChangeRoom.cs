using UnityEngine;

public class ChangeRoom : MonoBehaviour
{

    [SerializeField] private Vector3 _cameraChangePosition;
    [SerializeField] private Vector3 _playerChangePosition;
    private Camera _camera;
    private bool _hasTeleported = false;

    private void Start()
    {
        _camera = Camera.main.GetComponent<Camera>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !_hasTeleported)
        {
            collision.transform.position += _playerChangePosition;
            _camera.transform.position += _cameraChangePosition;
            _hasTeleported = true;
            Debug.LogFormat("Teleport Player to {0}", collision.transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            _hasTeleported = false;  // —брасываем флаг при выходе игрока из коллайдера
        }
    }

}
