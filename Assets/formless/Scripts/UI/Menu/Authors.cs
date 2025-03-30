using UnityEngine;

namespace Formless.UI.Menu
{
    public class Authors : MonoBehaviour
    {
        [SerializeField] private GameObject _authorsPanel;

        public void BackToMenu()
        {
            _authorsPanel.SetActive(false);
        }
    }
}

