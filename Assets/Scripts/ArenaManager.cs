using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public static ArenaManager instance;

    public List<FieldManager> fm;
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
        instance = this;
        //StartCoroutine(CalRoundTime(3));
    }

    IEnumerator CalRoundTime(float time, string nextRound = "Deployment")
    {
        yield return new WaitForSeconds(1f);
        var _restTime = time - 1f;
        //Debug.Log(_restTime);

        if(_restTime == 0)
        {
            fm[0].InitializingRound();
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
                    if(fm[0].myFilePieceList.Count > 0 && fm[0].enemyFilePieceList.Count > 0)
                        roundType = RoundType.Overtime;
                    yield break;
                case RoundType.Overtime:
                    roundType = RoundType.Ready;
                    yield break;
            }
        }
        StartCoroutine(CalRoundTime(_restTime));
    }

    public void ChangeRoundType(RoundType roundType)
    {
        switch (roundType)
        {
            case RoundType.Ready:
                //Please Add Here
                break;
            case RoundType.Battle:
                break;
            case RoundType.Event:
                break;
            case RoundType.Overtime:
                break;
            case RoundType.Duel:
                break;
            case RoundType.Dead:
                break;
        }
    }
}