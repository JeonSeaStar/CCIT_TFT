using UnityEngine;

/// <summary>
/// Lobby 씬을 로드할 때 서버로부터 유저 정보 불러오기
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
