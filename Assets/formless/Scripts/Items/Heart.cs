using Formless.Core.Managers;
using UnityEngine;
using Formless.Player;

namespace Formless.Items
{
    public class Heart : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                Debug.Log("Игрок наступил на сердечко");
                var collectible = gameObject.GetComponent<Collectible>();
                if (collectible != null && collectible.isCollected) return;
            
                collectible.isCollected = true;
                Player.Player.Instance.AddHealth();
                Destroy(gameObject);
            }
        }
    }
}
