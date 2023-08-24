using System.Collections.Generic;
using System.Text;
using UnityEngine;

// �ڳ� SDK namespace �߰�
using BackEnd;

public class BackendRank
{

    private static BackendRank _instance = null;

    public static BackendRank Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new BackendRank();
            }

            return _instance;
        }
    }

    public void RankInsert(int score)
    {
        //'������ UUID ��' �� '�ڳ� �ܼ� > ��ŷ ����'���� ������ ��ŷ�� UUID������ �������ּ���.
        string rankUUID = "c440b190-4213-11ee-8302-6fce818a121b";

        string tableName = "USER_DATA";
        string rowInData = string.Empty;

        //��ŷ�� �����ϱ� ���ؼ��� ���� �����Ϳ��� ����ϴ� �������� inDate���� �ʿ��մϴ�.
        //���� �����͸� �ҷ��� ��, �ش� �������� inDate���� �����ϴ� �۾��� �ؾ��մϴ�.
        Debug.Log("������ ��ȸ�� �õ��մϴ�.");
        var bro = Backend.GameData.GetMyData(tableName, new Where());

        if(bro.IsSuccess() == false)
        {
            Debug.LogError("������ ��ȸ �� ������ �߻��߽��ϴ�.");
            return;
        }

        Debug.Log("������ ��ȸ�� �����߽��ϴ�. : " + bro);

        if(bro.FlattenRows().Count <= 0)
        {
            rowInData = bro.FlattenRows()[0]["inDate"].ToString();
        }
        else
        {
            Debug.Log("�����Ͱ� �������� �ʽ��ϴ�. ������ ������ �õ��մϴ�.");
            var bro2 = Backend.GameData.Insert(tableName);

            if (bro2.IsSuccess() == false)
            {
                Debug.LogError("������ ���� �� ������ �߻��߽��ϴ� : " + bro2);
                return;
            }

            Debug.Log("������ ���Կ� �����߽��ϴ� : " + bro2);

            rowInData = bro2.GetInDate();
        }

        Debug.Log("�� ���� ������ rowInDate : " + rowInData);

        Param param = new Param();
        param.Add("level", score);

        //����� rowInDate�� ���� �����Ϳ� param���� ������ �����ϰ� ��ŷ�� �����͸� ������Ʈ �մϴ�.
        Debug.Log("��ŷ ������ �õ��մϴ�.");
        var rankBro = Backend.URank.User.UpdateUserScore(rankUUID, tableName, rowInData, param);

        if (rankBro.IsSuccess() == false)
        {
            Debug.LogError("��ŷ ��� �� ������ �߻��߽��ϴ�. : " + rankBro);
            return;
        }

        Debug.Log("��ŷ ���Կ� �����߽��ϴ�. : " + rankBro);
    }

    public void RankGet()
    {
        // Step 3. ��ŷ �ҷ����� ���� �߰�
    }
}