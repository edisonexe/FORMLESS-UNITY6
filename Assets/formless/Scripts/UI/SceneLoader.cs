using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Formless.Player;
public class SceneLoader : MonoBehaviour
{
    public GameObject loadingCanvas;
    public Image progressImage;
    public Text progressText;
    private DungeonGenerator _dungeonGenerator;
    private bool isGenerationComplete = false;
    public float fakeLoadSpeed = 0.2f;

    private void Start()
    {
        _dungeonGenerator = FindFirstObjectByType<DungeonGenerator>();

        if (_dungeonGenerator != null)
        {
            DungeonGenerator.OnDungeonFullGenerated += HandleGenerationComplete;
            if (loadingCanvas != null)
            {
                loadingCanvas.SetActive(true);
                StartCoroutine(FakeProgress());
            }
            DungeonGenerator.Instance.StartGenerating();
        }
    }

    private void HandleGenerationComplete()
    {
        isGenerationComplete = true;
    }

    private IEnumerator FakeProgress()
    {
        float progress = 0f;

        float range = Random.Range(0.65f, 0.85f);

        while (progress < range)
        {
            progress += Time.deltaTime * fakeLoadSpeed;
            progressImage.fillAmount = progress;
            progressText.text = Mathf.RoundToInt(progress * 100) + "%";
            yield return null;
        }

        while (!isGenerationComplete)
        {
            yield return null;
        }

        while (progress < 1f)
        {
            progress += Time.deltaTime * (fakeLoadSpeed * 2); // Ускоряем в 2 раза
            progressImage.fillAmount = progress;
            progressText.text = Mathf.RoundToInt(progress * 100) + "%";
            yield return null;
        }

        loadingCanvas.SetActive(false);

        Player.Instance.inputHandler.Enable();
        //Player.Instance.RCSetInputHandler();
        isGenerationComplete = false;
        progress = 0f;
    }



    private void OnDestroy()
    {
        if (_dungeonGenerator != null)
        {
            DungeonGenerator.OnDungeonFullGenerated -= HandleGenerationComplete;
        }
    }

}
