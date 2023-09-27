using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Battlehub.Dispatcher;


public class Friend
{
    public string nickName;
    public string inDate;
}

public class FriendUI : MonoBehaviour
{
    public GameObject friendListView;
    public GameObject errorTextView;
    public GameObject friendRequestView;
    public GameObject friendListParent;
    public GameObject friendPrefab;
    public Text title;
    public InputField insertNickNameField;

    private FriendTabUI[] friendTabInfo;

    void Awake()
    {
        friendTabInfo = this.GetComponentsInChildren<FriendTabUI>();
    }

    private void InsertFriendPrefab(string nickName, string inDate = "")
    {
        GameObject friend = GameObject.Instantiate(friendPrefab, Vector3.zero, Quaternion.identity, friendListParent.transform);
        friend.GetComponentInChildren<Text>().text = nickName;

        if (inDate.Equals(string.Empty) == false)
        {
            friend.GetComponent<Button>().onClick.AddListener(() =>
            {
                LobbyUI.GetInstance().SetLoadingObjectActive(true);
                LobbyUI.GetInstance().SetSelectObject("ģ�� ��û�� �����ұ��?", () =>
                {
                    // ok��ư ��ȣ�ۿ�
                    BackEndServerManager.GetInstance().AcceptFriend(inDate, (bool isSuccess, string error) =>
                    {
                        Dispatcher.Current.BeginInvoke(() =>
                        {
                            LobbyUI.GetInstance().SetLoadingObjectActive(false);

                            if (isSuccess == false)
                            {
                                LobbyUI.GetInstance().SetErrorObject("ģ�� ��û ���� ����\n\n" + error);
                                return;
                            }

                            LobbyUI.GetInstance().SetErrorObject("ģ�� ��û ���� ����!");
                        });
                    });

                    friend.SetActive(false);
                    GameObject.Destroy(friend, 0.1f);
                },
                () =>
                {
                    // cancel ��ư ��ȣ�ۿ�
                    BackEndServerManager.GetInstance().RejectFriend(inDate, (bool isSuccess, string error) =>
                    {
                        Dispatcher.Current.BeginInvoke(() =>
                        {
                            LobbyUI.GetInstance().SetLoadingObjectActive(false);

                            if (isSuccess == false)
                            {
                                LobbyUI.GetInstance().SetErrorObject("ģ�� ��û ���� ����\n\n" + error);
                                return;
                            }

                            LobbyUI.GetInstance().SetErrorObject("ģ�� ��û ���� ����");
                        });
                    });

                    friend.SetActive(false);
                    GameObject.Destroy(friend, 0.1f);
                });
            });
        }
    }

    private void ClearFriendList()
    {
        var parent = friendListParent.transform;

        while (parent.childCount > 0)
        {
            var child = parent.GetChild(0);
            GameObject.DestroyImmediate(child.gameObject);
        }
    }

    public void SetFriendList()
    {
        if (LobbyUI.GetInstance().IsLoadingObjectActive() || LobbyUI.GetInstance().IsErrorObjectActive())
        {
            return;
        }

        title.text = "ģ �� �� ��";
        LobbyUI.GetInstance().SetLoadingObjectActive(true);
        ClearFriendList();
        BackEndServerManager.GetInstance().GetFriendList((bool result, List<Friend> friendList) =>
        {
            Dispatcher.Current.BeginInvoke(() =>
            {
                LobbyUI.GetInstance().SetLoadingObjectActive(false);

                if (result == false)
                {
                    LobbyUI.GetInstance().SetErrorObject("����\n\nģ������� �ҷ����� �� �����߽��ϴ�.\n��� �� �ٽ� �õ����ּ���.");
                    errorTextView.SetActive(true);
                    return;
                }

                if (friendList == null)
                {
                    LobbyUI.GetInstance().SetErrorObject("ģ���� �������� �ʽ��ϴ�.");
                    errorTextView.SetActive(true);
                    return;
                }

                if (friendList.Count <= 0)
                {
                    LobbyUI.GetInstance().SetErrorObject("ģ���� �������� �ʽ��ϴ�.");
                    errorTextView.SetActive(true);
                    return;
                }

                foreach (var tmp in friendList)
                {
                    InsertFriendPrefab(tmp.nickName);
                }

                errorTextView.SetActive(false);
            });
        });
    }

