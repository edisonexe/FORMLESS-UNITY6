using UnityEngine.SceneManagement;

public class SceneChecker
{
    public bool IsCurrentScene(string sceneName)
    {
        string currentSceneName = SceneManager.GetActiveScene().name;

        return SceneManager.GetActiveScene().name == sceneName;
    }
}