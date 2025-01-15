using UnityEngine;

public class DestroyerPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SpawnPointRoom"))
        {
            Destroy(collision.gameObject);
        }
    }
}
