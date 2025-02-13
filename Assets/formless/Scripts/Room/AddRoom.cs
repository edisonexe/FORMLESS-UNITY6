using UnityEngine;
using Formless.Core.Managers;

namespace Formless.Room
{
    public class AddRoom : MonoBehaviour
    {
        private void Start()
        {
            GameplayManager.Instance.rooms.Add(gameObject);
        }
    }
}

