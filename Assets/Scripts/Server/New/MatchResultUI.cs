using UnityEngine;
using UnityEngine.UI;
using BackEnd;
using BackEnd.Tcp;
using System.Collections.Generic;
using TMPro;

/// <summary>
/// ���� ����� �˷��ִ� �Ŵ��� ������ ��� ����ϰ� ����Ǿ� �ִ��� Ȯ���ϰ�
/// ���� ����� ����ְ� ������ ������ �˷���
/// </summary>
public class MatchResultUI : MonoBehaviour
{
    private static MatchResultUI instance;
    public GameObject gameResultObject;
    public GameObject baseResultObject;
    public GameObject meleeResultObject;

    private GameObject endLoadingObject;
    private GameObject returnLobbyObject;
    private FadeAnimation fadeObject;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }

    public static MatchResultUI GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("MatchResultUI �ν��Ͻ��� �������� �ʽ��ϴ�.");
            return null;
        }

        return instance;
    }

    void Start()
    {
        var backGroundObject = gameResultObject.transform.GetChild(0).gameObject;
        endLoadingObject = backGroundObject.transform.GetChild(1).gameObject;
        returnLobbyObject = backGroundObject.transform.GetChild(4).gameObject;

        gameResultObject.SetActive(false);
        baseResultObject.SetActive(false);
        meleeResultObject.SetActive(false);
        endLoadingObject.SetActive(true);
        returnLobbyObject.SetActive(false);

        var fade = GameObject.FindGameObjectWithTag("Fade");
        if (fade != null)
        {
            fadeObject = fade.GetComponent<FadeAnimation>();
        }

        Debug.Log("MatchResultUI ���� �Ϸ�");
    }

    public void SetGameResult(MatchGameResult matchGameResult)
    {
        gameResultObject.SetActive(true);
        endLoadingObject.SetActive(true);
        returnLobbyObject.SetActive(false);
        baseResultObject.SetActive(false);
        meleeResultObject.SetActive(false);

        var matchInstance = BackEndMatchManager.GetInstance();
        if (matchInstance == null)
        {
            returnLobbyObject.SetActive(true);
            return;
        }

        if (matchInstance.nowModeType != MatchModeType.Melee)
        {
            var winData = baseResultObject.transform.GetChild(0).gameObject.GetComponentsInChildren<TMP_Text>();
            var loseData = baseResultObject.transform.GetChild(1).gameObject.GetComponentsInChildren<TMP_Text>();
            if (winData == null || loseData == null)
            {
                Debug.LogError("Result_Base UI �ҷ����� ����");
                return;
            }

            string winner = "";
            string loser = "";
            foreach (var user in matchGameResult.m_winners)
            {
                winner += matchInstance.GetNickNameBySessionId(user) + "\n";
            }

            foreach (var user in matchGameResult.m_losers)
            {
                loser += matchInstance.GetNickNameBySessionId(user) + "\n";
            }

            winData[1].text = winner;
            loseData[1].text = loser;
            Invoke("ShowResultBase", 0.8f);
        }
        else
        {
            var data = meleeResultObject.GetComponentsInChildren<TMP_Text>();
            if (data == null)
            {
                Debug.LogError("Result_Melee UI �ҷ����� ����");
                return;
            }

            string winner = "";
            foreach (var user in matchGameResult.m_winners)
            {
                winner += matchInstance.GetNickNameBySessionId(user) + "\n\n";
            }

            data[2].text = winner;
            Invoke("ShowResultMelee", 0.8f);
        }
    }

    private void ShowResultBase()
    {
        endLoadingObject.SetActive(false);
        baseResultObject.SetActive(true);
        meleeResultObject.SetActive(false);
        returnLobbyObject.SetActive(true);
    }

    private void ShowResultMelee()
    {
        endLoadingObject.SetActive(false);
        meleeResultObject.SetActive(true);
        baseResultObject.SetActive(false);
        returnLobbyObject.SetActive(true);
    }

    public void ReturnToMatchRobby()
    {
        if (fadeObject != null)
        {
            fadeObject.ProcessFadeOut(() =>
            {
                GameManager_Server.GetInstance().ChangeState(GameManager_Server.GameState.MatchLobby);
            });
        }
        else
        {
            GameManager_Server.GetInstance().ChangeState(GameManager_Server.GameState.MatchLobby);
        }
    }
}
