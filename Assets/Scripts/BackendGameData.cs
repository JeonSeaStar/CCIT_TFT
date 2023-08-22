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

    private string ganeDataRowInData = string.Empty;

    public void GameDataInsert() //�������� ����
    {

    }

    public void GameDataGet() //�������� �ҷ�����
    {

    }

    public void LevelUp() //�������� ����
    {

    }

    public void GameDataUpdate() //�������� ���� ������Ʈ
    {

    }
}
