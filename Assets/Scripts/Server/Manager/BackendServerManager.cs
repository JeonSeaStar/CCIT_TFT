using System;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using static BackEnd.SendQueue;

/// <summary>
/// �ڳ� ������ �����ϴ� ��ũ��Ʈ
/// �ڳ� �⺻ ����� ����
/// �ڳ� �ʱ�ȭ
/// Ŀ���� ȸ������
/// Ŀ���� �α���
/// ���� ���� �ҷ�����
/// </summary>
public class BackendServerManager : MonoBehaviour
{
    private static BackendServerManager instance;
    public bool isLogin { get; set; }                                    //�α��� ����

    private string tempNickName;                                         // ������ �г���(ID�� ����)
    public string myNickName { get; private set; } = string.Empty;       // �α����� ������ �г���
    public string myIndate { get; private set; } = string.Empty;         // �α����� ������ inDate
    private Action<bool, string> loginSuccessFunc = null;

    private const string BackendError = "statusCode : {0}\nErrorCode : {1}\nMessage : {2}";

    public string appleToken = ""; // SignInWithApple.cs���� ��ū���� ���� ���ڿ�
    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("BackendServerManager �ν��Ͻ��� �������� �ʽ��ϴ�.");
            instance = this;
            //�������� ����
            DontDestroyOnLoad(gameObject); 
        }
    }

    public static BackendServerManager GetInstance()
    {
        if(instance == null)
        {
            Debug.LogError("BackendServerManager �ν��Ͻ��� �������� �ʽ��ϴ�.");
            return null;
        }

        return instance;
    }


    /// <summary>
    /// ���� �ʱ�ȭ
    /// </summary>
    private void Start()
    {
        isLogin = false;
        var bro = Backend.Initialize(true);

        if(bro.IsSuccess())
        {
            // �ʱ�ȭ ���� �� statusCode 204 Success
            Debug.Log($"�ʱ�ȭ ���� : {bro}");
#if UNITY_ANDROID
            Debug.Log("GoogleHash - " + Backend.Utils.GetGoogleHash());
#endif
#if !UNITY_EDITOR
            //�ȵ���̵� ȯ�濡���� �۵�
            GetVersionInfo();
#endif
        }
        else
        {
            // �ʱ�ȭ ���� �� statusCode 400�� ���� ����
            Debug.LogError($"�ʱ�ȭ ���� : {bro}");
        }
    }

    private void Update()
    {
        // ������ �񵿱� �޼ҵ� ȣ��(�ݹ� �Լ� Ǯ��)�� ���� �ۼ�
        // ���� : https://developer.thebackend.io/unity3d/guide/Async/AsyncFuncPoll/
        if (Backend.IsInitialized)
        {
            Backend.AsyncPoll(); //�񵿱��Լ� Ǯ��
        }
    }

    //������ ���� ������ �������� �Լ� �����(�ȵ���̵�)������ �۵�
    private void GetVersionInfo() 
    {
        Enqueue(Backend.Utils.GetLatestVersion, callback =>
        {
            if(callback.IsSuccess() == false)
            {
                Debug.LogError("���������� �ҷ����� �� �����Ͽ����ϴ�. \n" + callback);
                return;
            }

            var version = callback.GetReturnValuetoJSON()["version"].ToString();

            Version server = new Version(version);
            Version client = new Version(Application.version);

            var result = server.CompareTo(client);
            if (result == 0)
            {
                // 0 �̸� �� ������ ��ġ
                return;
            }
            else if (result < 0)
            {
                // 0 �̸��̸� server ������ client ���� ����
                // �˼��� �־��� ��� ���⿡ �ش�ȴ�.
                // ex) �˼����� 3.0.0, ���̺꿡 ���ǰ� �ִ� ���� 2.0.0, �ܼ� ���� 2.0.0
                return;
            }
            else
            {
                // 0���� ũ�� server ������ client ���� ����
                if (client == null)
                {
                    // Ŭ���̾�Ʈ�� null�� ��� ����ó��
                    Debug.LogError("Ŭ���̾�Ʈ ���������� null �Դϴ�.");
                    return;
                }
            }

            //���� ������Ʈ �˾�
            LoginUI.GetInstance().OpenUpdatePopup();
        });
    }

    //�ڳ� ��ū���� �α��� (���� ���� �ȵ���̵� ���۷� �α��� ���� �α����� �Ϸ�Ǹ� �������ʹ� �ڳ� ��ū���� �α��� ����)
    public void BackendTokenLogin(Action<bool, string> func)
    {
        Enqueue(Backend.BMember.LoginWithTheBackendToken, callback =>
        {
            if(callback.IsSuccess())
            {
                Debug.Log("��ū �α��� ����");
                loginSuccessFunc = func;

                OnPrevBackendAuthorized();
                return;
            }

            Debug.Log("��ū �α��� ����\n" + callback.ToString());
            func(false, string.Empty);
        });
    }

    //Ŀ���� �α���
    public void CustomLogin(string id, string pw, Action<bool,string> func)
    {
        Enqueue(Backend.BMember.CustomLogin, id, pw, callback =>
        {
            if(callback.IsSuccess())
            {
                Debug.Log("Ŀ���� �α��� ����");
                loginSuccessFunc = func;

                OnPrevBackendAuthorized();
                return;
            }

            Debug.Log("Ŀ���� �α��� ����\n" + callback);
            func(false, string.Format(BackendError,callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
        });
    }

    //Ŀ���� ȸ������
    public void CustomSignIn(string id, string pw, Action<bool, string> func)
    {
        tempNickName = id;
        Enqueue(Backend.BMember.CustomSignUp, id, pw, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("Ŀ���� ȸ������ ����");
                loginSuccessFunc = func;

                OnPrevBackendAuthorized();
                return;
            }

            Debug.LogError("Ŀ���� ȸ������ ����\n" + callback.ToString());
            func(false, string.Format(BackendError, callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
        });
    }

    //�г��� ������Ʈ�� �ڵ����� ID�� ������ -���氡��-
    public void UpdateNickname(string nickname, Action<bool, string> func)
    {
        Enqueue(Backend.BMember.UpdateNickname, nickname, bro =>
        {
            // �г����� ������ ��ġ���� ������ �ȵ�
            if (!bro.IsSuccess())
            {
                Debug.LogError("�г��� ���� ����\n" + bro.ToString());
                func(false, string.Format(BackendError, bro.GetStatusCode(), bro.GetErrorCode(), bro.GetMessage()));
                return;
            }
            loginSuccessFunc = func;
            OnBackendAuthorized();
        });
    }

    // ���� ���� �ҷ����� �����۾�
    private void OnPrevBackendAuthorized()
    {
        isLogin = true;

        OnBackendAuthorized();
    }

    // ���� ���� ���� �ҷ�����
    private void OnBackendAuthorized()
    {
        Enqueue(Backend.BMember.GetUserInfo, callback =>
        {
            if (!callback.IsSuccess())
            {
                Debug.LogError("���� ���� �ҷ����� ����\n" + callback);
                loginSuccessFunc(false, string.Format(BackendError,
                callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
                return;
            }
            Debug.Log("��������\n" + callback);

            var info = callback.GetReturnValuetoJSON()["row"];
            if (info["nickname"] == null)
            {
                LoginUI.GetInstance().ActiveNickNameObject();
                return;
            }
            myNickName = info["nickname"].ToString();
            myIndate = info["inDate"].ToString();

            if (loginSuccessFunc != null)
            {
                BackendMatchManager.GetInstance().GetMatchList(loginSuccessFunc);
            }
        });
    }

    // �Խ�Ʈ �α��� ���
    public void GuestLogin(Action<bool, string> func)
    {
        Enqueue(Backend.BMember.GuestLogin, callback =>
        {
            if (callback.IsSuccess())
            {
                Debug.Log("�Խ�Ʈ �α��� ����");
                loginSuccessFunc = func;

                OnPrevBackendAuthorized();
                return;
            }

            Debug.Log("�Խ�Ʈ �α��� ����\n" + callback);
            func(false, string.Format(BackendError, callback.GetStatusCode(), callback.GetErrorCode(), callback.GetMessage()));
        });
    }



    #region �Ʒ��� ���ʹ� ģ�� ���� ��� (���� ����)

    public void GetFriendList(Action<bool, List<Friend>> func)
    {
        Enqueue(Backend.Friend.GetFriendList, 15, callback =>
        {
            if (callback.IsSuccess() == false)
            {
                func(false, null);
                return;
            }

            var friendList = new List<Friend>();

            foreach (LitJson.JsonData tmp in callback.Rows())
            {
                if (tmp.Keys.Contains("nickname") == false)
                {
                    continue;
                }
                Friend friend = new Friend();
                friend.nickName = tmp["nickname"]["S"].ToString();
                friend.inDate = tmp["inDate"]["S"].ToString();

                friendList.Add(friend);
            }

            func(true, friendList);
        });
    }

    public void GetReceivedRequestFriendList(Action<bool, List<Friend>> func)
    {
        Enqueue(Backend.Friend.GetReceivedRequestList, 15, callback =>
        {
            if (callback.IsSuccess() == false)
            {
                func(false, null);
                return;
            }

            var friendList = new List<Friend>();

            foreach (LitJson.JsonData tmp in callback.Rows())
            {
                if (tmp.Keys.Contains("nickname") == false)
                {
                    continue;
                }
                Friend friend = new Friend();
                friend.nickName = tmp["nickname"]["S"].ToString();
                friend.inDate = tmp["inDate"]["S"].ToString();

                friendList.Add(friend);
            }

            func(true, friendList);
        });
    }

    public void RequestFirend(string nickName, Action<bool, string> func)
    {
        Enqueue(Backend.Social.GetUserInfoByNickName, nickName, callback =>
        {
            Debug.Log(callback);
            if (callback.IsSuccess() == false)
            {
                func(false, callback.GetMessage());
                return;
            }
            string inDate = callback.GetReturnValuetoJSON()["row"]["inDate"].ToString();
            Enqueue(Backend.Friend.RequestFriend, inDate, callback2 =>
            {
                Debug.Log(callback2);
                if (callback2.IsSuccess() == false)
                {
                    func(false, callback2.GetMessage());
                    return;
                }

                func(true, string.Empty);
            });
        });
    }

    public void AcceptFriend(string inDate, Action<bool, string> func)
    {
        Enqueue(Backend.Friend.AcceptFriend, inDate, callback2 =>
        {
            if (callback2.IsSuccess() == false)
            {
                func(false, callback2.GetMessage());
                return;
            }

            func(true, string.Empty);
        });
    }

    public void RejectFriend(string inDate, Action<bool, string> func)
    {
        Enqueue(Backend.Friend.RejectFriend, inDate, callback2 =>
        {
            if (callback2.IsSuccess() == false)
            {
                func(false, callback2.GetMessage());
                return;
            }

            func(true, string.Empty);
        });
    }
    #endregion
}

