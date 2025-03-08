using UnityEngine;
namespace Formless.UI
{
    public class FadeScreen : MonoBehaviour
    {
        [SerializeField] Canvas _parentCanvas;
        private Canvas fadeScreenCanvas;

        void Start()
        {
            fadeScreenCanvas = GetComponent<Canvas>();

            fadeScreenCanvas.sortingLayerName = _parentCanvas.sortingLayerName;
            fadeScreenCanvas.sortingOrder = _parentCanvas.sortingOrder + 10;
        }
    }
}
