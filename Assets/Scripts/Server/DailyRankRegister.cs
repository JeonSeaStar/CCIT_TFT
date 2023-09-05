using UnityEngine;
using BackEnd;

/// <summary>
/// ���� �ְ� ������ ��ŷ�� ����ϴ� ��ũ��Ʈ %% ��ŷ �׽�Ʈ������ �����ѰŶ� ���� ���ɼ� ����
/// </summary>
public class DailyRankRegister : MonoBehaviour
{
    public void Process(int newScore)
    {
        UpdateMyRankData(newScore);

    }

    private void UpdateMyRankData(int newScore)
    {
        string rowInDatae = string.Empty;

        //��ŷ �����и� ������Ʈ�Ϸ��� ���� �����Ϳ��� ����ϴ� �������� inDate ���� �ʿ�
        Backend.GameData.GetMyData(Constants.USER_DATA_TABLE, new Where(), callback =>
        {
            if(!callback.IsSuccess())
            {
                Debug.LogError($"������ ��ȸ �� ������ �߻��߽��ϴ�. : {callback}");
                return;
            }

            Debug.Log($"������ ��ȸ�� �����߽��ϴ�. : {callback}");

            if(callback.FlattenRows().Count > 0)
            {
                rowInDatae = callback.FlattenRows()[0]["inDate"].ToString();
            }
            else
            {
                Debug.LogError("�����Ͱ� �������� �ʽ��ϴ�.");
                return;
            }

            Param param = new Param()
            {
                {"dailyBestScore", newScore }
            };

            //�ڳ� tableName ���̺��� rowInDate row �����͸� param ������ �����ϰ�, rankUuid ��Ű ���̺��� ��ŷ ���� ����
            Backend.URank.User.UpdateUserScore(Constants.DAILY_RANK_UUID, Constants.USER_DATA_TABLE, rowInDatae, param, callback =>
            {
                if(callback.IsSuccess())
                {
                    Debug.Log($"��ŷ ��Ͽ� �����߽��ϴ�. {callback}");
                }
                else
                {
                    Debug.LogError($"��ŷ ��� �� ������ �߻��߽��ϴ�. : {callback}");
                }
            });
        });
    }

    private void UpdateMyBestRankData(int newScore)
    {
        //rankUuid ��ŷ ���̺� ��ϵǾ� �ִ� �� ��ŷ ��ȸ, gap���� �����ϸ� �� ��ŷ ��/�Ʒ� ���� ���� �˻� ���� ex)gap�� 3�϶��� �� ��ŷ +3 -3 ������ ���� �˻� ����
        Backend.URank.User.GetMyRank(Constants.DAILY_RANK_UUID, callback =>
        {
            if(callback.IsSuccess())
            {
                //JSON ������ �Ľ� ����
                try
                {
                    LitJson.JsonData rankDataJson = callback.FlattenRows();

                    //�޾ƿ� �������� ������ 0�̸� �����Ͱ� ���� ��
                    if(rankDataJson.Count <= 0)
                    {
                        Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
                    }
                    else
                    {
                        //��ŷ�� ����� ���� �÷����� "dailyBestScore"�� ����������
                        //��ŷ�� �ҷ��� ���� �÷����� "score"�� ���ϵǾ� �ִ�.

                        //�߰��� ����� �׸��� �÷����� �׷��� ���
                        int bestScore = int.Parse(rankDataJson[0]["score"].ToString());

                        //���� ������ �ְ� �������� ������
                        if(newScore > bestScore)
                        {
                            //���� ������ ���ο� �ְ� ������ �����ϰ�, ��ŷ�� ���
                            UpdateMyRankData(newScore);

                            Debug.Log($"�ְ� ���� ���� {bestScore} -> {newScore}");
                        }
                    }
                }
                //JSON ������ �Ľ� ����
                catch(System.Exception e)
                {
                    Debug.LogError(e);
                }
            }
            else
            {
                //�ڽ��� ��ŷ ������ �������� ���� �۴� ���� ������ ���ο� ��ŷ���� ���
                if(callback.GetMessage().Contains("userRank"))
                {
                    UpdateMyRankData(newScore);

                    Debug.Log($"���ο� ��ŷ ������ ���� �� ��� : {callback}");
                }
            }
        });
    }
}
