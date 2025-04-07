using UnityEngine;

namespace Formless.UI
{
    public class MarkerAnimation : MonoBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float moveDistance = 0.5f; // Расстояние движения вверх-вниз
        [SerializeField] private float speed = 1.5f; // Скорость движения

        private Vector3 startPosition; // Начальная позиция маркера
        private bool movingUp = true; // Направление движения

        private void Start()
        {
            // Сохраняем начальную позицию маркера
            startPosition = transform.localPosition;
        }

        private void Update()
        {
            // Вычисляем новую позицию маркера
            float newY = startPosition.y + Mathf.PingPong(Time.time * speed, moveDistance);

            // Обновляем позицию маркера относительно родителя
            transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);
        }
    }
}