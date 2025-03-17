using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal; 

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

            // Пауза перед началом затухания
            yield return new WaitForSeconds(pauseDuration);

            float elapsedTime = 0f;

            // Плавное исчезновение
            while (elapsedTime < fadeDuration)
            {
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                material.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            material.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);

            targetObject.SetActive(false);

            //Object.Destroy(targetObject);
        }

        public static IEnumerator FadeLight(Light2D lightSource, float fadeDuration)
        {
            if (lightSource == null)
            {
                //Debug.LogError("FadeLight: Light source is null!");
                yield break;
            }

            float startIntensity = lightSource.intensity;
            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                lightSource.intensity = Mathf.Lerp(startIntensity, 0f, elapsedTime / fadeDuration);
                //Debug.Log($"FadeLight: intensity = {lightSource.intensity}");

                elapsedTime += Time.deltaTime;
                yield return null;
            }

            lightSource.intensity = 0f;
        }
    }
}
