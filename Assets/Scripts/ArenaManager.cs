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

    public const float readyTime = 30f;               // �غ� �ð�
    public const float battleTime = 60f;              // ���� �ð�
    public const float battleOverTime = 60f;          // ������ �ð�
    public const float groundEventTime = 10000f;      // ���� �ð�
    public const float duelTime = 60f;
    
                                    
    

    private void Awake()
    {
        instance = this;

        //fm[0] = FieldManager.instance;
    }

}
