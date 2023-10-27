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
    private void Matching()
    {
        float playerCountHalf = fieldManagers.Count / 2;
        int pairCount = Mathf.RoundToInt(playerCountHalf);

        List<Messenger> messengerList = new List<Messenger>();

        for (int i = 0; i < fieldManagers.Count; i++)
            messengerList.Add(fieldManagers[i].owerPlyer);

        List<Messenger> aGroup = new List<Messenger>();
        List<Messenger> bGroup = new List<Messenger>();

        for (int i = 0; i < pairCount; i++)
        {
            int m = Random.Range(0, pairCount);
            aGroup.Add(messengerList[m]);
            messengerList.RemoveAt(m);
        }
        bGroup = messengerList;

        for (int i = 0; i < bGroup.Count; i++)
            bGroup[i].isExpedition = true;

        for (int i = 0; i < aGroup.Count; i++)
        {
            int m;
            bool duplicate = true;

            do
            {
                m = Random.Range(0, bGroup.Count);

                for (int j = 0; j < aGroup[i].matchingInformation.matchingHistroy.Count; j++)
                {
                    if (aGroup[i].matchingInformation.matchingHistroy[j] != bGroup[m].matchingInformation.myIndex)
                    {
                        duplicate = false;
                        //상대방 매칭
                        aGroup[i].matchingInformation.matchingHistroy.Add(bGroup[m].matchingInformation.myIndex);
                    }
                }
            }
            while (duplicate);
        }
    }

    private void InitMatchingHistory()
    {
        for (int i = 0; i < fieldManagers.Count; i++)
            fieldManagers[i].owerPlyer.matchingInformation.HistoryIniti();
    }
    #endregion
}