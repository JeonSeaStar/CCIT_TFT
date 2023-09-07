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

    public const float readyTime = 30f;               
    public const float battleTime = 60f;              
    public const float battleOverTime = 60f;          
    public const float groundEventTime = 10000f;      
    public const float duelTime = 60f;

    private void Awake()
    {
        instance = this;
    }

    public void ChangeRoundType(RoundType roundType)
    {
        if (roundType == RoundType.Ready)
        {

        }
        else if (roundType == RoundType.Battle)
        {

        }
        else if (roundType == RoundType.Event)
        {

        }
        else if (roundType == RoundType.Overtime)
        {

        }
        else if (roundType == RoundType.Duel)
        {

        }
        else if (roundType == RoundType.Dead)
        {

        }
    }
}