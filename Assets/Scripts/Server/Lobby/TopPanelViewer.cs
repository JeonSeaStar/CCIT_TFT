using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// TOP Panel에 출력하는 UI 정보를 제어하는 스크립트
/// 서버에서 불러온 유저 정보 출력(레벨, 경험치 등)
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
        //닉네임이 없으면 gamer_id를 출력하고, 닉네임이 있으면 닉네임 출력
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
