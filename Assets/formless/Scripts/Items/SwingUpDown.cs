using UnityEngine;
public class SwingUpDown : MonoBehaviour
{
    [SerializeField] private float amplitude = 0.5f;
    [SerializeField] private float speed = 2f;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        float newY = startPos.y + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(startPos.x, newY, startPos.z);
    }
}

