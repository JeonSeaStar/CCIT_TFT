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

    public bool grab = false;

    public float groundHeight;

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
            InitializingRound();
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
    #region Calculate Synerge
    public void CalSynerge(Piece plus, Piece minus = null)
    {
        PieceData.Myth _plusMyth = plus.pieceData.myth;
        PieceData.Animal _plusAnimal = plus.pieceData.animal;
        PieceData.United _plusUnited = plus.pieceData.united;

        void ApplySynerge(PieceData.Myth myth, PieceData.Animal animal, PieceData.United united)
        {
            ApplyMythSynerge(myth);
            ApplyAnimalSynerge(animal);
            ApplyUnitedSynerge(united);
        }

        if (minus == null) //Set Piece
        {
            ApplySynerge(_plusMyth, _plusAnimal, _plusUnited);
        }
        else if (minus != null) //Change Piece
        {
            PieceData.Myth _minusMyth = minus.pieceData.myth;
            PieceData.Animal _minusAnimal = minus.pieceData.animal;
            PieceData.United _minusUnited = minus.pieceData.united;
            ApplySynerge(_plusMyth, _plusAnimal, _plusUnited);
            ApplySynerge(_minusMyth, _minusAnimal, _minusUnited);
        }
    }

    //Direct--------------------1
    //BattleStart---------------2
    //BattleInProgress----------3
    //OncePerAttack-------------4

    public BuffManager buffManager;
    void ApplyMythSynerge(PieceData.Myth value)
    {
        int _mythSynergeCount = mythActiveCount[value];
        switch (value)
        {
            case PieceData.Myth.GreatMountain://2
                ApplyMythBuff(buffManager.mythBuff[0].greatMoutainBuff, _mythSynergeCount, new int[] { 2, 4, 6, 8 }, value);
                break;
            case PieceData.Myth.FrostyWind://3
                ApplyMythBuff(buffManager.mythBuff[0].frostyWindBuff, _mythSynergeCount, new int[] { 3, 6, 9 }, value);
                break;
            case PieceData.Myth.SandKingdom://2//3
                ApplyMythBuff(buffManager.mythBuff[0].sandKingdomBuff, _mythSynergeCount, new int[] { 3, 5, 7 }, value);
                break;
            case PieceData.Myth.HeavenGround://1//3
                ApplyMythBuff(buffManager.mythBuff[0].heavenGroundBuff, _mythSynergeCount, new int[] { 2, 4 }, value);
                break;
            case PieceData.Myth.BurningGround://4
                ApplyMythBuff(buffManager.mythBuff[0].burningGroundBuff, _mythSynergeCount, new int[] { 2, 4 }, value);
                break;
        }
    }
    void ApplyAnimalSynerge(PieceData.Animal value)
    {
        int _animalSynergeCount = animalActiveCount[value];
        switch (value)
        {
            case PieceData.Animal.Hamster://3
                ApplyAnimalBuff(buffManager.animalBuff[0].hamsterBuff, _animalSynergeCount, new int[] { 2, 4, 6 }, value);
                break;
            case PieceData.Animal.Cat://4
                ApplyAnimalBuff(buffManager.animalBuff[0].catBuff, _animalSynergeCount, new int[] { 2, 4 }, value);
                break;
            case PieceData.Animal.Dog://2
                ApplyAnimalBuff(buffManager.animalBuff[0].dogBuff, _animalSynergeCount, new int[] { 3, 6, 9 }, value);
                break;
            case PieceData.Animal.Frog://1//2
                ApplyAnimalBuff(buffManager.animalBuff[0].frogBuff, _animalSynergeCount, new int[] { 2, 4, 6 }, value);
                break;
            case PieceData.Animal.Rabbit://4
                ApplyAnimalBuff(buffManager.animalBuff[0].rabbitBuff, _animalSynergeCount, new int[] { 3, 6, 9 }, value);
                break;
        }
    }
    void ApplyUnitedSynerge(PieceData.United value)
    {
        int _unitedSynergeCount = unitedActiveCount[value];
        switch (value)
        {
            case PieceData.United.UnderWorld://2//3
                ApplyUnitedBuff(buffManager.unitedBuff[0].underWorldBuff, _unitedSynergeCount, new int[] { 2, 4 }, value);
                break;
            case PieceData.United.Faddist://2
                ApplyUnitedBuff(buffManager.unitedBuff[0].FaddistBuff, _unitedSynergeCount, new int[] { 2, 4 }, value);
                break;
            case PieceData.United.WarMachine://1
                ApplyUnitedBuff(buffManager.unitedBuff[0].warMachineBuff, _unitedSynergeCount, new int[] { 2, 4 }, value);
                break;
            case PieceData.United.Creature://4
                ApplyUnitedBuff(buffManager.unitedBuff[0].creatureBuff, _unitedSynergeCount, new int[] { 3, 5, 7, 9 }, value);
                break;
        }
    }
    void ApplyMythBuff(List<BuffData> buffList, int count, int[] thresholds, PieceData.Myth mythType)
    {
        for (int i = 0; i < thresholds.Length; i++)
        {
            if (count >= thresholds[i])
            {
                foreach (var piece in myFilePieceList)
                {
                    if (piece.pieceData.myth == mythType)
                    {
                        if (i > 0)
                        {
                            for (int j = 0; j < i; j++)
                            {
                                if (piece.buffList.Contains(buffList[j])) piece.buffList.Remove(buffList[j]);
                                if(DualPlayers[0].buffDatas.Contains(buffList[j])) DualPlayers[0].buffDatas.Remove(buffList[j]);
                            }
                        }
                        if (!piece.buffList.Contains(buffList[i])) piece.buffList.Add(buffList[i]);
                        if (!DualPlayers[0].buffDatas.Contains(buffList[i])) DualPlayers[0].buffDatas.Add(buffList[i]);
                    }
                }
            }
            else if(count < thresholds[i])
            {
                foreach (var piece in myFilePieceList)
                {
                    if (piece.buffList.Contains(buffList[i])) piece.buffList.Remove(buffList[i]);
                    if (DualPlayers[0].buffDatas.Contains(buffList[i])) DualPlayers[0].buffDatas.Remove(buffList[i]);
                }
            }
        }
    }
    void ApplyAnimalBuff(List<BuffData> buffList, int count, int[] thresholds, PieceData.Animal animalType)
    {
        for (int i = 0; i < thresholds.Length; i++)
        {
            if (count >= thresholds[i])
            {
                foreach (var piece in myFilePieceList)
                {
                    if (piece.pieceData.animal == animalType)
                    {
                        if (i > 0)
                        {
                            for (int j = 0; j < i; j++)
                            {
                                if (piece.buffList.Contains(buffList[j])) piece.buffList.Remove(buffList[j]);
                                if (DualPlayers[0].buffDatas.Contains(buffList[j])) DualPlayers[0].buffDatas.Remove(buffList[j]);
                            }
                        }
                        if (!piece.buffList.Contains(buffList[i])) piece.buffList.Add(buffList[i]);
                        if (!DualPlayers[0].buffDatas.Contains(buffList[i])) DualPlayers[0].buffDatas.Add(buffList[i]);
                    }
                }
            }
            else if (count < thresholds[i])
            {
                foreach (var piece in myFilePieceList)
                {
                    if (piece.buffList.Contains(buffList[i])) piece.buffList.Remove(buffList[i]);
                    if (DualPlayers[0].buffDatas.Contains(buffList[i])) DualPlayers[0].buffDatas.Remove(buffList[i]);
                }
            }
        }
    }
    void ApplyUnitedBuff(List<BuffData> buffList, int count, int[] thresholds, PieceData.United unitedType)
    {
        for (int i = 0; i < thresholds.Length; i++)
        {
            if (count >= thresholds[i])
            {
                foreach (var piece in myFilePieceList)
                {
                    if (piece.pieceData.united == unitedType)
                    {
                        if (i > 0)
                        {
                            for (int j = 0; j < i; j++)
                            {
                                if (piece.buffList.Contains(buffList[j])) piece.buffList.Remove(buffList[j]);
                                if (DualPlayers[0].buffDatas.Contains(buffList[j])) DualPlayers[0].buffDatas.Remove(buffList[j]);
                            }
                        }
                        if (!piece.buffList.Contains(buffList[i])) piece.buffList.Add(buffList[i]);
                        if (!DualPlayers[0].buffDatas.Contains(buffList[i])) DualPlayers[0].buffDatas.Add(buffList[i]);
                    }
                }
            }
            else if (count < thresholds[i])
            {
                foreach (var piece in myFilePieceList)
                {
                    if (piece.buffList.Contains(buffList[i])) piece.buffList.Remove(buffList[i]);
                    if (DualPlayers[0].buffDatas.Contains(buffList[i])) DualPlayers[0].buffDatas.Remove(buffList[i]);
                }
            }
        }
    }

    #endregion
    public void InitializingRound()
    {
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
            if (_equipmentControl != null)
            {
                _equipmentControl.transform.position = new Vector3(_equipmentControl.originalPosition.x, 0, _equipmentControl.originalPosition.z);
                return;
            }
        }
        foreach (var test in myFilePieceList)
        {
            test.ExpeditionTileCheck();
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
        Vector3 targetPosition = new Vector3(targetTile.transform.position.x, groundHeight, targetTile.transform.position.z);
        GameObject pieceObject = Instantiate(pieceData.piecePrefab, targetPosition, Quaternion.Euler(0, 0, 0));
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