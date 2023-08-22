using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;


//뒤끝 SDK namespace 추가
using BackEnd;

public class UserData
{
    public int level = 1;
    public int money1 = 0; //무료 다이아
    public int money2 = 0; //유료 다이아
    public string info = string.Empty;
    public Dictionary<string, int> inventory = new Dictionary<string, int>();
    public List<string> equipment = new List<string>();

    //데이터를 디버깅하기 위한 함수입니다. (Debug.Log(UserData);)
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
            result.AppendLine($"| {itemKey} : {inventory[itemKey]}개");
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

    public void GameDataInsert() //게임정보 삽입
    {

    }

    public void GameDataGet() //게임정보 불러오기
    {

    }

    public void LevelUp() //게임정보 수정
    {

    }

    public void GameDataUpdate() //게임정보 수정 업데이트
    {

    }
}
