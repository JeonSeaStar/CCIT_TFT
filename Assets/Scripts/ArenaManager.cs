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
        Ready,
        Battle,
        Event,
        Overtime,
        Duel,
        Dead,
        Max
    };
    public RoundType roundType = RoundType.None;

    public float readyTime = 30f;               
    public float battleTime = 60f;              
    public float battleOverTime = 60f;          
    public float groundEventTime = 10000f;      
    public float duelTime = 60f;

    private void Awake()
    {
        instance = this;
    }

    public void ChangeRoundType(RoundType roundType)
    {
        switch(roundType)
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