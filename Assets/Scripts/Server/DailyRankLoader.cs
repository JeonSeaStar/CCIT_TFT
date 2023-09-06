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
        //rankUuid ��ŷ ���̺� �ִ� ������ offset ~ offset + limit ���� ��ŷ ������ �ҷ���
        Backend.URank.User.GetRankList(Constants.DAILY_RANK_UUID, Constants.MAX_RANK_LIST, callback =>
        {
            if(callback.IsSuccess())
            {
                //JSON ������ �Ľ� ����
                try
                {
                    Debug.Log($"��ŷ ��ȸ�� �����߽��ϴ�. : {callback}");

                    LitJson.JsonData rankDateJson = callback.FlattenRows();

                    //�޾ƿ� �������� ������ 0�̸� �����Ͱ� ���� ��
                    if(rankDataList.Count <= 0)
                    {
                        // 1~20������ �����͸� �� �����ͷ� ����
                        for(int i = 0; i < Constants.MAX_RANK_LIST; ++i)
                        {
                            SetRankData(rankDataList[i], i + 1, "-", 0);
                        }

                        Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
                    }
                    else
                    {
                        int rankerCount = rankDateJson.Count;

                        //��ŷ ������ �ҷ��� ����� �� �ֵ��� ����
                        for(int i =0; i < rankerCount; i++)
                        {
                            rankDataList[i].Rank = int.Parse(rankDateJson[i]["rank"].ToString());
                            rankDataList[i].Score = int.Parse(rankDateJson[i]["score"].ToString());


                            //�г����� ������ �������� �ʴ� ������ ������ �� �ֱ� ������ �г����� �������� �ʴ� ������ �г��� ��� gamerId�� ���
                            rankDataList[i].Nickname = rankDateJson[i].ContainsKey("nickname") == true ? rankDateJson[i]["nickname"]?.ToString() : UserInfo.Data.gamerId;
                        }
                        //���� limitCount�� ������ ���ں��� ���� ��ŷ�� ��ϵ� ���ڰ� ������ �������� �� ������ ����
                        for (int i = rankerCount; i < Constants.MAX_RANK_LIST; ++i)
                        {
                            SetRankData(rankDataList[i], i + 1, "-", 0);
                        }
                    }
                }
                //JSON ������ �Ľ� ����
                catch(System.Exception e)
                {
                    //try-catch ���� ���
                    Debug.LogError(e);
                }
            }
            else
            {
                // 1 ~ 20������ �����͸� �� �����ͷ� ����
                for(int i = 0; i < Constants.MAX_RANK_LIST; ++i)
                {
                    SetRankData(rankDataList[i], i + 1, "-", 0);
                }

                Debug.LogError($"��ŷ ��ȸ �� ������ �߻��߽��ϴ�. : {callback}");
            }
        });
    }

    private void GetMyRank()
    {
        // �� ��ŷ ���� �ҷ�����
        Backend.URank.User.GetMyRank(Constants.DAILY_RANK_UUID, callback =>
        {
            //�г����� ������ gameId, �г����� ������ nickname ���
            string nickname = UserInfo.Data.nickname == null ? UserInfo.Data.gamerId : UserInfo.Data.nickname;

            if (callback.IsSuccess())
            {
                //JSON ������ �Ľ� ����
                try
                {
                    LitJson.JsonData rankDataJson = callback.FlattenRows();

                    //�޾ƿ� �������� ������ 0�̸� �����Ͱ� ���� ��
                    if (rankDataJson.Count <= 0)
                    {
                        //["������ ����","�г���", 0]�� ���� ���
                        SetRankData(myRankData, 1000000000, nickname, 0);

                        Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
                    }
                    else
                    {
                        myRankData.Rank = int.Parse(rankDataJson[0]["rank"].ToString());
                        myRankData.Score = int.Parse(rankDataJson[0]["score"].ToString());

                        //�г����� ������ �����ϴ� ���� ������ ������ �� �ֱ� ������ �г����� �������� �ʴ� ������ �г��� ��� gameId�� ���
                        myRankData.Nickname = rankDataJson[0].ContainsKey("nickname") == true ? rankDataJson[0]["nickname"]?.ToString() : UserInfo.Data.gamerId;
                    }
                }
                //�ڽ��� ��ŷ ���� JSON ������ �Ľ̿� �������� ��
                catch (System.Exception e)
                {
                    //["������ ����","�г���",0]�� ���� ���
                    SetRankData(myRankData, 1000000000, nickname, 0);

                    //try-catch ���� ���
                    Debug.LogError(e);
                }
            }
            else
            {
                //�ڽ��� ��ŷ ���� �����Ͱ� �������� ���� ��
                if (callback.GetMessage().Contains("userRank"))
                {
                    //["������ ����","�г���",0]�� ���� ���
                    SetRankData(myRankData, 1000000000, nickname, 0);
                }
            }
        });
    }

    private void SetRankData(DailyRankData rankData, int rank, string nickname, int score)
    {
        rankData.Rank = rank;
        rankData.Nickname = nickname;
        rankData.Score = score;
    }
}
