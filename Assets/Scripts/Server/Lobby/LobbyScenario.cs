using UnityEngine;

/// <summary>
/// Lobby ���� �ε��� �� �����κ��� ���� ���� �ҷ�����
/// </summary>
public class LobbyScenario : MonoBehaviour
{
    [SerializeField] private UserInfo user;

    [SerializeField] private SceneNames nextScene;

    private void Awake()
    {
        user.GetUserInfoFromBackend();
    }

    private void Start()
    {
        BackendGameData.Instance.GameDataLoad();
    }

    public void OnAfterProgress()
    {
        Utils.LoadScene(SceneNames.Tile);
    }
}
