using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool IsGameOver { set; get; } = false;

    public int score;

    [SerializeField]
    private DailyRankRegister dailyRank;

    

    public void GameOver()
    {
        //�ߺ� ó�� ���� �ʵ��� bool ������ ����
        if (IsGameOver == true)
            return;

        IsGameOver = true;

        dailyRank.Process(score);

        //����ġ ���� �� ������ ���� �˻�
        //(���� ���� �ý��ۿ� ���� ������ ���� ������ ����ġ�� �ִ�ġ�� 100���� ����)
        //(������ �ѹ� �÷����� ������ ����ġ�� ���÷� 25�� ����)
        BackendGameData.Instance.UserGameData.experience += 25;
        if(BackendGameData.Instance.UserGameData.experience >= 100)
        {
            BackendGameData.Instance.UserGameData.experience = 0;
            BackendGameData.Instance.UserGameData.level++;
        }

        // ���� ���� ������Ʈ
        BackendGameData.Instance.GameDataUpdate(AfterGameOver);
    }

    public void AfterGameOver()
    {
        Utils.LoadScene(SceneNames.Lobby);
    }

    public void ScoreUp()
    {
        score++;
    }
}
