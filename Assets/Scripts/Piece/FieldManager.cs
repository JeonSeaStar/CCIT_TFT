//using System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ArenaManager;

public class FieldManager : MonoBehaviour
{
    //public static FieldManager instance;

    public Messenger[] DualPlayers = new Messenger[2];

    public List<Transform> targetPositions = new List<Transform>();

    public List<Piece> myFilePieceList;
    public List<Piece> enemyFilePieceList;

    public PathFinding pathFinding;

    public List<Tile> readyTileList;
    public List<Tile> battleTileList;

    public GameObject[] readyZoneHexaIndicators;
    public GameObject[] battleFieldHexaIndicators;

    public bool grab, isDrag = false;
    public Piece controlPiece;

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
    public Tile aaa, bbb, ccc;
    //

    void Awake()
    {
        Synerge._fm = this;

        //for test
        //var testa = GameObject.Instantiate(aa);
        //var testb = GameObject.Instantiate(bb);
        //var testc = GameObject.Instantiate(aa);
        //testa.transform.position = new Vector3(0, 0, -1.7f);
        //testb.transform.position = new Vector3(1, 0, -1.7f);
        //testc.transform.position = new Vector3(2, 0, -1.7f);
        //aaa.piece = testa;
        //bbb.piece = testb;
        //ccc.piece = testc;
        //
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("GreatMountain시너지 = " + mythActiveCount[PieceData.Myth.GreatMountain] +
                      System.Environment.NewLine +
                      "FrostyWind시너지 = " + mythActiveCount[PieceData.Myth.FrostyWind] +
                      System.Environment.NewLine +
                      "SandKingdom 시너지 = " + mythActiveCount[PieceData.Myth.SandKingdom] +
                      System.Environment.NewLine +
                      "HeavenGround 시너지 = " + mythActiveCount[PieceData.Myth.HeavenGround] +
                      System.Environment.NewLine +
                      "BurningGround 시너지 = " + mythActiveCount[PieceData.Myth.BurningGround]);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            ArenaManager.Instance.roundType = RoundType.Battle;
            if (DualPlayers[0].isGrab)
            {
                DualPlayers[0].isDrag = true;
                object _controlObject = (DualPlayers[0].ControlPiece != null) ? DualPlayers[0].ControlPiece : DualPlayers[0].ControlEquipment;

                Piece _pieceControl = _controlObject as Piece;
                if (_pieceControl != null)
                {
                    var _transform = _pieceControl.currentTile.transform;
                    _pieceControl.transform.position = new Vector3(_transform.position.x, 0, _transform.position.z);
                    return;
                }

                Equipment _equipmentControl = _controlObject as Equipment;
                if(_equipmentControl != null)
                {
                    _pieceControl.transform.position = new Vector3(_equipmentControl.originPos.x, 0, _equipmentControl.originPos.z);
                    return;
                }
            }
                foreach (var test in myFilePieceList)
            {
                test.ExpeditionTileCheck();
            }
        }
        if (Input.GetKeyDown(KeyCode.R) && ArenaManager.Instance.roundType == RoundType.Battle)
        {
            ArenaManager.Instance.roundType = RoundType.Ready;
            //InitializingRound();
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
        if (ArenaManager.Instance.roundType == RoundType.Deployment)
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
        if (ArenaManager.Instance.roundType == RoundType.Battle)
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



    List<int> _mythCountCheck = new List<int>() { 2, 3, 4, 6, 9 };
    List<int> _animalCountCheck = new List<int>() { 2, 3, 4, 5, 6, 7, 8, 9 };
    List<int> _unitedCountCheck = new List<int>() { 2, 3, 4, 5, 7, 9 };
    public void CalSynerge(Piece plus, Piece minus = null)
    {
        PieceData.Myth _plusMyth = plus.pieceData.myth;
        PieceData.Animal _plusAnimal = plus.pieceData.animal;
        PieceData.United _plusUnited = plus.pieceData.united;

        int _mythCount = mythActiveCount[plus.pieceData.myth];
        int _animalCount = animalActiveCount[plus.pieceData.animal];
        int _unitedCount = unitedActiveCount[plus.pieceData.united];

        bool check = plus.GetComponent<PieceControl>().currentTile.isReadyTile;

        if (minus == null) //Set Piece
        {
            if (check) //Plus
            {
                if (_mythCountCheck.Contains(_mythCount))
                {
                    switch (_plusMyth)
                    {
                        case PieceData.Myth.GreatMountain:
                            Synerge.MythGreatMoutain(check);
                            break;
                        case PieceData.Myth.FrostyWind:
                            Synerge.MythFrostyWind(check);
                            break;
                        case PieceData.Myth.SandKingdom:
                            Synerge.MythSandKingdom(check);
                            break;
                        case PieceData.Myth.HeavenGround:
                            Synerge.MythHeavenGround(check);
                            break;
                        case PieceData.Myth.BurningGround:
                            Synerge.MythBurningGround(check);
                            break;
                    }
                }
                if (_animalCountCheck.Contains(_animalCount))
                {
                    switch (_plusAnimal)
                    {
                        case PieceData.Animal.Hamster:
                            Synerge.AnimalHamster(check);
                            break;
                        case PieceData.Animal.Cat:
                            Synerge.AnimalCat(check);
                            break;
                        case PieceData.Animal.Dog:
                            Synerge.AnimalDog(check);
                            break;
                        case PieceData.Animal.Frog:
                            Synerge.AnimalFrog(check);
                            break;
                        case PieceData.Animal.Rabbit:
                            Synerge.AnimalRabbit(check);
                            break;
                    }
                }
                if (_unitedCountCheck.Contains(_unitedCount))
                {
                    switch (_plusUnited)
                    {
                        case PieceData.United.UnderWorld:
                            Synerge.UnitedUnderWorld(check);
                            break;
                        case PieceData.United.Faddist:
                            Synerge.UnitedFaddist(check);
                            break;
                        case PieceData.United.WarMachine:
                            Synerge.UnitedWarMachine(check);
                            break;
                        case PieceData.United.Creature:
                            Synerge.UnitedCreature(check);
                            break;
                    }
                }
            }
            else //Minus
            {
                switch (_plusMyth)
                {
                    case PieceData.Myth.GreatMountain:
                        Synerge.MythGreatMoutain(check);
                        break;
                    case PieceData.Myth.FrostyWind:
                        Synerge.MythFrostyWind(check);
                        break;
                    case PieceData.Myth.SandKingdom:
                        Synerge.MythSandKingdom(check);
                        break;
                    case PieceData.Myth.HeavenGround:
                        Synerge.MythHeavenGround(check);
                        break;
                    case PieceData.Myth.BurningGround:
                        Synerge.MythBurningGround(check);
                        break;
                }
                switch (_plusAnimal)
                {
                    case PieceData.Animal.Hamster:
                        Synerge.AnimalHamster(check);
                        break;
                    case PieceData.Animal.Cat:
                        Synerge.AnimalCat(check);
                        break;
                    case PieceData.Animal.Dog:
                        Synerge.AnimalDog(check);
                        break;
                    case PieceData.Animal.Frog:
                        Synerge.AnimalFrog(check);
                        break;
                    case PieceData.Animal.Rabbit:
                        Synerge.AnimalRabbit(check);
                        break;
                }
                switch (_plusUnited)
                {
                    case PieceData.United.UnderWorld:
                        Synerge.UnitedUnderWorld(check);
                        break;
                    case PieceData.United.Faddist:
                        Synerge.UnitedFaddist(check);
                        break;
                    case PieceData.United.WarMachine:
                        Synerge.UnitedWarMachine(check);
                        break;
                    case PieceData.United.Creature:
                        Synerge.UnitedCreature(check);
                        break;
                }
            }
        }
        else //Change Piece
        {
            PieceData.Myth _minusMyth = (minus != null) ? minus.pieceData.myth : PieceData.Myth.None;
            PieceData.Animal _minusAnimal = (minus != null) ? minus.pieceData.animal : PieceData.Animal.None;
            PieceData.United _minusUnited = (minus != null) ? minus.pieceData.united : PieceData.United.None;

            if (check) //Plus
            {
                switch (_plusMyth)
                {
                    case PieceData.Myth.GreatMountain:
                        Synerge.MythGreatMoutain(check);
                        break;
                    case PieceData.Myth.FrostyWind:
                        Synerge.MythFrostyWind(check);
                        break;
                    case PieceData.Myth.SandKingdom:
                        Synerge.MythSandKingdom(check);
                        break;
                    case PieceData.Myth.HeavenGround:
                        Synerge.MythHeavenGround(check);
                        break;
                    case PieceData.Myth.BurningGround:
                        Synerge.MythBurningGround(check);
                        break;
                }
                switch (_minusMyth)
                {
                    case PieceData.Myth.GreatMountain:
                        Synerge.MythGreatMoutain(!check);
                        break;
                    case PieceData.Myth.FrostyWind:
                        Synerge.MythFrostyWind(!check);
                        break;
                    case PieceData.Myth.SandKingdom:
                        Synerge.MythSandKingdom(!check);
                        break;
                    case PieceData.Myth.HeavenGround:
                        Synerge.MythHeavenGround(!check);
                        break;
                    case PieceData.Myth.BurningGround:
                        Synerge.MythBurningGround(!check);
                        break;
                }


                switch (_plusAnimal)
                {
                    case PieceData.Animal.Hamster:
                        Synerge.AnimalHamster(check);
                        break;
                    case PieceData.Animal.Cat:
                        Synerge.AnimalCat(check);
                        break;
                    case PieceData.Animal.Dog:
                        Synerge.AnimalDog(check);
                        break;
                    case PieceData.Animal.Frog:
                        Synerge.AnimalFrog(check);
                        break;
                    case PieceData.Animal.Rabbit:
                        Synerge.AnimalRabbit(check);
                        break;
                }
                switch (_minusAnimal)
                {
                    case PieceData.Animal.Hamster:
                        Synerge.AnimalHamster(!check);
                        break;
                    case PieceData.Animal.Cat:
                        Synerge.AnimalCat(!check);
                        break;
                    case PieceData.Animal.Dog:
                        Synerge.AnimalDog(!check);
                        break;
                    case PieceData.Animal.Frog:
                        Synerge.AnimalFrog(!check);
                        break;
                    case PieceData.Animal.Rabbit:
                        Synerge.AnimalRabbit(!check);
                        break;
                }


                switch (_plusUnited)
                {
                    case PieceData.United.UnderWorld:
                        Synerge.UnitedUnderWorld(check);
                        break;
                    case PieceData.United.Faddist:
                        Synerge.UnitedFaddist(check);
                        break;
                    case PieceData.United.WarMachine:
                        Synerge.UnitedWarMachine(check);
                        break;
                    case PieceData.United.Creature:
                        Synerge.UnitedCreature(check);
                        break;
                }
                switch (_minusUnited)
                {
                    case PieceData.United.UnderWorld:
                        Synerge.UnitedUnderWorld(!check);
                        break;
                    case PieceData.United.Faddist:
                        Synerge.UnitedFaddist(!check);
                        break;
                    case PieceData.United.WarMachine:
                        Synerge.UnitedWarMachine(!check);
                        break;
                    case PieceData.United.Creature:
                        Synerge.UnitedCreature(!check);
                        break;
                }
            }
            else
            {
                switch (_plusMyth)
                {
                    case PieceData.Myth.GreatMountain:
                        Synerge.MythGreatMoutain(!check);
                        break;
                    case PieceData.Myth.FrostyWind:
                        Synerge.MythFrostyWind(!check);
                        break;
                    case PieceData.Myth.SandKingdom:
                        Synerge.MythSandKingdom(!check);
                        break;
                    case PieceData.Myth.HeavenGround:
                        Synerge.MythHeavenGround(!check);
                        break;
                    case PieceData.Myth.BurningGround:
                        Synerge.MythBurningGround(!check);
                        break;
                }
                switch (_minusMyth)
                {
                    case PieceData.Myth.GreatMountain:
                        Synerge.MythGreatMoutain(check);
                        break;
                    case PieceData.Myth.FrostyWind:
                        Synerge.MythFrostyWind(check);
                        break;
                    case PieceData.Myth.SandKingdom:
                        Synerge.MythSandKingdom(check);
                        break;
                    case PieceData.Myth.HeavenGround:
                        Synerge.MythHeavenGround(check);
                        break;
                    case PieceData.Myth.BurningGround:
                        Synerge.MythBurningGround(check);
                        break;
                }


                switch (_plusAnimal)
                {
                    case PieceData.Animal.Hamster:
                        Synerge.AnimalHamster(!check);
                        break;
                    case PieceData.Animal.Cat:
                        Synerge.AnimalCat(!check);
                        break;
                    case PieceData.Animal.Dog:
                        Synerge.AnimalDog(!check);
                        break;
                    case PieceData.Animal.Frog:
                        Synerge.AnimalFrog(!check);
                        break;
                    case PieceData.Animal.Rabbit:
                        Synerge.AnimalRabbit(!check);
                        break;
                }
                switch (_minusAnimal)
                {
                    case PieceData.Animal.Hamster:
                        Synerge.AnimalHamster(check);
                        break;
                    case PieceData.Animal.Cat:
                        Synerge.AnimalCat(check);
                        break;
                    case PieceData.Animal.Dog:
                        Synerge.AnimalDog(check);
                        break;
                    case PieceData.Animal.Frog:
                        Synerge.AnimalFrog(check);
                        break;
                    case PieceData.Animal.Rabbit:
                        Synerge.AnimalRabbit(check);
                        break;
                }


                switch (_plusUnited)
                {
                    case PieceData.United.UnderWorld:
                        Synerge.UnitedUnderWorld(!check);
                        break;
                    case PieceData.United.Faddist:
                        Synerge.UnitedFaddist(!check);
                        break;
                    case PieceData.United.WarMachine:
                        Synerge.UnitedWarMachine(!check);
                        break;
                    case PieceData.United.Creature:
                        Synerge.UnitedCreature(!check);
                        break;
                }
                switch (_minusUnited)
                {
                    case PieceData.United.UnderWorld:
                        Synerge.UnitedUnderWorld(check);
                        break;
                    case PieceData.United.Faddist:
                        Synerge.UnitedFaddist(check);
                        break;
                    case PieceData.United.WarMachine:
                        Synerge.UnitedWarMachine(check);
                        break;
                    case PieceData.United.Creature:
                        Synerge.UnitedCreature(check);
                        break;
                }
            }
        }
    }

    //Methods Needs to be run once at the start of the battle round
    public delegate void BattleBuff();
    BattleBuff sOnceBuff;

    //Methods Needs to be run multiple times during the battle round
    public delegate IEnumerator CoroutineBuff();
    CoroutineBuff sCoroutineBuff;

    public void InitializingRound()
    {
        if (DualPlayers[0].isGrab)
        {
            DualPlayers[0].isDrag = true;
            var _transform = controlPiece.GetComponent<PieceControl>().currentTile.transform;
            controlPiece.transform.position = new Vector3(_transform.position.x, 0, _transform.position.z);

            for (int i = 0; i < battleFieldHexaIndicators.Length; i++)
            {
                battleFieldHexaIndicators[i].SetActive(false);
            }
        }
    }

    [System.Serializable]
    public class PieceCount
    {
        public List<Piece> count = new List<Piece>();
    }

    [System.Serializable]
    public class PieceList
    {
        public PieceData pieceData;
        public List<PieceCount> count = new List<PieceCount>();
    }
    public List<PieceList> currentPieceList;

    public void PieceListCountUp(Piece piece)
    {
        int kind = CheckPieceKind(piece.pieceData);
        int grade = piece.grade;

        currentPieceList[kind].count[grade].count.Add(piece);

        if (currentPieceList[kind].count[grade].count.Count >= 3)
        {
            FusionPiece(piece);
        }
    }

    public void PieceListCountDown(Piece piece)
    {
        int kind = CheckPieceKind(piece.pieceData);
        int grade = piece.grade;

        currentPieceList[kind].count[grade].count.Remove(piece);
    }

    public int CheckPieceKind(PieceData pieceData)
    {
        int kind = -1;

        for (int i = 0; i < currentPieceList.Count; i++)
        {
            if (currentPieceList[i].pieceData == pieceData)
            {
                kind = i;
                break;
            }
        }

        return kind;
    }

    public void SpawnPiece(PieceData pieceData, int grade, Tile targetTile)
    {
        GameObject pieceObject = Instantiate(pieceData.piecePrefab, targetTile.transform.position, Quaternion.Euler(-90, 180, 0));
        Piece piece = pieceObject.GetComponent<Piece>();
        piece.currentTile = targetTile;
        piece.grade = grade;

        targetTile.IsFull = true;

        //나중에 지울 거
        piece.pieceData = pieceData;
        //

        PieceListCountUp(piece);
    }

    void FusionPiece(Piece piece)
    {
        int kind = CheckPieceKind(piece.pieceData);
        int grade = piece.grade;

        Piece parentPiece = piece;
        Piece firstChild = null;
        Piece secondChild = null;

        Tile parentTile = parentPiece.currentTile;

        //set parentPiece
        for (int i = 0; i < currentPieceList[kind].count[grade].count.Count; i++)
        {
            Tile compareTile = currentPieceList[kind].count[grade].count[i].currentTile;

            if(parentTile.isReadyTile)
            {
                if (!compareTile.isReadyTile)
                {
                    parentPiece = currentPieceList[kind].count[grade].count[i];
                    parentTile = parentPiece.currentTile;
                }
                else if (compareTile.isReadyTile)
                {
                    if (readyTileList.IndexOf(parentTile) > readyTileList.IndexOf(compareTile))
                    {
                        parentPiece = currentPieceList[kind].count[grade].count[i];
                        parentTile = parentPiece.currentTile;
                    }
                }
            }

            if (parentPiece == currentPieceList[kind].count[grade].count[i]) continue;

            if(!parentTile.isReadyTile && !compareTile.isReadyTile)
            {
                if (parentTile.gridX > compareTile.gridX)
                {
                    parentPiece = currentPieceList[kind].count[grade].count[i];
                    parentTile = parentPiece.currentTile;
                }
                else if (parentTile.gridX == compareTile.gridX)
                {
                    if (parentTile.gridY > compareTile.gridY)
                    {
                        parentPiece = currentPieceList[kind].count[grade].count[i];
                        parentTile = parentPiece.currentTile;
                    }
                }
            }
        }
        currentPieceList[kind].count[grade].count.Remove(parentPiece);


        //set firstChild
        firstChild = GetChildPiece(kind, grade);

        //set secondChild
        secondChild = GetChildPiece(kind, grade);

        Tile targetTile = parentPiece.currentTile;

        //fusion
        print("parentPiece: " + targetTile.gameObject.name);
        print("firstChild: " + firstChild.currentTile.gameObject.name);
        print("secondChild: " + secondChild.currentTile.gameObject.name);

        DestroyPiece(parentPiece, targetTile);
        DestroyPiece(firstChild, firstChild.currentTile);
        DestroyPiece(secondChild, secondChild.currentTile);
        SpawnPiece(piece.pieceData, grade + 1, targetTile);
    }

    Piece GetChildPiece(int kind, int grade)
    {
        Piece childPiece = currentPieceList[kind].count[grade].count[0];

        for (int i = 0; i < currentPieceList[kind].count[grade].count.Count; i++)
        {
            Tile parentTile = childPiece.currentTile;
            Tile compareTile = currentPieceList[kind].count[grade].count[i].currentTile;

            if (!parentTile.isReadyTile && compareTile.isReadyTile)
                childPiece = currentPieceList[kind].count[grade].count[i];
            else if (parentTile.isReadyTile && compareTile.isReadyTile)
            {
                if (readyTileList.IndexOf(parentTile) < readyTileList.IndexOf(compareTile))
                    childPiece = currentPieceList[kind].count[grade].count[i];
            }

            if (!parentTile.isReadyTile && !compareTile.isReadyTile)
            {
                if (parentTile.gridX < compareTile.gridX)
                    childPiece = currentPieceList[kind].count[grade].count[i];
                else if (parentTile.gridX == compareTile.gridX)
                {
                    if (parentTile.gridY > compareTile.gridY)
                        childPiece = currentPieceList[kind].count[grade].count[i];
                }
            }
        }
        currentPieceList[kind].count[grade].count.Remove(childPiece);
        return childPiece;
    }

    public void DestroyPiece(Piece piece, Tile targetTile)
    {
        targetTile.IsFull = false;
        CheckPieceKind(piece.pieceData);
        Destroy(piece.gameObject);
    }

    public Tile GetTile()
    {
        Tile targetTile = null;

        targetTile = TileCheck(targetTile, readyTileList);
        if (targetTile != null) { return targetTile; }

        targetTile = TileCheck(targetTile, battleTileList);
        if (targetTile != null) { return targetTile; }
        else return null;
    }

    Tile TileCheck(Tile tile, List<Tile> tileArray)
    {
        foreach (Tile tileObject in tileArray)
        {
            if (!tileObject.IsFull)
            {
                tile = tileObject;
                break;
            }
        }

        return tile;
    }

    public Chest chest;
}