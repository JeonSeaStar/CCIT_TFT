//using System;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static ArenaManager;

public class FieldManager : MonoBehaviour
{
    //public static FieldManager instance;

    public Messenger owerPlyer;
    public Messenger[] DualPlayers = new Messenger[2];

    public List<Transform> targetPositions = new List<Transform>();

    [System.Serializable]
    public class PieceDPList
    {
        public Piece piece;
        public Tile dpTile;

        public PieceDPList(Piece piece, Tile dpTile)
        {
            this.piece = piece;
            this.dpTile = dpTile;
        }
    }

    public List<PieceDPList> pieceDpList;

    [Header("아군 전투 유닛")] public List<Piece> myFilePieceList;
    [Header("상대 전투 유닛")] public List<Piece> enemyFilePieceList;
    [Header("아이템 소지 목록")] public List<Equipment> myEquipmentList;
    [Header("아군 기물")] public Transform pieceParent;
    [Header("상대 기물")] public Transform enemyParent;

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

    [System.Serializable]
    public class EnemyInformation
    {
        public GameObject piece;
        public Vector2 spawnTile;
    }

    [System.Serializable]
    public class StageInformation
    {
        public List<EnemyInformation> enemyInformation;
    }

    public List<StageInformation> stageInformation;
    public int currentStage;

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

        if (Input.GetKeyDown(KeyCode.B) && ArenaManager.Instance.roundType != RoundType.Battle)
        {
            ArenaManager.Instance.roundType = RoundType.Battle;
            foreach (var effect in sBattleStartEffect) effect(true);
            foreach (var effect in sCoroutineEffect) effect();

            Debug.Log("즉시 발동 시너지 효과 : " + sBattleStartEffect.Count);
            Debug.Log("지연 발동 시너지 효과 : " + sCoroutineEffect.Count);
            InitializingRound();
        }

        if(Input.GetKeyDown(KeyCode.N) && ArenaManager.Instance.roundType == RoundType.Battle)
        {
            ArenaManager.Instance.roundType = RoundType.Deployment;
            foreach (var effect in sBattleStartEffect) effect(false);
            StopAllCoroutines();
        }


