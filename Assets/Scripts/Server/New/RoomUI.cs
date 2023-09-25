using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BackEnd.Tcp;
using Battlehub.Dispatcher;
using BackEnd;

public partial class LobbyUI : MonoBehaviour
{
    public GameObject readyRoomObject;
    public GameObject friendListParent;
    public GameObject readyUserListParent;
    public GameObject friendPrefab;
    public GameObject friendEmptyObject;

    private List<string> readyUserList = null;
    public void OpenRoomUI()
    {
        // ��ġ ������ ���� ���� ��û
        if (BackendMatchManager.GetInstance().CreateMatchRoom() == true)
        {
            SetLoadingObjectActive(true);
        }
    }

    public void CreateRoomResult(bool isSuccess, List<MatchMakingUserInfo> userList = null)
    {
        // ��� �� ������ ���� �� ���� UI�� Ȱ��ȭ ��Ű��,
        // ģ������� ��ȸ
        if (isSuccess == true)
        {
            readyRoomObject.SetActive(true);
            SetFriendList();
            if (userList == null)
            {
                SetReadyUserList(BackendServerManager.GetInstance().myNickName);
            }
            else
            {
                SetReadyUserList(userList);
            }
        }
        // ��� �� ������ ���� �� ������ ���
        else
        {
            SetLoadingObjectActive(false);
            SetErrorObject("���� ������ �����߽��ϴ�.\n\n��� �� �ٽ� �õ����ּ���.");
        }
    }

    public void LeaveReadyRoom()
    {
        BackendMatchManager.GetInstance().LeaveMatchRoom();
        // readyRoomObject.SetActive(false);
    }

    public void CloseRoomUIOnly()
    {
        readyRoomObject.SetActive(false);
    }

    public void RequestMatch()
    {
        if (loadingObject.activeSelf || recordObject.activeSelf || errorObject.activeSelf || requestProgressObject.activeSelf || matchDoneObject.activeSelf)
        {
            return;
        }

        foreach (var tab in matchInfotabList)
        {
            if (tab.IsOn() == true)
            {
                BackendMatchManager.GetInstance().RequestMatchMaking(tab.index);
                return;
            }
        }

        Debug.Log("Ȱ��ȭ�� ���� �������� �ʽ��ϴ�.");
    }

    // ģ�� ��� ����
    public void SetFriendList()
    {
        ClearFriendList();
        BackendServerManager.GetInstance().GetFriendList((bool result, List<Friend> friendList) =>
        {
            Dispatcher.Current.BeginInvoke(() =>
            {
                SetLoadingObjectActive(false);
                if (result == false)
                {
                    friendEmptyObject.SetActive(true);
                    return;
                }

                if (friendList == null)
                {
                    friendEmptyObject.SetActive(true);
                    return;
                }

                if (friendList.Count <= 0)
                {
                    friendEmptyObject.SetActive(true);
                    return;
                }

                foreach (var tmp in friendList)
                {
                    InsertFriendPrefab(tmp.nickName);
                }

                friendEmptyObject.SetActive(false);
            });
        });
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

    private void InsertFriendPrefab(string nickName)
    {
        GameObject friend = GameObject.Instantiate(friendPrefab, Vector3.zero, Quaternion.identity, friendListParent.transform);
        friend.GetComponentInChildren<Text>().text = nickName;
        friend.GetComponent<Button>().onClick.AddListener(() =>
        {
            SetSelectObject(nickName + " �������� �ʴ븦 �����ڽ��ϱ�?", () =>
            {
                var userCount = readyUserListParent.transform.childCount;
                if (userCount >= 2)
                {
                    SetErrorObject("�濡 ���� �ڸ��� �����ϴ�. (�ִ� �ο� 2��)");
                    return;
                }

                Backend.Match.InviteUser(nickName);
                Debug.Log(nickName + " �ʴ븦 ����");
            },
            () =>
            {
                Debug.Log(nickName + " �ʴ븦 ������ ����");
            });
        });
    }

    // ���� �ο� ����
    public void SetReadyUserList(List<MatchMakingUserInfo> userList)
    {
        ClearReadyUserList();

        if (userList == null)
        {
            Debug.LogError("ready user list is null");
            return;
        }
        if (userList.Count <= 0)
        {
            Debug.LogError("ready user list is empty");
            return;
        }

        foreach (var user in userList)
        {
            InsertReadyUserPrefab(user.m_nickName);
        }
    }

    public void SetReadyUserList(string nickName)
    {
        ClearReadyUserList();

        if (string.IsNullOrEmpty(nickName))
        {
            Debug.LogError("ready user list is empty");
            return;
        }

        InsertReadyUserPrefab(nickName);
    }

    public void InsertReadyUserPrefab(string nickName)
    {
        if (readyUserList == null)
        {
            return;
        }

        if (readyUserList.Contains(nickName))
        {
            return;
        }

        GameObject friend = GameObject.Instantiate(friendPrefab, Vector3.zero, Quaternion.identity, readyUserListParent.transform);
        friend.GetComponentInChildren<Text>().text = nickName;
        friend.GetComponent<Button>().onClick.AddListener(() =>
        {
            SetSelectObject(nickName + " ������ �����ϰڽ��ϱ�?", () =>
            {
                Backend.Match.KickUser(nickName);
                Debug.Log(nickName + " ���� �õ���");
            },
            () =>
            {
                Debug.Log(nickName + " �������� ����");
            });
        });

        readyUserList.Add(nickName);
    }

    public void DeleteReadyUserPrefab(string nickName)
    {
        if (readyUserList == null)
        {
            return;
        }

        if (readyUserList.Contains(nickName) == false)
        {
            return;
        }

        var parent = readyUserListParent.transform;

        for (int i = 0; i < readyUserList.Count; ++i)
        {
            if (nickName.Equals(readyUserList[i]) == false)
            {
                continue;
            }
            var child = parent.GetChild(i);
            if (child == null)
            {
                continue;
            }
            GameObject.DestroyImmediate(child.gameObject);
        }

        readyUserList.Remove(nickName);
    }

    private void ClearReadyUserList()
    {
        readyUserList = new List<string>();
        var parent = readyUserListParent.transform;

        while (parent.childCount > 0)
        {
            var child = parent.GetChild(0);
            GameObject.DestroyImmediate(child.gameObject);
        }
    }
}

