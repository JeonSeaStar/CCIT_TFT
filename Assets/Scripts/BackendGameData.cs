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

    private string gameDataRowInDate = string.Empty;

    public void GameDataInsert() //게임정보 삽입
    {
        if (userData == null)
        {
            userData = new UserData();
        }

        Debug.Log("데이터를 초기화합니다.");
        userData.level = 1;
        userData.money1 = 0;
        userData.money2 = 0;
        userData.info = "Null";

        userData.equipment.Add("이즈리얼");
        userData.equipment.Add("기본 도깨비");

        userData.inventory.Add("이벤트 재화", 0);


        Debug.Log("뒤끝 업데이트 목록에 해당 데이터들을 추가합니다.");
        Param param = new Param();
        param.Add("level", userData.level);
        param.Add("money1", userData.money1);
        param.Add("money2", userData.money2);
        param.Add("info", userData.info);
        param.Add("equipment", userData.equipment);
        param.Add("inventory", userData.inventory);


        Debug.Log("게임정보 데이터 삽입을 요청합니다");
        var bro = Backend.GameData.Insert("USER_DATA", param);

        if(bro.IsSuccess())
        {
            Debug.Log("게임정보 데이터 삽입에 성공했습니다. : " + bro);

            //삽입한 게임정보의 고유값입니다.
            gameDataRowInDate = bro.GetInDate();
        }
        else
        {
            Debug.LogError("게임정보 데이터 삽입에 실패했습니다. : " + bro);
        }
    }


    public void GameDataGet() //게임정보 불러오기
    {
        Debug.Log(".게임 정보 조회 함수를 호출합니다.");
        var bro = Backend.GameData.GetMyData("USER_DATA", new Where());
        if(bro.IsSuccess())
        {
            Debug.Log("게임 정보 조회에 성공했습니다. : " + bro);
            LitJson.JsonData gameDataJson = bro.FlattenRows(); // Json으로 리턴된 데이터를 받아옵니다.

            // 받아온 데이터의 갯수가 0이라면 데이터가 존재하지 않는 것입니다.
            if (gameDataJson.Count <= 0)
            {
                Debug.LogWarning("데이터가 존재하지 않습니다.");
            }
            else
            {
                gameDataRowInDate = gameDataJson[0]["inDate"].ToString(); //불러온 게임정보의 고유값입니다.

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
            Debug.LogError("게임 정보 조회에 실패했습니다. : " + bro);
        }
    }
    

    public void LevelUp() //게임정보 수정
    {
        Debug.Log("레벨을 1 증가시킵니다.");
        userData.level += 1;
        userData.money1 += 100;
        userData.money2 += 200;
        userData.info = "내용을 변경합니다.";
    }

    public void GameDataUpdate() //게임정보 수정 업데이트
    {
        if(userData == null)
        {
            Debug.LogError("서버에서 다운받거나 새로 삽입한 데이터가 존재하지 않습니다. Insert 혹은 Get을 통해 데이터를 생성해주세요.");
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
            Debug.Log("내 제일 최신 게임정보 데이터 수정을 요청합니다.");

            bro = Backend.GameData.Update("USER_DATA", new Where(), param);
        }
        else
        {
            Debug.Log($"{gameDataRowInDate}의 게임정보 데이터 수정을 요청합니다.");

            bro = Backend.GameData.UpdateV2("USER_DATA", gameDataRowInDate, Backend.UserInDate, param);
        }

        if(bro.IsSuccess())
        {
            Debug.Log("게임정보 데이터 수정에 성공했습니다. : " + bro);
        }
        else
        {
            Debug.LogError("게임정보 데이터 수정에 실패했습니다. : " + bro);
        }
    }
}
