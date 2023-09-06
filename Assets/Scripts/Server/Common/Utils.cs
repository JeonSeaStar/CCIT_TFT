using UnityEngine.SceneManagement;

public enum SceneNames { Logo = 0, Login, Lobby, Tile, Game}

/// <summary>
/// �� ��ȯ�� ���� ��ƿ �޼ҵ带 �����ϴ� ��ũ��Ʈ
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