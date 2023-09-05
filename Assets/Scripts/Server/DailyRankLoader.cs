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

        //1~20�� ��ŷ ����� ���� UI ������Ʈ ����
        for(int i = 0; i < Constants.MAX_RANK_LIST; i++)
        {
            GameObject clone = Instantiate(rankDataPrefab, rankDataParent);
            rankDataList.Add(clone.GetComponent<DailyRankData>());
        }
    }

    private void OnEnable()
    {
        // 1�� ��ŷ�� ���̵��� scroll �� ����
        scrollbar.value = 1;
        // 1 ~ 20���� ��ŷ ���� �ҷ�����
        GetRankList();
        // �� ��ŷ ���� �ҷ�����
        GetMyRank();
    }

    private void GetRankList()
    {
        //1~20�� ��ŷ ���� �ҷ�����
    }

    private void GetMyRank()
    {

    }
}
