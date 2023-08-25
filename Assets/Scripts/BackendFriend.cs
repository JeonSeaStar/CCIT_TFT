using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;

//�ڳ� SDK namespace �߰�
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

    public void SendFriendRequest(string nickName) //ģ�� ��û ������ ����
    {
        var inDateBro = Backend.Social.GetUserInfoByNickName(nickName);

        if(inDateBro.IsSuccess() == false)
        {
            Debug.LogError("���� �̸� �˻� ���� ������ �߻��߽��ϴ�. : " + inDateBro);
            return;
        }

        string inDate = inDateBro.GetReturnValuetoJSON()["row"]["inDate"].ToString();

        Debug.Log($"{nickName}�� inDate���� {inDate} �Դϴ�.");

        var friendBro = Backend.Friend.RequestFriend(inDate);

        if (friendBro.IsSuccess() == false)
        {
            Debug.LogError($"{inDate} ģ�� ��û ���� ������ �߻��߽��ϴ�. : " + friendBro);
            return;
        }

        Debug.Log("ģ�� ��û�� �����߽��ϴ�." + friendBro);
    }

    public void GetRecivedRequestFriend() //ģ�� ��û �ҷ����� �� �����ϱ� ����(�ҷ����� �κ�)
    {
        var bro = Backend.Friend.GetReceivedRequestList();

        if(bro.IsSuccess() == false)
        {
            Debug.Log("ģ�� ��û ���� ����Ʈ�� �ҷ����� �� ������ �߻��߽��ϴ�. : " + bro);
            return;
        }

        if (bro.FlattenRows().Count <= 0)
        {
            Debug.LogError("ģ�� ��û�� �� ������ �������� �ʽ��ϴ�.");
            return;
        }

        Debug.Log("ģ�� ��û�� ���� ����Ʈ �ҷ����⿡ �����߽��ϴ�. : " + bro);

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

    public void ApplyFriend(int index) //ģ�� ��û �ҷ����� �����ϱ� ����(�����ϱ� �κ�)
    {
        if(_requstFriendList.Count <= 0)
        {
            Debug.LogError("��û�� �� ģ���� �������� �ʽ��ϴ�.");
            return;
        }

        if(index >= _requstFriendList.Count)
        {
            Debug.LogError($"��û�� ģ�� ��û ����Ʈ�� ������ ������ϴ�. ���� : {index} / ����Ʈ �ִ� : {_requstFriendList.Count}");
            return;
        }

        var bro = Backend.Friend.AcceptFriend(_requstFriendList[index].Item2);

        if(bro.IsSuccess() == false)
        {
            Debug.LogError("ģ�� ���� �� ������ �߻��߽��ϴ�. : " + bro);
            return;
        }

        Debug.Log($"{_requstFriendList[index].Item1}��(��) ģ���� �Ǿ����ϴ�. : " + bro);
    }

    public void GetFriendList() //ģ�� ����Ʈ �ҷ�����
    {

    }
}
