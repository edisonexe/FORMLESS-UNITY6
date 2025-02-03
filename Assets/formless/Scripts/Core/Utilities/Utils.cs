using System.Collections;
using UnityEngine;

namespace Formless.Core.Utilties
{
    public static class Utils
    {
        public static IEnumerator FadeOutAndDestroy(GameObject targetObject, Material material)
        {
            if (material == null)
            {
                Debug.LogError("Material is null! Cannot perform fade out.");
                yield break;
            }

            float pauseDuration = 2f;
            float fadeDuration = 3f;
            Color originalColor = material.color;

            // ����� ����� ������� ���������
            yield return new WaitForSeconds(pauseDuration);

            float elapsedTime = 0f;

            // ������� ������������
            while (elapsedTime < fadeDuration)
            {
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            material.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

            // ������� ������������ ������, � �� ��������
            //if (targetObject.transform.parent != null)
            //{
            //    Object.Destroy(targetObject.transform.parent.gameObject);
            //}
            //else
            //{
            //    Object.Destroy(targetObject);
            //}
            Object.Destroy(targetObject);
        }
    }
}
