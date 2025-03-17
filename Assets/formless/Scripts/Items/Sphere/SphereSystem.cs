using System.Collections.Generic;
using Formless.Core.Managers;
using UnityEngine;

namespace Formless.Items
{
    public class SphereSystem : MonoBehaviour
    {
        [SerializeField] private float orbitRadius = 2f;
        [SerializeField] private float orbitSpeed = 100f;
        [SerializeField] private int orbitResolution = 100;

        private List<GameObject> orbs = new List<GameObject>();
        private LineRenderer lineRenderer;

        private BoxCollider2D boxCollider;

        void Start()
        {
            // �������� BoxCollider2D ����������
            boxCollider = GetComponent<BoxCollider2D>();

            // ������� � ����������� LineRenderer
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.positionCount = orbitResolution; // ���������� �����
            lineRenderer.startWidth = 0.05f; // ������� �����
            lineRenderer.endWidth = 0.05f; // ������� �����
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.green;
            lineRenderer.endColor = Color.green;
        }

        void Update()
        {
            UpdateOrbPositions();
            //DrawOrbit();
        }

        public void AddOrb()
        {
            GameObject newOrb = Instantiate(PrefabManager.Instance.SpherePrefab, transform.position, Quaternion.identity);
            orbs.Add(newOrb);
            float cooldown = 0.5f;
            //float cooldown = CalculateDamageCooldown();
            newOrb.GetComponent<Sphere>().SetDamageCooldown(cooldown);
        }

        private void UpdateOrbPositions()
        {
            // ��������� ������� ����
            for (int i = 0; i < orbs.Count; i++)
            {
                if (orbs[i] != null)
                {
                    float angle = (orbitSpeed * Time.time + i * 360f / orbs.Count) % 360f;
                    float angleRad = angle * Mathf.Deg2Rad;

                    // �������� ����� ������� � ������ BoxCollider2D
                    Vector3 center = transform.position + (Vector3)boxCollider.offset;

                    // ������������ ����� ������� ����
                    Vector3 newPosition = new Vector3(Mathf.Cos(angleRad) * orbitRadius, Mathf.Sin(angleRad) * orbitRadius, 0f);
                    orbs[i].transform.position = center + newPosition;
                }
            }
        }

        private void DrawOrbit()
        {
            // ��������� ����� ��� ������
            for (int i = 0; i < orbitResolution; i++)
            {
                float angle = i * (360f / orbitResolution);
                float angleRad = angle * Mathf.Deg2Rad;

                // �������� ����� ������� � ������ BoxCollider2D
                Vector3 center = transform.position + (Vector3)boxCollider.offset;

                // ���������� ����� ������
                Vector3 orbitPoint = new Vector3(Mathf.Cos(angleRad) * orbitRadius, Mathf.Sin(angleRad) * orbitRadius, 0f);

                // ������������� ������� ����� ��� ��������� ������
                lineRenderer.SetPosition(i, center + orbitPoint);
            }
        }

        private float CalculateDamageCooldown()
        {
            return 360f / orbitSpeed;
        }
    }
}
