using Formless.Player;
using UnityEngine;

public class FootstepsSound : MonoBehaviour
{
    public AudioClip footstepClip; // ���� �����
    public float stepInterval = 0.5f; // �������� ����� ������
    private AudioSource audioSource;
    private float stepTimer;
    private bool isMoving;

    private void Awake()
    {
        // ��������� ��������� AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = footstepClip;
        audioSource.volume = 0.5f;
        audioSource.loop = false; // ������ ��� ��������������� ��������
    }

    private void Update()
    {
        // ���������, �������� �� �����
        isMoving = Player.Instance.IsMoving; // ����������� ����� ��� �������� ��������

        if (isMoving)
        {
            stepTimer += Time.deltaTime;

            // ���� ������ ���������� �������, ������������� ���� ����
            if (stepTimer >= stepInterval)
            {
                PlayFootstepSound();
                stepTimer = 0f; // ���������� ������
            }
        }
        else
        {
            stepTimer = 0f; // ���������� ������, ���� ����� �� ��������
        }
    }

    private void PlayFootstepSound()
    {
        if (footstepClip != null)
        {
            audioSource.PlayOneShot(footstepClip); // ������������� ���� ����
        }
    }
}