    public void SetReceivedRequestFriendList()
    {
        if (LobbyUI.GetInstance().IsLoadingObjectActive() || LobbyUI.GetInstance().IsErrorObjectActive())
        {
            return;
        }

        title.text = "ģ �� �� û �� �� �� ��";
        LobbyUI.GetInstance().SetLoadingObjectActive(true);
        ClearFriendList();
        BackEndServerManager.GetInstance().GetReceivedRequestFriendList((bool result, List<Friend> friendList) =>
        {
            Dispatcher.Current.BeginInvoke(() =>
            {
                LobbyUI.GetInstance().SetLoadingObjectActive(false);

                if (result == false)
                {
                    LobbyUI.GetInstance().SetErrorObject("����\n\nģ�� ��û ���� ����� �ҷ����� �� �����߽��ϴ�.\n��� �� �ٽ� �õ����ּ���.");
                    errorTextView.SetActive(true);
                    return;
                }

                if (friendList == null)
                {
                    LobbyUI.GetInstance().SetErrorObject("ģ�� ��û ���� ����� �������� �ʽ��ϴ�.");
                    errorTextView.SetActive(true);
                    return;
                }

                if (friendList.Count <= 0)
                {
                    LobbyUI.GetInstance().SetErrorObject("ģ���� �������� �ʽ��ϴ�.");
                    errorTextView.SetActive(true);
                    return;
                }



                foreach (var tmp in friendList)
                {
                    InsertFriendPrefab(tmp.nickName, tmp.inDate);
                }

                errorTextView.SetActive(false);
            });
        });
    }

    public void RequestFriend()
    {
        if (LobbyUI.GetInstance().IsLoadingObjectActive() || LobbyUI.GetInstance().IsErrorObjectActive())
        {
            return;
        }

        if (insertNickNameField.text.Equals(string.Empty))
        {
            Debug.Log("�г��� â�� ����ֽ��ϴ�.");
            LobbyUI.GetInstance().SetErrorObject("����\n\n�г��� â�� ����ֽ��ϴ�.");
            return;
        }

        LobbyUI.GetInstance().SetLoadingObjectActive(true);
        var nickName = insertNickNameField.text;

        BackEndServerManager.GetInstance().RequestFirend(nickName, (bool isSuccess, string errorText) =>
        {
            Dispatcher.Current.BeginInvoke(() =>
            {
                LobbyUI.GetInstance().SetLoadingObjectActive(false);

                if (isSuccess == false)
                {
                    LobbyUI.GetInstance().SetErrorObject("����\n\n" + errorText);
                    return;
                }

                LobbyUI.GetInstance().SetErrorObject("ģ�� ��û ����!\n\n������ ģ�� ��û�� �����ϸ� ģ���� �˴ϴ�.");
            });
        });
    }

    public void OpenFriendInfo()
    {
        this.gameObject.SetActive(true);

        SwitchFriendTab();
    }

    public void SwitchFriendTab()
    {
        for (int i = 0; i < friendTabInfo.Length; ++i)
        {
            if (friendTabInfo[i].IsOn() == true)
            {
                switch (i)
                {
                    case 0:
                        SetFriendList();
                        break;
                    case 1:
                        SetReceivedRequestFriendList();
                        break;
                    case 2:
                        OpenRequestFriend();
                        break;
                    default:
                        Debug.LogError("OpenFriendInfo index error : " + i);
                        break;
                }
                return; ;
            }
        }
    }

    public void OpenRequestFriend()
    {
        friendRequestView.SetActive(true);
        friendListView.SetActive(false);
    }
}
