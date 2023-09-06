using UnityEngine;

public class GameController : MonoBehaviour
{
    public bool IsGameOver { set; get; } = false;

    public int score;

    [SerializeField]
    private DailyRankRegister dailyRank;

    

    public void GameOver()
    {
        //중복 처리 되지 않도록 bool 변수로 제어
        if (IsGameOver == true)
            return;

        //게임오버 되었을 때 호출할 메소드들을 실행
        IsGameOver = true;

        //현재 점수 정보를 바탕으로 랭킹 데이터 갱신
        dailyRank.Process(score);

        /*
        //경험치 증가 및 레벨업 여부 검사
        //(현재 레벨 시스템에 대한 설정이 없기 때문에 경험치의 최대치를 100으로 가정)
        //(게임을 한번 플레이할 때마다 경험치는 예시로 25씩 증가)
        BackendGameData.Instance.UserGameData.experience += 25;
        if(BackendGameData.Instance.UserGameData.experience >= 100)
        {
            BackendGameData.Instance.UserGameData.experience = 0;
            BackendGameData.Instance.UserGameData.level++;
        }

        // 게임 정보 업데이트
        BackendGameData.Instance.GameDataUpdate(AfterGameOver);
        */

        //로비로 다시 돌아감
        AfterGameOver();
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
