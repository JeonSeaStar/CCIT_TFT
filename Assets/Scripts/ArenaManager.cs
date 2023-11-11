using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    private static ArenaManager instance;
    public static ArenaManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ArenaManager>();
                if (instance == null)
                {
                    GameObject _arena = new GameObject();
                    _arena.name = "ArenaManager";
                    instance = _arena.AddComponent<ArenaManager>();
                    DontDestroyOnLoad(_arena);
                }
            }
            return instance;
        }
    }

    public List<FieldManager> fieldManagers;

    public int currentRound = 0;
    public int currentStage = 0;

    public List<PlayerPair> playerMatchingPair;

    public enum RoundType
    {
        None = -1,
        Deployment,     //배치
        Ready,          //대기
        Battle,         //전투
        Event,
        Overtime,
        Duel,           //None
        Dead,
        Max
    };
    public RoundType roundType = RoundType.None;

    public float deploymentTime = 30;
    public float readyTime = 6f;
    public float battleTime = 60f;
    public float battleOverTime = 60f;
    public float groundEventTime = 10000f;
    public float duelTime = 60f;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        //StartCoroutine(CalRoundTime(3));
    }

    IEnumerator CalRoundTime(float time, string nextRound = "Deployment")
    {
        yield return new WaitForSeconds(1f);
        var _restTime = time - 1f;
        if (_restTime == 0)
        {
            fieldManagers[0].InitializingRound();
            switch (roundType)
            {
                case RoundType.Deployment:
                    roundType = RoundType.Ready;
                    StartCoroutine(CalRoundTime(readyTime, "Battle"));
                    Debug.Log("현재 배치 라운드 입니다~!");
                    yield break;
                case RoundType.Ready:
                    if (nextRound == "Deployment") //다음 라운드가 배치 라운드인 경우
                    {
                        roundType = RoundType.Deployment;
                        StartCoroutine(CalRoundTime(deploymentTime));
                        Debug.Log("현재 배치 라운드 입니다~!");
                    }
                    else if (nextRound == "Battle") //다음 라운드가 전투 라운드인 경우
                    {
                        roundType = RoundType.Battle;
                        StartCoroutine(CalRoundTime(battleTime));
                        Debug.Log("현재 전투 라운드 입니다~!");
                    }
                    yield break;
                case RoundType.Battle:
                    if (fieldManagers[0].myFilePieceList.Count > 0 && fieldManagers[0].enemyFilePieceList.Count > 0)
                        roundType = RoundType.Overtime;
                    yield break;
                case RoundType.Overtime:
                    roundType = RoundType.Ready;
                    yield break;
            }
        }
        StartCoroutine(CalRoundTime(_restTime));
    }

    #region 매칭
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) { Matching(); }
        if (Input.GetKeyDown(KeyCode.S)) { InitMatchingHistory(); }
    }

    private void Matching()
    {
        float playerCountHalf = fieldManagers.Count / 2;
        int pairCount = Mathf.RoundToInt(playerCountHalf);

        List<Messenger> messengerList = new List<Messenger>();
        playerMatchingPair = new List<PlayerPair>();

        for (int i = 0; i < fieldManagers.Count; i++)
        {
            fieldManagers[i].owerPlyer.isExpedition = false;
            fieldManagers[i].owerPlyer.matchingInformation.pairings = false;
            messengerList.Add(fieldManagers[i].owerPlyer);
        }

        List<Messenger> aGroup = new List<Messenger>();
        List<Messenger> bGroup = new List<Messenger>();

        for (int i = 0; i < pairCount; i++)
        {
            int m = Random.Range(0, messengerList.Count);
            aGroup.Add(messengerList[m]);
            messengerList.RemoveAt(m);
        }
        bGroup = messengerList;

        for(int i = 0; i < aGroup.Count; i++)
        {
            DFS(aGroup[i], bGroup);
        }
    }

    private bool DFS(Messenger homePlayer, List<Messenger> awayPlayerList)
    {
        List<Messenger> canPairingsPlayer = new List<Messenger>();
        canPairingsPlayer = awayPlayerList;

        for (int i = 0; i < homePlayer.matchingInformation.matchingHistroy.Count; i++)
        {
            for (int j = 0; j < canPairingsPlayer.Count; j++)
            {
                if (homePlayer.matchingInformation.matchingHistroy[i] == canPairingsPlayer[j].matchingInformation.myIndex)
                {
                    canPairingsPlayer.RemoveAt(j);
                    break;
                }
            }
        }

        for (int i  = 0; i < canPairingsPlayer.Count; i++)
        {
            int t = canPairingsPlayer[i].matchingInformation.myIndex;
            Messenger targetPlayer = GetMessenger(t);
            if (targetPlayer.matchingInformation.pairings) continue;
            targetPlayer.matchingInformation.pairings = true;

            if(!homePlayer.matchingInformation.pairings || DFS(targetPlayer, awayPlayerList))
            {
                PlayerMatching(homePlayer, targetPlayer);
                return true;
            }
        }
        
        return false;
    }

    private bool CheckPairings(int playerIndex)
    {
        for(int i = 0; i < playerMatchingPair.Count; i++)
        {
            if (playerMatchingPair[i].awayPlayer.matchingInformation.myIndex == playerIndex)
                return true;
        }

        return false;
    }

    private Messenger GetMessenger(int index)
    {
        for(int i =0; i < fieldManagers.Count; i++)
        {
            if (fieldManagers[i].owerPlyer.matchingInformation.myIndex == index)
                return fieldManagers[i].owerPlyer;
        }

        return null;
    }

    private void InitMatchingHistory()
    {
        for (int i = 0; i < fieldManagers.Count; i++)
            fieldManagers[i].owerPlyer.matchingInformation.HistoryIniti();
    }

    private void PlayerMatching(Messenger home, Messenger away)
    {
        PlayerPair pair = new PlayerPair(home, away);
        playerMatchingPair.Add(pair);

        home.matchingInformation.matchingHistroy.Add(away.matchingInformation.myIndex);
        away.matchingInformation.matchingHistroy.Add(home.matchingInformation.myIndex);
    }
    #endregion
}

[System.Serializable]
public class PlayerPair
{
    public Messenger homePlayer;
    public Messenger awayPlayer;

    public PlayerPair(Messenger home, Messenger away)
    {
        homePlayer = home;
        awayPlayer = away;
    }
}