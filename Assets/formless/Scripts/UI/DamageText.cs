using TMPro;
using UnityEngine;

namespace Formless.UI
{
    public class DamageText : MonoBehaviour
    {
        private float _lifeTime = 1f;
        private float _speedMoveUp = 1f;
        private TextMeshPro _textMesh;
        private MeshRenderer _meshRenderer;

        public void Initialize(float damage)
        {
            _textMesh = GetComponent<TextMeshPro>();
            _meshRenderer = GetComponent<MeshRenderer>();

            _textMesh.text = "-" + damage.ToString();
            _meshRenderer.sortingOrder = 10;

            Destroy(gameObject, _lifeTime);
        }

        private void Update()
        {
            transform.position += Vector3.up * _speedMoveUp * Time.deltaTime;

            Color textColor = _textMesh.color;
            textColor.a -= Time.deltaTime / _lifeTime;
            _textMesh.color = textColor;
        }
    }
}

