using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


//�ڳ� SDK namespace �߰�
using BackEnd;

public class UserData
{
    public int level = 1;
    public int money1 = 0; //���� ���̾�
    public int money2 = 0; //���� ���̾�
    public string info = string.Empty;
    public Dictionary<string, int> inventory = new Dictionary<string, int>();
    public List<string> equipment = new List<string>();

    //�����͸� ������ϱ� ���� �Լ��Դϴ�. (Debug.Log(UserData);)
    public override string ToString()
    {
        StringBuilder result = new StringBuilder();
        result.AppendLine($"level : {level}");
        result.AppendLine($"money1 : {money1}");
        result.AppendLine($"money2 : {money2}");
        result.AppendLine($"info : {info}");
        result.AppendLine($"inventory");
        foreach (var itemKey in inventory.Keys)
        {
            result.AppendLine($"| {itemKey} : {inventory[itemKey]}��");
        }

        result.AppendLine($"equipment");
        foreach(var equip in equipment)
        {
            result.AppendLine($"| {equip}");
        }

        return result.ToString();
    }
}



public class BackendGameData : MonoBehaviour
{
    private static BackendGameData _instance = null;

    public static BackendGameData Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new BackendGameData();
            }

            return _instance;
        }
    }

    public static UserData userData;

    private string gameDataRowInDate = string.Empty;

    public void GameDataInsert() //�������� ����
    {
        if (userData == null)
        {
            userData = new UserData();
        }

        Debug.Log("�����͸� �ʱ�ȭ�մϴ�.");
        userData.level = 1;
        userData.money1 = 0;
        userData.money2 = 0;
        userData.info = "Null";

        userData.equipment.Add("�����");
        userData.equipment.Add("�⺻ ������");

        userData.inventory.Add("�̺�Ʈ ��ȭ", 0);


        Debug.Log("�ڳ� ������Ʈ ��Ͽ� �ش� �����͵��� �߰��մϴ�.");
        Param param = new Param();
        param.Add("level", userData.level);
        param.Add("money1", userData.money1);
        param.Add("money2", userData.money2);
        param.Add("info", userData.info);
        param.Add("equipment", userData.equipment);
        param.Add("inventory", userData.inventory);


        Debug.Log("�������� ������ ������ ��û�մϴ�");
        var bro = Backend.GameData.Insert("USER_DATA", param);

        if(bro.IsSuccess())
        {
            Debug.Log("�������� ������ ���Կ� �����߽��ϴ�. : " + bro);

            //������ ���������� �������Դϴ�.
            gameDataRowInDate = bro.GetInDate();
        }
        else
        {
            Debug.LogError("�������� ������ ���Կ� �����߽��ϴ�. : " + bro);
        }
    }


    public void GameDataGet() //�������� �ҷ�����
    {
        Debug.Log(".���� ���� ��ȸ �Լ��� ȣ���մϴ�.");
        var bro = Backend.GameData.GetMyData("USER_DATA", new Where());
        if(bro.IsSuccess())
        {
            Debug.Log("���� ���� ��ȸ�� �����߽��ϴ�. : " + bro);
            LitJson.JsonData gameDataJson = bro.FlattenRows(); // Json���� ���ϵ� �����͸� �޾ƿɴϴ�.

            // �޾ƿ� �������� ������ 0�̶�� �����Ͱ� �������� �ʴ� ���Դϴ�.
            if (gameDataJson.Count <= 0)
            {
                Debug.LogWarning("�����Ͱ� �������� �ʽ��ϴ�.");
            }
            else
            {
                gameDataRowInDate = gameDataJson[0]["inDate"].ToString(); //�ҷ��� ���������� �������Դϴ�.

                userData = new UserData();

                userData.level = int.Parse(gameDataJson[0]["level"].ToString());
                userData.money1 = int.Parse(gameDataJson[0]["money1"].ToString());
                userData.money2 = int.Parse(gameDataJson[0]["money2"].ToString());
                userData.info = gameDataJson[0]["info"].ToString();

                foreach (string itemKey in gameDataJson[0]["inventory"].Keys)
                {
                    userData.inventory.Add(itemKey, int.Parse(gameDataJson[0]["inventory"][itemKey].ToString()));
                }

                foreach (LitJson.JsonData equip in gameDataJson[0]["equipment"])
                {
                    userData.equipment.Add(equip.ToString());
                }

                Debug.Log(userData.ToString());
            }
        }
        else
        {
            Debug.LogError("���� ���� ��ȸ�� �����߽��ϴ�. : " + bro);
        }
    }
    

    public void LevelUp() //�������� ����
    {
        Debug.Log("������ 1 ������ŵ�ϴ�.");
        userData.level += 1;
        userData.money1 += 100;
        userData.money2 += 200;
        userData.info = "������ �����մϴ�.";
    }

    public void GameDataUpdate() //�������� ���� ������Ʈ
    {
        if(userData == null)
        {
            Debug.LogError("�������� �ٿ�ްų� ���� ������ �����Ͱ� �������� �ʽ��ϴ�. Insert Ȥ�� Get�� ���� �����͸� �������ּ���.");
            return;
        }

        Param param = new Param();

        param.Add("level", userData.level);
        param.Add("money1", userData.money1);
        param.Add("money2", userData.money2);
        param.Add("info", userData.info);
        param.Add("equipment", userData.equipment);
        param.Add("inventory", userData.inventory);

        BackendReturnObject bro = null;

        if(string.IsNullOrEmpty(gameDataRowInDate))
        {
            Debug.Log("�� ���� �ֽ� �������� ������ ������ ��û�մϴ�.");

            bro = Backend.GameData.Update("USER_DATA", new Where(), param);
        }
        else
        {
            Debug.Log($"{gameDataRowInDate}�� �������� ������ ������ ��û�մϴ�.");

            bro = Backend.GameData.UpdateV2("USER_DATA", gameDataRowInDate, Backend.UserInDate, param);
        }

        if(bro.IsSuccess())
        {
            Debug.Log("�������� ������ ������ �����߽��ϴ�. : " + bro);
        }
        else
        {
            Debug.LogError("�������� ������ ������ �����߽��ϴ�. : " + bro);
        }
    }
}
