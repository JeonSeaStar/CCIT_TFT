//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ArenaManager;

public class FieldManager : MonoBehaviour
{
    public static FieldManager instance;

    public Messenger[] DualPlayers = new Messenger[2];

    public List<Transform> targetPositions = new List<Transform>();

    public List<Piece> myFilePieceList;
    public List<Piece> enemyFilePieceList;

    public PathFinding pathFinding;

    public GameObject[] readyZoneHexaIndicators;
    public GameObject[] battleFieldHexaIndicators;

    public bool grab;

    public Dictionary<PieceData.Mythology, int> SynergeMythology = new Dictionary<PieceData.Mythology, int>()
    {
        { PieceData.Mythology.NONE      ,0 },
        { PieceData.Mythology.A         ,0 },
        { PieceData.Mythology.B         ,0 },
        { PieceData.Mythology.C         ,0 },
        { PieceData.Mythology.D         ,0 },
        { PieceData.Mythology.E         ,0 }
    };
    public Dictionary<PieceData.Species, int> SynergeSpecies = new Dictionary<PieceData.Species, int>()
    {
        { PieceData.Species.NONE        ,0 },
        { PieceData.Species.HAMSTER     ,0 },
        { PieceData.Species.CAT         ,0 },
        { PieceData.Species.DOG         ,0 },
        { PieceData.Species.FROG        ,0 },
        { PieceData.Species.RABBIT      ,0 },
    };
    public Dictionary<PieceData.PlusSynerge, int> SynergePlusSynerge = new Dictionary<PieceData.PlusSynerge, int>()
    {
        { PieceData.PlusSynerge.NONE    ,0 },
        { PieceData.PlusSynerge.A       ,0 },
        { PieceData.PlusSynerge.B       ,0 },
        { PieceData.PlusSynerge.C       ,0 },
        { PieceData.PlusSynerge.D       ,0 },
        { PieceData.PlusSynerge.E       ,0 }
    };

    void Awake()
    {
        instance = this;
        ArenaManager.instance.fm[0] = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("SynergeMythology  A    시너지    = " + SynergeMythology[PieceData.Mythology.A] +
                      System.Environment.NewLine +
                      "SynergeMythology  B    시너지    = " + SynergeMythology[PieceData.Mythology.B] +
                      System.Environment.NewLine +
                      "SynergeMythology  C    시너지    = " + SynergeMythology[PieceData.Mythology.C] +
                      System.Environment.NewLine +
                      "SynergeMythology  D    시너지    = " + SynergeMythology[PieceData.Mythology.D] +
                      System.Environment.NewLine +
                      "SynergeMythology  E    시너지    = " + SynergeMythology[PieceData.Mythology.E]);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            ArenaManager.instance.roundType = RoundType.BATTLE;
        }

    }
    int d = 0;
    public Piece SpawnPiece(GameObject p, int star)
    {
        GameObject pieceGameObject = Instantiate(p, new Vector3(0, 0, 0), Quaternion.identity);
        pieceGameObject.name = "testPiece[" + d + "]";
        pieceGameObject.TryGetComponent(out Piece piece);
        piece.star = star;
        d++;
        return piece;
    }

    /// <summary>
    /// Hexa Icon ON / OFF
    /// </summary>
    /// <param name="isactive"></param>
    public void ActiveHexaIndicators(bool isactive)
    {
        if (ArenaManager.instance.roundType == RoundType.READY)
        {
            for (int i = 0; i < readyZoneHexaIndicators.Length; i++)
            {
                readyZoneHexaIndicators[i].SetActive(isactive);
            }
            for (int i = 0; i < battleFieldHexaIndicators.Length; i++)
            {
                battleFieldHexaIndicators[i].SetActive(isactive);
            }
        }
        if (ArenaManager.instance.roundType == RoundType.BATTLE)
        {
            for (int i = 0; i < readyZoneHexaIndicators.Length; i++)
            {
                readyZoneHexaIndicators[i].SetActive(isactive);
            }
        }
    }

    void LifeAttack(Messenger defeatedPlayer)
    {

    }
}