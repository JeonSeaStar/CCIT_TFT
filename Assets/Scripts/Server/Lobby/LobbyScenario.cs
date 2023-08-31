using UnityEngine;

/// <summary>
/// Lobby ���� �ε��� �� �����κ��� ���� ���� �ҷ�����
/// </summary>
public class LobbyScenario : MonoBehaviour
{
    [SerializeField] private UserInfo user;

    private void Awake()
    {
        user.GetUserInfoFromBackend();
    }

    private void Start()
    {
        BackendGameData.Instance.GameDataLoad();
    }
}
