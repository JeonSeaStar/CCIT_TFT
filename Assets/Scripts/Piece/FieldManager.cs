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

<<<<<<< HEAD
    public bool grab;

    public Dictionary<PieceData.Mythology, int> SynergeMythology = new Dictionary<PieceData.Mythology, int>()
=======
    public Dictionary<PieceData.Myth, int> SynergeMythology = new Dictionary<PieceData.Myth, int>()
>>>>>>> c2678a28aff9b002bd45a879d25ab06f09590d5d
    {
        { PieceData.Myth.None                   ,0 },
        { PieceData.Myth.GreatMountain          ,0 },
        { PieceData.Myth.FrostyWind             ,0 },
        { PieceData.Myth.SandKingdom            ,0 },
        { PieceData.Myth.HeavenGround           ,0 },
        { PieceData.Myth.BurningGround          ,0 }
    };
    public Dictionary<PieceData.Animal, int> SynergeSpecies = new Dictionary<PieceData.Animal, int>()
    {
        { PieceData.Animal.None        ,0 },
        { PieceData.Animal.Hamster     ,0 },
        { PieceData.Animal.Cat         ,0 },
        { PieceData.Animal.Dog         ,0 },
        { PieceData.Animal.Frog        ,0 },
        { PieceData.Animal.Rabbit      ,0 },
    };
    public Dictionary<PieceData.United, int> SynergePlusSynerge = new Dictionary<PieceData.United, int>()
    {
        { PieceData.United.None              ,0 },
        { PieceData.United.UnderWorld        ,0 },
        { PieceData.United.Faddist           ,0 },
        { PieceData.United.WarMachine        ,0 },
        { PieceData.United.Creature          ,0 }
    };

    void Awake()
    {
        instance = this;
        ArenaManager.instance.fm.Add(this);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("SynergeMythology  A    시너지    = " + SynergeMythology[PieceData.Myth.GreatMountain] +
                      System.Environment.NewLine +
                      "SynergeMythology  B    시너지    = " + SynergeMythology[PieceData.Myth.FrostyWind] +
                      System.Environment.NewLine +
                      "SynergeMythology  C    시너지    = " + SynergeMythology[PieceData.Myth.SandKingdom] +
                      System.Environment.NewLine +
                      "SynergeMythology  D    시너지    = " + SynergeMythology[PieceData.Myth.HeavenGround] +
                      System.Environment.NewLine +
                      "SynergeMythology  E    시너지    = " + SynergeMythology[PieceData.Myth.BurningGround]);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            ArenaManager.instance.roundType = RoundType.Battle;
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
        if (ArenaManager.instance.roundType == RoundType.Ready)
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
        if (ArenaManager.instance.roundType == RoundType.Battle)
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