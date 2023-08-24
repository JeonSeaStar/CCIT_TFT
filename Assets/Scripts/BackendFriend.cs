using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

//뒤끝 SDK namespace 추가
using BackEnd;


public class BackendFriend
{
    private static BackendFriend _instance = null;

    public static BackendFriend Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new BackendFriend();
            }

            return _instance;
        }
    }

    private List<Tuple<string, string>> _requstFriendList = new List<Tuple<string, string>>();

    public void SendFriendRequest(string nickName) //친구 요청 보내기 로직
    {
        var inDateBro = Backend.Social.GetUserInfoByNickName(nickName);

        if(inDateBro.IsSuccess() == false)
        {
            Debug.LogError("유저 이름 검색 도중 에러가 발생했습니다. : " + inDateBro);
            return;
        }

        string inDate = inDateBro.GetReturnValuetoJSON()["row"]["inDate"].ToString();

        Debug.Log($"{nickName}의 inDate값은 {inDate} 입니다.");

        var friendBro = Backend.Friend.RequestFriend(inDate);

        if (friendBro.IsSuccess() == false)
        {
            Debug.LogError($"{inDate} 친구 요청 도중 에러가 발생했습니다. : " + friendBro);
            return;
        }

        Debug.Log("친구 요청에 성공했습니다." + friendBro);
    }

    public void GetRecivedRequestFriend() //친구 요청 불러오기 및 수락하기 로직(불러오기 부분)
    {

    }

    public void ApplyFriend(int index) //친구 요청 불러오기 수락하기 로직(수락하기 부분)
    {

    }

    public void GetFriendList() //친구 리스트 불러오기
    {

    }
}
