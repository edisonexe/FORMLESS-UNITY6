using Formless.Player;
using UnityEngine;

public class FootstepsSound : MonoBehaviour
{
    public AudioClip footstepClip; // Звук шагов
    public float stepInterval = 0.5f; // Интервал между шагами
    private AudioSource audioSource;
    private float stepTimer;
    private bool isMoving;

    private void Awake()
    {
        // Добавляем компонент AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = footstepClip;
        audioSource.volume = 0.5f;
        audioSource.loop = false; // Каждый шаг воспроизводится отдельно
    }

    private void Update()
    {
        // Проверяем, движется ли игрок
        isMoving = Player.Instance.IsMoving; // Используйте метод для проверки движения

        if (isMoving)
        {
            stepTimer += Time.deltaTime;

            // Если прошло достаточно времени, воспроизводим звук шага
            if (stepTimer >= stepInterval)
            {
                PlayFootstepSound();
                stepTimer = 0f; // Сбрасываем таймер
            }
        }
        else
        {
            stepTimer = 0f; // Сбрасываем таймер, если игрок не движется
        }
    }

    private void PlayFootstepSound()
    {
        if (footstepClip != null)
        {
            audioSource.PlayOneShot(footstepClip); // Воспроизводим звук шага
        }
    }
}