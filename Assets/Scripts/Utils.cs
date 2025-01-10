using System.Collections;
using UnityEngine;

namespace Formless.Utils
{
    public static class FormlessUtils
    {
        public static Vector3 GetRandomDirection()
        {
            return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        }

        //public static IEnumerator FadeOutAndDestroy(GameObject targetObject, Material material)
        //{
        //    Debug.Log("����� � ��������");
        //    if (material == null)
        //    {
        //        Debug.LogError("Material is null! Cannot perform fade out.");
        //        yield break;
        //    }

        //    float pauseDuration = 1f; // ������������ �����
        //    float fadeDuration = 3f; // ������������ ���������
        //    Color originalColor = material.color;

        //    // ����� ����� ������� ���������
        //    yield return new WaitForSeconds(pauseDuration);

        //    float elapsedTime = 0f;

        //    while (elapsedTime < fadeDuration)
        //    {
        //        float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
        //        material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
        //        elapsedTime += Time.deltaTime;
        //        yield return null;
        //    }

        //    material.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        //    Object.Destroy(targetObject);
        //}

        public static IEnumerator FadeOutAndDestroy(GameObject targetObject, Material material)
        {
            Debug.Log("����� � ��������");

            if (material == null)
            {
                Debug.LogError("Material is null! Cannot perform fade out.");
                yield break;
            }

            float pauseDuration = 2f; // ������������ �����
            float fadeDuration = 3f; // ������������ ���������
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
            if (targetObject.transform.parent != null)
            {
                Object.Destroy(targetObject.transform.parent.gameObject);
            }
            else
            {
                Debug.LogError("Parent not found, destroying the target object instead.");
                Object.Destroy(targetObject);
            }
        }

    }


}
