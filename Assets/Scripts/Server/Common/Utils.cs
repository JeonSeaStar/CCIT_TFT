using UnityEngine.SceneManagement;

public enum SceneNames { Logo = 0, Login, Lobby, Tile}

/// <summary>
/// 씬 전환과 같은 유틸 메소드를 정의하는 스크립트
/// </summary>
public static class Utils
{
    public static string GetActiveScene()
    {
        return SceneManager.GetActiveScene().name;
    }

    public static void LoadScene(string sceneName = "")
    {
        if (sceneName == "")
        {
            SceneManager.LoadScene(GetActiveScene());
        }
        else
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public static void LoadScene(SceneNames sceneName)
    {
        SceneManager.LoadScene(sceneName.ToString());
    }

}
