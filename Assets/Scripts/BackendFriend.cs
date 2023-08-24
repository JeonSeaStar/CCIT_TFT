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
