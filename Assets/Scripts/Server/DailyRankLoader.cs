using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd;


public class DailyRankLoader : MonoBehaviour
{
    [SerializeField] private GameObject rankDataPrefab;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] private Transform rankDataParent;
    [SerializeField] private DailyRankData myRankData;

    private List<DailyRankData> rankDataList;

    private void Awake()
    {
        rankDataList = new List<DailyRankData>();

        //1~20위 랭킹 출력을 위한 UI 오브젝트 생성
        for(int i = 0; i < Constants.MAX_RANK_LIST; i++)
        {
            GameObject clone = Instantiate(rankDataPrefab, rankDataParent);
            rankDataList.Add(clone.GetComponent<DailyRankData>());
        }
    }

    private void OnEnable()
    {
        // 1위 랭킹이 보이도록 scroll 값 설정
        scrollbar.value = 1;
        // 1 ~ 20위의 랭킹 정보 불러오기
        GetRankList();
        // 내 랭킹 정보 불러오기
        GetMyRank();
    }

    private void GetRankList()
    {
        //1~20위 랭킹 정보 불러오기
    }

    private void GetMyRank()
    {

    }
}