        if (Input.GetKeyDown(KeyCode.R) && ArenaManager.Instance.roundType == RoundType.Battle)
        {
            ArenaManager.Instance.roundType = RoundType.Ready;
            //InitializingRound();
        }
    }

    public void AddDPList(Piece target)
    {
        PieceDPList pieceDP = new PieceDPList(target, target.targetTile);
        pieceDpList.Add(pieceDP);
    }

    public void RemoveDPList(Piece target)
    {
        for (int i = 0; i < pieceDpList.Count; i++)
        {
            if (pieceDpList[i].piece == target)
            {
                pieceDpList.RemoveAt(i);
                return;
            }
        }
    }

    public void FieldInit()
    {
        foreach (Piece piece in myFilePieceList)
            piece.gameObject.SetActive(false);

        foreach (Piece piece in enemyFilePieceList)
            piece.gameObject.SetActive(false);

        foreach (Tile tile in battleTileList)
        {
            tile.piece = null;
            tile.IsFull = false;
            tile.walkable = true;
        }

        foreach (PieceDPList dp in pieceDpList)
        {
            dp.dpTile.piece = dp.piece;
            dp.dpTile.IsFull = true;
            dp.dpTile.walkable = false;

            dp.piece.currentTile = dp.dpTile;
            dp.piece.targetTile = dp.dpTile;
            dp.piece.dead = false;
            dp.piece.target = null;
            dp.piece.candidatePath = null;
            dp.piece.path = null;
            dp.piece.target = null;

            dp.piece.transform.position = new Vector3(dp.dpTile.transform.position.x, 0, dp.dpTile.transform.position.z);

            dp.piece.gameObject.SetActive(true);
        }

        foreach (Piece piece in enemyFilePieceList)
            Destroy(piece.gameObject);
        enemyFilePieceList = new List<Piece>();
    }

    public void SpawnEnemy(int stage)
    {
        for (int i = 0; i < stageInformation[stage].enemyInformation.Count; i++)
        {
            int tileX = ((int)stageInformation[stage].enemyInformation[i].spawnTile.x);
            int tileY = ((int)stageInformation[stage].enemyInformation[i].spawnTile.y);

            GameObject enemyGameObject = Instantiate(stageInformation[stage].enemyInformation[i].piece, Vector3.zero, Quaternion.identity);
            enemyGameObject.transform.parent = enemyParent;

            Piece enemyPiece = enemyGameObject.GetComponent<Piece>();
            Tile targetTile = pathFinding.grid[tileX].tile[tileY];

            targetTile.piece = enemyPiece;
            targetTile.IsFull = true;
            targetTile.walkable = false;

            enemyPiece.currentTile = targetTile;
            enemyPiece.targetTile = targetTile;

            enemyGameObject.transform.position = new Vector3(targetTile.transform.position.x, 0, targetTile.transform.position.z);

            enemyFilePieceList.Add(enemyPiece);
        }
    }

    public void NextStage()
    {
        FieldInit();

        ArenaManager.Instance.roundType = RoundType.Deployment;
        foreach (var effect in sBattleStartEffect) effect(false);
        StopAllCoroutines();

        SpawnEnemy(currentStage);
        currentStage++;
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
    #region Calculate Synerge
    public void SynergeIncrease(Piece piece)
    {
        mythActiveCount[piece.pieceData.myth]++;
        animalActiveCount[piece.pieceData.animal]++;
        //unitedActiveCount[piece.pieceData.united]++;
    }
    public void SynergeDecrease(Piece piece)
    {
        mythActiveCount[piece.pieceData.myth]--;
        animalActiveCount[piece.pieceData.animal]--;
        //unitedActiveCount[piece.pieceData.united]--;
    }
    public void CalSynerge(Piece plus, Piece minus = null)
    {
        PieceData.Myth _plusMyth = plus.pieceData.myth;
        PieceData.Animal _plusAnimal = plus.pieceData.animal;
        PieceData.United _plusUnited = plus.pieceData.united;

        void ApplySynerge(PieceData.Myth myth, PieceData.Animal animal, PieceData.United united)
        {
            ApplyMythSynerge(myth);
            ApplyAnimalSynerge(animal);
            //ApplyUnitedSynerge(united);
        }

        if (minus == null) //Set Piece
        {
            ApplySynerge(_plusMyth, _plusAnimal, _plusUnited);

            SynergeBoard.instance.SynergeCountUpdate(_plusMyth, _plusAnimal, _plusUnited, true);
        }
        else if (minus != null) //Change Piece
        {
            PieceData.Myth _minusMyth = minus.pieceData.myth;
            PieceData.Animal _minusAnimal = minus.pieceData.animal;
            PieceData.United _minusUnited = minus.pieceData.united;
            ApplySynerge(_plusMyth, _plusAnimal, _plusUnited);
            ApplySynerge(_minusMyth, _minusAnimal, _minusUnited);

            SynergeBoard.instance.SynergeCountUpdate(_plusMyth, _plusAnimal, _plusUnited, true);
            SynergeBoard.instance.SynergeCountUpdate(_minusMyth, _minusAnimal, _minusUnited, false);
        }
    }

    //Direct--------------------1
    //BattleStart---------------2
    //BattleInProgress----------3
    //OncePerAttack-------------4

    public delegate void BattleStartEffect(bool isAdd); public HashSet<BattleStartEffect> sBattleStartEffect = new HashSet<BattleStartEffect>();
    public void AddBattleStartEffect(BattleStartEffect handler) { sBattleStartEffect.Add(handler); }
    public void RemoveBattleStartEffect(BattleStartEffect handler) { sBattleStartEffect.Remove(handler); }

    public delegate void CoroutineEffect(); public HashSet<CoroutineEffect> sCoroutineEffect = new HashSet<CoroutineEffect>();
    void AddCoroutine(CoroutineEffect handler) { sCoroutineEffect.Add(handler); }
    void RemoveCoroutine(CoroutineEffect handler) { sCoroutineEffect.Remove(handler); }

    public BuffManager buffManager;
    void ApplyMythSynerge(PieceData.Myth value)
    {
        int _mythSynergeCount = mythActiveCount[value];
        switch (value)
        {
            case PieceData.Myth.GreatMountain://2
                ApplyMythBuff(buffManager.mythBuff[0].greatMoutainBuff, _mythSynergeCount, new int[] { 3, 6, 9 }, value);
                break;
            case PieceData.Myth.FrostyWind://3
                ApplyMythBuff(buffManager.mythBuff[0].frostyWindBuff, _mythSynergeCount, new int[] { 3, 6, 9 }, value);
                break;
            case PieceData.Myth.SandKingdom://1//3
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
                ApplyAnimalBuff(buffManager.animalBuff[0].catBuff, _animalSynergeCount, new int[] { 2, 4, 6, 8 }, value);
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
    /*void ApplyUnitedSynerge(PieceData.United value)
    {
        int _unitedSynergeCount = unitedActiveCount[value];
        switch (value)
        {
            case PieceData.United.UnderWorld://2//3
                ApplyUnitedBuff(buffManager.unitedBuff[0].underWorldBuff, _unitedSynergeCount, new int[] { 2, 4 }, value);
                break;
            case PieceData.United.Faddist://2
                ApplyUnitedBuff(buffManager.unitedBuff[0].faddistBuff, _unitedSynergeCount, new int[] { 2, 4 }, value);
                break;
            case PieceData.United.WarMachine://1
                ApplyUnitedBuff(buffManager.unitedBuff[0].warMachineBuff, _unitedSynergeCount, new int[] { 2, 4 }, value);
                break;
            case PieceData.United.Creature://4
                ApplyUnitedBuff(buffManager.unitedBuff[0].creatureBuff, _unitedSynergeCount, new int[] { 3, 5, 7, 9 }, value);
                break;
        }
    }
    */

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
                        if (i > 0 && piece.buffList.Contains(buffList[i - 1]))
                        {
                            piece.buffList.Remove(buffList[i - 1]);
                            switch (mythType)
                            {
                                case PieceData.Myth.GreatMountain:
                                    RemoveBattleStartEffect(buffManager.mythBuff[0].greatMoutainBuff[i - 1].BattleStartEffect);
                                    break;
                                case PieceData.Myth.FrostyWind:
                                    RemoveCoroutine(buffManager.mythBuff[0].frostyWindBuff[i - 1].CoroutineEffect);
                                    break;
                                case PieceData.Myth.SandKingdom:
                                    buffManager.mythBuff[0].sandKingdomBuff[i - 1].DirectEffect(piece, false);
                                    RemoveBattleStartEffect(buffManager.mythBuff[0].sandKingdomBuff[i - 1].BattleStartEffect);
                                    RemoveCoroutine(buffManager.mythBuff[0].sandKingdomBuff[i - 1].CoroutineEffect);
                                    break;
                                case PieceData.Myth.HeavenGround:
                                    buffManager.mythBuff[0].heavenGroundBuff[i - 1].DirectEffect(piece, false);
                                    RemoveCoroutine(buffManager.mythBuff[0].heavenGroundBuff[i - 1].CoroutineEffect);
                                    break;
                                case PieceData.Myth.BurningGround:
                                    //buffManager.mythBuff[0].burningGroundBuff[i - 1].DirectEffect(piece, false);
                                    break;
                            }
                        }
                        if (i > 0 && DualPlayers[0].buffDatas.Contains(buffList[i - 1])) DualPlayers[0].buffDatas.Remove(buffList[i - 1]);

                        if (!piece.buffList.Contains(buffList[i]))
                        {
                            piece.buffList.Add(buffList[i]);
                            switch (mythType)
                            {
                                case PieceData.Myth.GreatMountain:
                                    AddBattleStartEffect(buffManager.mythBuff[0].greatMoutainBuff[i].BattleStartEffect);
                                    break;
                                case PieceData.Myth.FrostyWind:
                                    AddCoroutine(buffManager.mythBuff[0].frostyWindBuff[i].CoroutineEffect);
                                    break;
                                case PieceData.Myth.SandKingdom:
                                    buffManager.mythBuff[0].sandKingdomBuff[i].DirectEffect(piece, true);
                                    AddBattleStartEffect(buffManager.mythBuff[0].sandKingdomBuff[i].BattleStartEffect);
                                    AddCoroutine(buffManager.mythBuff[0].sandKingdomBuff[i].CoroutineEffect);
                                    break;
                                case PieceData.Myth.HeavenGround:
                                    buffManager.mythBuff[0].heavenGroundBuff[i].DirectEffect(piece, true);
                                    AddCoroutine(buffManager.mythBuff[0].heavenGroundBuff[i].CoroutineEffect);
                                    break;
                                case PieceData.Myth.BurningGround:
                                    //buffManager.mythBuff[0].burningGroundBuff[i].DirectEffect(piece, true);
                                    break;
                            }
                        }
                        if (!DualPlayers[0].buffDatas.Contains(buffList[i])) DualPlayers[0].buffDatas.Add(buffList[i]);
                    }
                }
            }
            else if (count < thresholds[i])
            {
                foreach (var piece in myFilePieceList)
                {
                    if (piece.buffList.Contains(buffList[i]))
                    {
                        piece.buffList.Remove(buffList[i]);
                        switch (mythType)
                        {
                            case PieceData.Myth.GreatMountain:
                                RemoveBattleStartEffect(buffManager.mythBuff[0].greatMoutainBuff[i].BattleStartEffect);
                                break;
                            case PieceData.Myth.FrostyWind:
                                RemoveCoroutine(buffManager.mythBuff[0].frostyWindBuff[i].CoroutineEffect);
                                break;
                            case PieceData.Myth.SandKingdom:
                                buffManager.mythBuff[0].sandKingdomBuff[i].DirectEffect(piece, false);
                                RemoveBattleStartEffect(buffManager.mythBuff[0].sandKingdomBuff[i].BattleStartEffect);
                                RemoveCoroutine(buffManager.mythBuff[0].sandKingdomBuff[i].CoroutineEffect);
                                break;
                            case PieceData.Myth.HeavenGround:
                                buffManager.mythBuff[0].heavenGroundBuff[i].DirectEffect(piece, false);
                                RemoveCoroutine(buffManager.mythBuff[0].heavenGroundBuff[i].CoroutineEffect);
                                break;
                            case PieceData.Myth.BurningGround:
                                //buffManager.mythBuff[0].burningGroundBuff[i - 1].DirectEffect(piece, false);
                                break;
                        }
                    }
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
                        //Remove
                        if (i > 0 && piece.buffList.Contains(buffList[i - 1]))
                        {
                            piece.buffList.Remove(buffList[i - 1]);
                            switch (animalType)
                            {
                                case PieceData.Animal.Hamster:
                                    RemoveBattleStartEffect(buffManager.animalBuff[0].hamsterBuff[i - 1].BattleStartEffect);
                                    RemoveCoroutine(buffManager.animalBuff[0].hamsterBuff[i - 1].CoroutineEffect);
                                    break;
                                case PieceData.Animal.Cat:
                                    buffManager.animalBuff[0].catBuff[i - 1].DirectEffect(piece, false);
                                    break;
                                case PieceData.Animal.Dog:
                                    buffManager.animalBuff[0].dogBuff[i - 1].DirectEffect(piece, false);
                                    RemoveBattleStartEffect(buffManager.animalBuff[0].dogBuff[i - 1].BattleStartEffect);
                                    break;
                                case PieceData.Animal.Frog:
                                    buffManager.animalBuff[0].frogBuff[i - 1].DirectEffect(piece, false);
                                    RemoveBattleStartEffect(buffManager.animalBuff[0].frogBuff[i - 1].BattleStartEffect);
                                    break;
                                case PieceData.Animal.Rabbit:
                                    RemoveCoroutine(buffManager.animalBuff[0].rabbitBuff[i - 1].CoroutineEffect);
                                    break;
                            }
                        }
                        if (i > 0 && DualPlayers[0].buffDatas.Contains(buffList[i - 1])) DualPlayers[0].buffDatas.Remove(buffList[i - 1]);
                        //Add
                        if (!piece.buffList.Contains(buffList[i]))
                        {
                            piece.buffList.Add(buffList[i]);
                            switch (animalType)
                            {
                                case PieceData.Animal.Hamster:
                                    AddBattleStartEffect(buffManager.animalBuff[0].hamsterBuff[i].BattleStartEffect);
                                    AddCoroutine(buffManager.animalBuff[0].hamsterBuff[i].CoroutineEffect);
                                    break;
                                case PieceData.Animal.Cat:
                                    buffManager.animalBuff[0].catBuff[i].DirectEffect(piece, true);
                                    break;
                                case PieceData.Animal.Dog:
                                    buffManager.animalBuff[0].dogBuff[i].DirectEffect(piece, true);
                                    AddBattleStartEffect(buffManager.animalBuff[0].dogBuff[i].BattleStartEffect);
                                    break;
                                case PieceData.Animal.Frog:
                                    buffManager.animalBuff[0].frogBuff[i].DirectEffect(piece, true);
                                    AddBattleStartEffect(buffManager.animalBuff[0].frogBuff[i].BattleStartEffect);
                                    break;
                                case PieceData.Animal.Rabbit:
                                    AddCoroutine(buffManager.animalBuff[0].rabbitBuff[i].CoroutineEffect);
                                    break;
                            }
                        }
                        if (!DualPlayers[0].buffDatas.Contains(buffList[i])) DualPlayers[0].buffDatas.Add(buffList[i]);
                    }
                }
            }
            else if (count < thresholds[i])
            {
                foreach (var piece in myFilePieceList)
                {
                    if (piece.buffList.Contains(buffList[i]))
                    {
                        piece.buffList.Remove(buffList[i]);
                        switch (animalType)
                        {
                            case PieceData.Animal.Hamster:
                                RemoveBattleStartEffect(buffManager.animalBuff[0].hamsterBuff[i].BattleStartEffect);
                                RemoveCoroutine(buffManager.animalBuff[0].hamsterBuff[i].CoroutineEffect);
                                break;
                            case PieceData.Animal.Cat:
                                buffManager.animalBuff[0].catBuff[i].DirectEffect(piece, false);
                                break;
                            case PieceData.Animal.Dog:
                                buffManager.animalBuff[0].dogBuff[i].DirectEffect(piece, false);
                                RemoveBattleStartEffect(buffManager.animalBuff[0].dogBuff[i].BattleStartEffect);
                                break;
                            case PieceData.Animal.Frog:
                                buffManager.animalBuff[0].frogBuff[i].DirectEffect(piece, false);
                                RemoveBattleStartEffect(buffManager.animalBuff[0].frogBuff[i].BattleStartEffect);
                                break;
                            case PieceData.Animal.Rabbit:
                                RemoveCoroutine(buffManager.animalBuff[0].rabbitBuff[i].CoroutineEffect);
                                break;
                        }
                    }
                    if (DualPlayers[0].buffDatas.Contains(buffList[i])) DualPlayers[0].buffDatas.Remove(buffList[i]);
                }
            }
        }
    }
    /*void ApplyUnitedBuff(List<BuffData> buffList, int count, int[] thresholds, PieceData.United unitedType)
    {
        for (int i = 0; i < thresholds.Length; i++)
        {
            if (count >= thresholds[i])
            {
                foreach (var piece in myFilePieceList)
                {
                    if (piece.pieceData.united == unitedType)
                    {
                        //Remove
                        if (i > 0 && piece.buffList.Contains(buffList[i - 1]))
                        {
                            piece.buffList.Remove(buffList[i - 1]);
                            switch (unitedType)
                            {
                                case PieceData.United.UnderWorld://3
                                    RemoveCoroutine(buffManager.unitedBuff[0].underWorldBuff[i - 1].CoroutineEffect);
                                    break;
                                case PieceData.United.Faddist://2
                                    RemoveBattleStartEffect(buffManager.unitedBuff[0].faddistBuff[i - 1].BattleStartEffect);
                                    break;
                                case PieceData.United.WarMachine://1
                                    buffManager.unitedBuff[0].warMachineBuff[i - 1].DirectEffect(piece, false);
                                    break;
                                case PieceData.United.Creature://4
                                    buffManager.unitedBuff[0].creatureBuff[i - 1].DirectEffect(piece, false);
                                    break;
                            }
                        }
                        if (i > 0 && DualPlayers[0].buffDatas.Contains(buffList[i - 1])) DualPlayers[0].buffDatas.Remove(buffList[i - 1]);
                        //Add
                        if (!piece.buffList.Contains(buffList[i]))
                        {
                            piece.buffList.Add(buffList[i]);
                            switch (unitedType)
                            {
                                case PieceData.United.UnderWorld://3
                                    AddCoroutine(buffManager.unitedBuff[0].underWorldBuff[i].CoroutineEffect);
                                    break;
                                case PieceData.United.Faddist://2
                                    AddBattleStartEffect(buffManager.unitedBuff[0].faddistBuff[i].BattleStartEffect);
                                    break;
                                case PieceData.United.WarMachine://1
                                    buffManager.unitedBuff[0].warMachineBuff[i].DirectEffect(piece, true);
                                    break;
                                case PieceData.United.Creature://4
                                    buffManager.unitedBuff[0].creatureBuff[i].DirectEffect(piece, true);
                                    break;
                            }
                        }
                        if (!DualPlayers[0].buffDatas.Contains(buffList[i])) DualPlayers[0].buffDatas.Add(buffList[i]);
                    }
                }
            }
            else if (count < thresholds[i])
            {
                foreach (var piece in myFilePieceList)
                {
                    if (piece.buffList.Contains(buffList[i]))
                    {
                        piece.buffList.Remove(buffList[i]);
                        switch (unitedType)
                        {
                            case PieceData.United.UnderWorld://3
                                RemoveCoroutine(buffManager.unitedBuff[0].underWorldBuff[i - 1].CoroutineEffect);
                                break;
                            case PieceData.United.Faddist://2
                                RemoveBattleStartEffect(buffManager.unitedBuff[0].faddistBuff[i - 1].BattleStartEffect);
                                break;
                            case PieceData.United.WarMachine://1
                                buffManager.unitedBuff[0].warMachineBuff[i - 1].DirectEffect(piece, false);
                                break;
                            case PieceData.United.Creature://4
                                buffManager.unitedBuff[0].creatureBuff[i - 1].DirectEffect(piece, false);
                                break;
                        }
                    }
                    if (DualPlayers[0].buffDatas.Contains(buffList[i])) DualPlayers[0].buffDatas.Remove(buffList[i]);
                }
            }
        }
    }*/

    public void ActiveSynerge() // 임시 변경 요망
    {
        foreach (var effect in sBattleStartEffect) effect(true);
        foreach (var effect in sCoroutineEffect) effect();

        Debug.Log("즉시 발동 시너지 효과 : " + sBattleStartEffect.Count);
        Debug.Log("지연 발동 시너지 효과 : " + sCoroutineEffect.Count);
        InitializingRound();
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

    public Piece SpawnPiece(PieceData pieceData, int grade, Tile targetTile)
    {
        Vector3 targetPosition = new Vector3(targetTile.transform.position.x, groundHeight, targetTile.transform.position.z);
        GameObject pieceObject = Instantiate(pieceData.piecePrefab, targetPosition, Quaternion.Euler(0, 0, 0));
        pieceObject.transform.parent = pieceParent;
        Piece piece = pieceObject.GetComponent<Piece>();
        piece.currentTile = targetTile;
        piece.grade = grade;

        targetTile.IsFull = true;

        //나중에 지울 거
        piece.pieceData = pieceData;
        //

        PieceListCountUp(piece);

        return piece;
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

            if (parentTile.isReadyTile)
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

            if (!parentTile.isReadyTile && !compareTile.isReadyTile)
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