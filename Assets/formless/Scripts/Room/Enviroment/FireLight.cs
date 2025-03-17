using UnityEngine;
using UnityEngine.Rendering.Universal; // ��� Light2D


namespace Formless.Room.Enviroment
{
    public class FireLight : MonoBehaviour
    {
        [SerializeField] private Light2D fireLight;
        [SerializeField] private float radiusVariation = 0.2f; // ��������� ���������� ������
        [SerializeField] private float flickerSpeed = 0.5f; // �������� ��������

        private float baseRadius;

        void Start()
        {
            if (fireLight == null)
            {
                fireLight = GetComponent<Light2D>();
            }

            baseRadius = fireLight.pointLightOuterRadius;
        }

        void Update()
        {
            float radiusOffset = Mathf.Sin(Time.time * flickerSpeed) * radiusVariation;
            fireLight.pointLightOuterRadius = baseRadius + radiusOffset;
        }
    }
}
