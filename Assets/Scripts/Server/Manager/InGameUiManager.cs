using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// UI ����� �� ���� ����
/// </summary>
public class InGameUiManager : MonoBehaviour
{
    static private InGameUiManager instance;
    public TMP_Text scoreBoard;
    public GameObject startCountObject;
    public GameObject reconnectBoardObject;

    private TMP_Text startCountText;
    private TMP_Text reconnectBoardText;
    const string HostOfflineMsg = "ȣ��Ʈ���� ������ ���������ϴ�.\n���� �����";
    const string PlayerReconnectMsg = "{0} �÷��̾� ��������...";

    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;

        startCountText = startCountObject.GetComponentInChildren<TMP_Text>();
        startCountObject.SetActive(true);
        reconnectBoardText = reconnectBoardObject.GetComponentInChildren<TMP_Text>();
        Debug.Log("�ΰ��� UI ���� �Ϸ�");
    }

    public static InGameUiManager GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("InGameUiManager �ν��Ͻ��� �������� �ʽ��ϴ�.");
            return null;
        }

        return instance;
    }

    public void SetScoreBoard(int score)
    {
        scoreBoard.text = "�� ������ �÷��̾� �� : " + score;
    }

    public void SetStartCount(int time, bool isEnable = true)
    {
        startCountObject.SetActive(isEnable);
        if (isEnable)
        {
            if (time == 0)
            {
                startCountText.text = "Game Start!";
            }
            else
            {
                startCountText.text = string.Format("{0}", time);
            }
        }
    }

    public void SetHostWaitBoard()
    {
        reconnectBoardText.text = HostOfflineMsg;
        reconnectBoardObject.SetActive(true);
        // 4�� �� ������ �޽��� ����
        Invoke("ReconnectBoardClose", 4.0f);
    }

    public void SetReconnectBoard(string playerName)
    {
        reconnectBoardText.text = string.Format(PlayerReconnectMsg, playerName);
        reconnectBoardObject.SetActive(true);
        // 4�� �� ������ �޽��� ����
        Invoke("ReconnectBoardClose", 4.0f);
    }

    private void ReconnectBoardClose()
    {
        reconnectBoardObject.SetActive(false);
    }
}
