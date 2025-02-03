using UnityEngine;

namespace Formless.Room
{
    public class DestroyerPoint : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("SpawnPointRoom"))
            {
                Debug.Log("�������� ������ main");
                Destroy(collision.gameObject);
            }
        }
    }
}
