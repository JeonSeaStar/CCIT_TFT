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
        var bro = Backend.Friend.GetReceivedRequestList();

        if(bro.IsSuccess() == false)
        {
            Debug.Log("친구 요청 받은 리스트를 불러오는 중 에러가 발생했습니다. : " + bro);
            return;
        }

        if (bro.FlattenRows().Count <= 0)
        {
            Debug.LogError("친구 요청이 온 내역이 존재하지 않습니다.");
            return;
        }

        Debug.Log("친구 요청을 받은 리스트 불러오기에 성공했습니다. : " + bro);

        int index = 0;
        foreach(LitJson.JsonData friendJson in bro.FlattenRows())
        {
            string nickName = friendJson["nickname"]?.ToString();
            string inDate = friendJson["inDate"].ToString();

            _requstFriendList.Add(new Tuple<string, string>(nickName, inDate));

            Debug.Log($"{index}. {nickName} - {inDate}");
            index++;
        }
    }

    public void ApplyFriend(int index) //친구 요청 불러오기 수락하기 로직(수락하기 부분)
    {
        if(_requstFriendList.Count <= 0)
        {
            Debug.LogError("요청이 온 친구가 존재하지 않습니다.");
            return;
        }

        if(index >= _requstFriendList.Count)
        {
            Debug.LogError($"요청한 친구 요청 리스트의 범위를 벗어났습니다. 선택 : {index} / 리스트 최대 : {_requstFriendList.Count}");
            return;
        }

        var bro = Backend.Friend.AcceptFriend(_requstFriendList[index].Item2);

        if(bro.IsSuccess() == false)
        {
            Debug.LogError("친구 수락 중 에러가 발생했습니다. : " + bro);
            return;
        }

        Debug.Log($"{_requstFriendList[index].Item1}이(가) 친구가 되었습니다. : " + bro);
    }

    public void GetFriendList() //친구 리스트 불러오기
    {

    }
}
