using UnityEngine;
using Formless.Core.Managers;
using Formless.Player;
public class BossKey : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            var collectible = gameObject.GetComponent<Collectible>();
            if (collectible != null && collectible.isCollected) return;

            collectible.isCollected = true;
            Player.Instance.PickupBossKey();
            Destroy(gameObject);
        }
    }
}
