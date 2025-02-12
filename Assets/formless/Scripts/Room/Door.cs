using UnityEngine;

namespace Formless.Room
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private GameObject wallPrefab;
        private BoxCollider2D _boxCollider2D;
        private bool _isProcessed = false;
        private bool _isReplaced = false;
        private bool _isOpened = false;

        private void Awake()
        {
            _boxCollider2D = GetComponent<BoxCollider2D>();
        }

        private void Start()
        {
            if (!_isOpened)
            {
                Invoke("DisableDoorBoxCollider", 5f);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (_isProcessed || _isOpened) return;

            if (collision.CompareTag("Wall"))
            {
                ReplaceDoorWithWall();
                _isProcessed = true;
            }
            //else
            //{
            //    _boxCollider2D.enabled = false;
            //}
        }

        public void OpenDoor()
        {
            _isOpened = true;
            _boxCollider2D.enabled = false;
        }

        private void ReplaceDoorWithWall()
        {
            Debug.Log(" олизи€ двери со стеной, замена на стену");

            if (!_isReplaced)
            {
                _boxCollider2D.enabled = false;
                Instantiate(wallPrefab, transform.position, Quaternion.identity, transform.parent);
                _isReplaced = true;
                Destroy(gameObject);
            }
        }

        private void DisableDoorBoxCollider()
        {
            _boxCollider2D.enabled = false;
        }
    }
}
