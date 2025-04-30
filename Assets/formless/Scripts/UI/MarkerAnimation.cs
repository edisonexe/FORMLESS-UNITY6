using UnityEngine;

namespace Formless.UI
{
    public class MarkerAnimation : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveDistance = 0.5f;
        [SerializeField] private float speed = 1.5f;

        private Vector3 startPosition;

        private void Start()
        {
            startPosition = transform.localPosition;
        }

        private void Update()
        {
            float newY = startPosition.y + Mathf.PingPong(Time.time * speed, moveDistance);

            transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);
        }

        public void SetStartPosition(Vector3 newPosition)
        {
            startPosition = newPosition;
            transform.localPosition = startPosition; // Обновляем текущую позицию
        }
    }
}