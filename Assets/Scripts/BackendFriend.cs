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

    }

    public void GetRecivedRequestFriend() //ģ�� ��û �ҷ����� �� �����ϱ� ����(�ҷ����� �κ�)
    {

    }

    public void ApplyFriend(int index) //ģ�� ��û �ҷ����� �����ϱ� ����(�����ϱ� �κ�)
    {

    }

    public void GetFriendList() //ģ�� ����Ʈ �ҷ�����
    {

    }
}
