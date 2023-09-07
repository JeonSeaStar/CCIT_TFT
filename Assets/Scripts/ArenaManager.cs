using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaManager : MonoBehaviour
{
    public static ArenaManager instance;

    public List<FieldManager> fm;
    public enum RoundType
    {
        NONE = -1,
        READY,
        BATTLE,
        EVENT,
        OVERTIME,
        DUEL,
        DEAD,
        MAX
    };
    public RoundType roundType = RoundType.NONE;

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
        if (roundType == RoundType.READY)
        {

        }
        else if (roundType == RoundType.BATTLE)
        {

        }
        else if (roundType == RoundType.EVENT)
        {

        }
        else if (roundType == RoundType.OVERTIME)
        {

        }
        else if (roundType == RoundType.DUEL)
        {

        }
        else if (roundType == RoundType.DEAD)
        {

        }
    }
}