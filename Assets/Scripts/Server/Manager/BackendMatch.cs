using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using BackEnd.Tcp;

/// <summary>
/// ��ġ�Ŵ��� (��Ī ���� ���)
/// BackendMatch.cs���� ���ǵ� ��ɵ�
/// ��Ī�� �ʿ��� ���� ����
/// ��Ī���� �����ϱ�
/// ��Ī���� ������
/// ��Ī ��û�ϱ�
/// ��Ī ��û ����ϱ�
/// </summary>

public partial class BackEndMatchManager : MonoBehaviour
{
    public MatchType nowMatchType { get; private set; } = MatchType.None;     // ���� ���õ� ��ġ Ÿ��
    public MatchModeType nowModeType { get; private set; } = MatchModeType.None; // ���� ���õ� ��ġ ��� Ÿ��

    // ����� �α�
    private string NOTCONNECT_MATCHSERVER = "��ġ ������ ����Ǿ� ���� �ʽ��ϴ�.";
    private string RECONNECT_MATCHSERVER = "��ġ ������ ������ �õ��մϴ�.";
    private string FAIL_CONNECT_MATCHSERVER = "��ġ ���� ���� ���� : {0}";
    private string SUCCESS_CONNECT_MATCHSERVER = "��ġ ���� ���� ����";
    private string SUCCESS_MATCHMAKE = "��Ī ���� : {0}";
    private string SUCCESS_REGIST_MATCHMAKE = "��Ī ��⿭�� ��ϵǾ����ϴ�.";
    private string FAIL_REGIST_MATCHMAKE = "��Ī ���� : {0}";
    private string CANCEL_MATCHMAKE = "��Ī ��û ��� : {0}";
    private string INVAILD_MATCHTYPE = "�߸��� ��ġ Ÿ���Դϴ�.";
    private string INVALID_MODETYPE = "�߸��� ��� Ÿ���Դϴ�.";
    private string INVALID_OPERATION = "�߸��� ��û�Դϴ�\n{0}";
    private string EXCEPTION_OCCUR = "���� �߻� : {0}\n�ٽ� ��Ī�� �õ��մϴ�.";

    // ��Ī�� ��ġ Ÿ��,��� Ÿ�� ����
    public void SetNowMatchInfo(MatchType matchType, MatchModeType matchModeType)
    {
        this.nowMatchType = matchType;
        this.nowModeType = matchModeType;

        Debug.Log(string.Format("��Ī Ÿ��/��� : {0}/{1}", nowMatchType, nowModeType));
    }

    // ��Ī ���� ����
    public void JoinMatchServer()
    {
        if (isConnectMatchServer)
        {
            return;
        }
        ErrorInfo errorInfo;
        isConnectMatchServer = true;
        if (!Backend.Match.JoinMatchMakingServer(out errorInfo))
        {
            var errorLog = string.Format(FAIL_CONNECT_MATCHSERVER, errorInfo.ToString());
            Debug.Log(errorLog);
        }
    }

    // ��Ī ���� ��������
    public void LeaveMatchServer()
    {
        isConnectMatchServer = false;
        Backend.Match.LeaveMatchMakingServer();
    }

    // ��Ī ��� �� �����
    // ȥ�� ��Ī�� �ϴ��� ������ ���� ������ �� ��Ī�� ��û�ؾ� ��
    public bool CreateMatchRoom()
    {
        // ��û ������ ����Ǿ� ���� ������ ��Ī ���� ����
        if (!isConnectMatchServer)
        {
            Debug.Log(NOTCONNECT_MATCHSERVER);
            Debug.Log(RECONNECT_MATCHSERVER);
            JoinMatchServer();
            return false;
        }
        Debug.Log("�� ���� ��û�� ������ ����");
        Backend.Match.CreateMatchRoom();
        return true;
    }

    // ��Ī ��� �� ������
    public void LeaveMatchLoom()
    {
        Backend.Match.LeaveMatchRoom();
    }

    // ��Ī ��û�ϱ�
    // MatchType (1:1 / ������ / ����)
    // MatchModeType (���� / ����Ʈ / MMR)
    public void RequestMatchMaking(int index)
    {
        // ��û ������ ����Ǿ� ���� ������ ��Ī ���� ����
        if (!isConnectMatchServer)
        {
            Debug.Log(NOTCONNECT_MATCHSERVER);
            Debug.Log(RECONNECT_MATCHSERVER);
            JoinMatchServer();
            return;
        }
        // ���� �ʱ�ȭ
        isConnectInGameServer = false;

        Backend.Match.RequestMatchMaking(matchInfos[index].matchType, matchInfos[index].matchModeType, matchInfos[index].inDate);
        if (isConnectInGameServer)
        {
            Backend.Match.LeaveGameServer(); //�ΰ��� ���� ���ӵǾ� ���� ��츦 ����� �ΰ��� ���� ���� ȣ��
        }

        //nowMatchType = matchInfos[index].matchType;
        //nowModeType = matchInfos[index].matchModeType;
        //numOfClient = int.Parse(matchInfos[index].headCount);
    }


    // ��Ī ��û ����ϱ�
    public void CancelRegistMatchMaking()
    {
        Backend.Match.CancelMatchMaking();
    }

    // ��Ī ���� ���ӿ� ���� ���ϰ�
    private void ProcessAccessMatchMakingServer(ErrorInfo errInfo)
    {
        if (errInfo != ErrorInfo.Success)
        {
            // ���� ����
            isConnectMatchServer = false;
        }

        if (!isConnectMatchServer)
        {
            var errorLog = string.Format(FAIL_CONNECT_MATCHSERVER, errInfo.ToString());
            // ���� ����
            Debug.Log(errorLog);
        }
        else
        {
            //���� ����
            Debug.Log(SUCCESS_CONNECT_MATCHSERVER);
        }
    }

