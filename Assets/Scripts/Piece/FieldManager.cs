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

    public Dictionary<PieceData.Myth, int> mythActiveCount = new Dictionary<PieceData.Myth, int>()
    {
        { PieceData.Myth.None                   ,0 },
        { PieceData.Myth.GreatMountain          ,0 },
        { PieceData.Myth.FrostyWind             ,0 },
        { PieceData.Myth.SandKingdom            ,0 },
        { PieceData.Myth.HeavenGround           ,0 },
        { PieceData.Myth.BurningGround          ,0 }
    };
    public Dictionary<PieceData.Animal, int> animalActiveCount = new Dictionary<PieceData.Animal, int>()
    {
        { PieceData.Animal.None        ,0 },
        { PieceData.Animal.Hamster     ,0 },
        { PieceData.Animal.Cat         ,0 },
        { PieceData.Animal.Dog         ,0 },
        { PieceData.Animal.Frog        ,0 },
        { PieceData.Animal.Rabbit      ,0 },
    };
    public Dictionary<PieceData.United, int> unitedActiveCount = new Dictionary<PieceData.United, int>()
    {
        { PieceData.United.None              ,0 },
        { PieceData.United.UnderWorld        ,0 },
        { PieceData.United.Faddist           ,0 },
        { PieceData.United.WarMachine        ,0 },
        { PieceData.United.Creature          ,0 }
    };

    //fot test
    public GameObject aa, bb;
    public Tile aaa, bbb,ccc;
    //

    void Awake()
    {
        instance = this;
        ArenaManager.instance.fm.Add(this);


        //for test
        var testa = GameObject.Instantiate(aa);
        var testb = GameObject.Instantiate(bb);
        var testc = GameObject.Instantiate(aa);
        testa.transform.position = new Vector3(0, 0, -1.7f);
        testb.transform.position = new Vector3(1, 0, -1.7f);
        testc.transform.position = new Vector3(2, 0, -1.7f);
        aaa.piece = testa;
        bbb.piece = testb;
        ccc.piece = testc;
        //
    }

        




    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("GreatMountain    시너지    = " + mythActiveCount[PieceData.Myth.GreatMountain] +
                      System.Environment.NewLine +
                      "FrostyWind       시너지    = " + mythActiveCount[PieceData.Myth.FrostyWind] +
                      System.Environment.NewLine +
                      "SandKingdom      시너지    = " + mythActiveCount[PieceData.Myth.SandKingdom] +
                      System.Environment.NewLine +
                      "HeavenGround     시너지    = " + mythActiveCount[PieceData.Myth.HeavenGround] +
                      System.Environment.NewLine +
                      "BurningGround    시너지    = " + mythActiveCount[PieceData.Myth.BurningGround]);
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


    public void SynergeIncrease(Piece piece)
    {
        mythActiveCount[piece.pieceData.myth]++;
        animalActiveCount[piece.pieceData.animal]++;
        unitedActiveCount[piece.pieceData.united]++;
    }

    public void SynergeDecrease(Piece piece)
    {
        mythActiveCount[piece.pieceData.myth]--;
        animalActiveCount[piece.pieceData.animal]--;
        unitedActiveCount[piece.pieceData.united]--;
    }


    void CalSynerge(Piece plus, Piece minus = null)
    {
        var _plusMyth = plus.pieceData.myth;
        var _minusMyth = (minus != null) ? minus.pieceData.myth : PieceData.Myth.None;
        var _plusAnimal = plus.pieceData.animal;
        var _minusAnimal = (minus != null) ? minus.pieceData.animal : PieceData.Animal.None;
        var _plusUnited = plus.pieceData.united;
        var _minusUnited = (minus != null) ? minus.pieceData.united : PieceData.United.None;

        if(minus == null)
        {
            if(plus.GetComponent<Tile>().isReadyTile) //Plus
            {
                //
            }
            else //Minus
            {
                //
            }
        }
        else
        {
            
        }
    }


}