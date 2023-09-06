using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// TOP Panel�� ����ϴ� UI ������ �����ϴ� ��ũ��Ʈ
/// �������� �ҷ��� ���� ���� ���(����, ����ġ ��)
/// </summary>
public class TopPanelViewer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textNickname;
    [SerializeField] private TextMeshProUGUI textLevel;
    [SerializeField] private Slider sliderExperience;
    [SerializeField] private TextMeshProUGUI textJewel;
    [SerializeField] private TextMeshProUGUI textGold;

    private void Awake()
    {
        BackendGameData.Instance.onGameDataLoadEvent.AddListener(UpdateGameData);
    }

    public void UpdateNickname()
    {
        //�г����� ������ gamer_id�� ����ϰ�, �г����� ������ �г��� ���
        textNickname.text = UserInfo.Data.nickname == null ? UserInfo.Data.gamerId : UserInfo.Data.nickname;
    }

    public void UpdateGameData()
    {
        int currentLevel = BackendGameData.Instance.UserGameData.level;

        textLevel.text = currentLevel.ToString();
        sliderExperience.value = BackendGameData.Instance.UserGameData.experience / BackendChartData.levelChart[currentLevel-1].maxExperience;
        textGold.text = $"{BackendGameData.Instance.UserGameData.gold}";
        textJewel.text = $"{BackendGameData.Instance.UserGameData.jewel}";

    }
}