    /*
	 * ��Ī ��û�� ���� ���ϰ� (ȣ��Ǵ� ����)
	 * ��Ī ��û �������� ��
	 * ��Ī �������� ��
	 * ��Ī ��û �������� ��
	*/
    private void ProcessMatchMakingResponse(MatchMakingResponseEventArgs args)
    {
        string debugLog = string.Empty;
        bool isError = false;
        switch (args.ErrInfo)
        {
            case ErrorCode.Success:
                // ��Ī �������� ��
                debugLog = string.Format(SUCCESS_MATCHMAKE, args.Reason);
                LobbyUI.GetInstance().MatchDoneCallback();
                ProcessMatchSuccess(args);
                break;
            case ErrorCode.Match_InProgress:
                // ��Ī ��û �������� �� or ��Ī ���� �� ��Ī ��û�� �õ����� ��

                // ��Ī ��û �������� ��
                if (args.Reason == string.Empty)
                {
                    debugLog = SUCCESS_REGIST_MATCHMAKE;

                    LobbyUI.GetInstance().MatchRequestCallback(true);
                }
                break;
            case ErrorCode.Match_MatchMakingCanceled:
                // ��Ī ��û�� ��ҵǾ��� ��
                debugLog = string.Format(CANCEL_MATCHMAKE, args.Reason);

                LobbyUI.GetInstance().MatchRequestCallback(false);
                break;
            case ErrorCode.Match_InvalidMatchType:
                isError = true;
                // ��ġ Ÿ���� �߸� �������� ��
                debugLog = string.Format(FAIL_REGIST_MATCHMAKE, INVAILD_MATCHTYPE);

                LobbyUI.GetInstance().MatchRequestCallback(false);
                break;
            case ErrorCode.Match_InvalidModeType:
                isError = true;
                // ��ġ ��带 �߸� �������� ��
                debugLog = string.Format(FAIL_REGIST_MATCHMAKE, INVALID_MODETYPE);

                LobbyUI.GetInstance().MatchRequestCallback(false);
                break;
            case ErrorCode.InvalidOperation:
                isError = true;
                // �߸��� ��û�� �������� ��
                debugLog = string.Format(INVALID_OPERATION, args.Reason);
                LobbyUI.GetInstance().MatchRequestCallback(false);
                break;
            case ErrorCode.Match_Making_InvalidRoom:
                isError = true;
                // �߸��� ��û�� �������� ��
                debugLog = string.Format(INVALID_OPERATION, args.Reason);
                LobbyUI.GetInstance().MatchRequestCallback(false);
                break;
            case ErrorCode.Exception:
                isError = true;
                // ��Ī �ǰ�, �������� �� ������ �� ���� �߻� �� exception�� ���ϵ�
                // �� ��� �ٽ� ��Ī ��û�ؾ� ��
                debugLog = string.Format(EXCEPTION_OCCUR, args.Reason);

                LobbyUI.GetInstance().RequestMatch();
                break;
        }

        if (!debugLog.Equals(string.Empty))
        {
            Debug.Log(debugLog);
            if (isError == true)
            {
                LobbyUI.GetInstance().SetErrorObject(debugLog);
            }
        }
    }

    // ��Ī �������� ��
    // �ΰ��� ������ �����ؾ� �Ѵ�.
    private void ProcessMatchSuccess(MatchMakingResponseEventArgs args)
    {
        ErrorInfo errorInfo;
        if (sessionIdList != null)
        {
            Debug.Log("���� ���� ���� ����");
            sessionIdList.Clear();
        }

        if (!Backend.Match.JoinGameServer(args.RoomInfo.m_inGameServerEndPoint.m_address, args.RoomInfo.m_inGameServerEndPoint.m_port, false, out errorInfo))
        {
            var debugLog = string.Format(FAIL_ACCESS_INGAME, errorInfo.ToString(), string.Empty);
            Debug.Log(debugLog);
        }
        // ���ڰ����� �ΰ��� ����ū�� �����صξ�� �Ѵ�.
        // �ΰ��� �������� �뿡 ������ �� �ʿ�
        // 1�� ���� ��� ������ �뿡 �������� ������ �ش� ���� �ı�ȴ�.
        isConnectInGameServer = true;
        isJoinGameRoom = false;
        isReconnectProcess = false;
        inGameRoomToken = args.RoomInfo.m_inGameRoomToken;
        isSandBoxGame = args.RoomInfo.m_enableSandbox;
        var info = GetMatchInfo(args.MatchCardIndate);
        if (info == null)
        {
            Debug.LogError("��ġ ������ �ҷ����� �� �����߽��ϴ�.");
            return;
        }

        nowMatchType = info.matchType;
        nowModeType = info.matchModeType;
        numOfClient = int.Parse(info.headCount);
    }

    public void ProcessReconnect()
    {
        Debug.Log("������ ���μ��� ����");
        if (roomInfo == null)
        {
            Debug.LogError("������ �� �� ������ �������� �ʽ��ϴ�.");
            return;
        }
        ErrorInfo errorInfo;

        if (sessionIdList != null)
        {
            Debug.Log("���� ���� ���� ���� : " + sessionIdList.Count);
            sessionIdList.Clear();
        }

        if (!Backend.Match.JoinGameServer(roomInfo.host, roomInfo.port, true, out errorInfo))
        {
            var debugLog = string.Format(FAIL_ACCESS_INGAME, errorInfo.ToString(), string.Empty);
            Debug.Log(debugLog);
        }

        isConnectInGameServer = true;
        isJoinGameRoom = false;
        isReconnectProcess = true;
    }
}