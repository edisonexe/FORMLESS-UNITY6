using Formless.Core.Managers;
using Formless.Items;
using Formless.Player;
using UnityEngine;

namespace Formless.Player
{
    public class UseItem : MonoBehaviour
    {

        void Update()
        {
            if (Player.Instance.IsUseBombPressed() && Player.Instance.BombCount > 0)
            {
                Player.Instance.UseBomb();

                GameObject bomb = Instantiate(PrefabManager.Instance.BombPrefabWithout, Player.Instance.transform.position, Quaternion.identity);

                bomb.GetComponent<Bomb>().ActivateBombPrepare();
            }
        }
    }
}

