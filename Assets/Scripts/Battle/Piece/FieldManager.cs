//using System;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldManager : MonoBehaviour
{
    private static FieldManager instance;
    public static FieldManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<FieldManager>();
                if (instance == null)
                {
                    GameObject _arena = new GameObject();
                    _arena.name = "FieldManager";
                    instance = _arena.AddComponent<FieldManager>();
                    //DontDestroyOnLoad(_arena);
                }
            }
            return instance;
        }
    }

    public Messenger owerPlayer;

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

    #region 기물 능력치
    public class PieceStatusList
    {
        public List<float> pieceHealth = new List<float>();
        public List<float> pieceMana = new List<float>();
        public List<float> pieceManaRecovery = new List<float>();
        public List<float> pieceAttackDamage = new List<float>();
        public List<float> pieceAbilityPower = new List<float>();
        public List<float> pieceAbilityPowerCoefficient = new List<float>();
        public List<float> pieceArmor = new List<float>();
        public List<float> pieceMagicResist = new List<float>();
        public List<float> pieceAttackSpeed = new List<float>();
        public List<float> pieceCriticalChance = new List<float>();
        public List<float> pieceCriticalDamage = new List<float>();
        public List<float> pieceAttackRange = new List<float>();
        public List<float> pieceBloodBrain = new List<float>();
        public List<float> pieceMoveSpeed = new List<float>();

        public void AddPieceStatus(Piece piece)
        {
            pieceHealth.Add(piece.health);
            pieceMana.Add(piece.mana);
            pieceManaRecovery.Add(piece.manaRecovery);
            pieceAttackDamage.Add(piece.attackDamage);
            pieceAbilityPower.Add(piece.abilityPower);
            pieceAbilityPowerCoefficient.Add(piece.abilityPowerCoefficient);
            pieceArmor.Add(piece.armor);
            pieceMagicResist.Add(piece.magicResist);
            pieceAttackSpeed.Add(piece.attackSpeed);
            pieceCriticalChance.Add(piece.criticalChance);
            pieceCriticalDamage.Add(piece.criticalDamage);
            pieceAttackRange.Add(piece.attackRange);
            pieceBloodBrain.Add(piece.bloodBrain);
            pieceMoveSpeed.Add(piece.moveSpeed);
        }
        public void ClearPieceStatusList()
        {
            pieceHealth.Clear();
            pieceMana.Clear();
            pieceManaRecovery.Clear();
            pieceAttackDamage.Clear();
            pieceAbilityPower.Clear();
            pieceAbilityPowerCoefficient.Clear();
            pieceArmor.Clear();
            pieceMagicResist.Clear();
            pieceAttackSpeed.Clear();
            pieceCriticalChance.Clear();
            pieceCriticalDamage.Clear();
            pieceAttackRange.Clear();
            pieceBloodBrain.Clear();
            pieceMoveSpeed.Clear();
        }
        public void SetStatus(Piece piece, int index)
        {
            piece.health = this.pieceHealth[index];
            piece.mana = this.pieceMana[index];
            piece.manaRecovery = this.pieceManaRecovery[index];
            piece.attackDamage = this.pieceAttackDamage[index];
            piece.abilityPower = this.pieceAbilityPower[index];
            piece.abilityPowerCoefficient = this.pieceAbilityPowerCoefficient[index];
            piece.armor = this.pieceArmor[index];
            piece.magicResist = this.pieceMagicResist[index];
            piece.attackSpeed = this.pieceAttackSpeed[index];
            piece.criticalChance = this.pieceCriticalChance[index];
            piece.criticalDamage = this.pieceCriticalDamage[index];
            piece.attackRange = this.pieceAttackRange[index];
            piece.bloodBrain = this.pieceBloodBrain[index];
            piece.moveSpeed = this.pieceMoveSpeed[index];
        }
    }
    public PieceStatusList pieceStatus = new PieceStatusList();
    #endregion

    [Header("아군 전투 유닛")] public List<Piece> myFilePieceList;
    [Header("상대 전투 유닛")] public List<Piece> enemyFilePieceList;
    [Header("아이템 소지 목록")] public List<Equipment> myEquipmentList;
    [Header("아군 기물")] public Transform pieceParent;
    [Header("상대 기물")] public Transform enemyParent;

    public PathFinding pathFinding;
    public PlayerState playerState;
    public FieldPieceStatus fieldPieceStatus;
    public MapChanger mapChanger;

    public List<Tile> readyTileList;
    public List<Tile> battleTileList;

    public GameObject[] readyZoneHexaIndicators;
    public GameObject[] battleFieldHexaIndicators;
    [SerializeField] public List<CatCoin> catcoin;

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

    public enum RoundType
    {
        None = -1,
        Deployment,     //배치
        Ready,          //대기
        Battle,         //전투
        Dead,
        Max
    };
    public RoundType roundType = RoundType.None;

    public RoundState roundState;
    public int currentRound = 0;

    public ResultPopup resultPopup;
    public enum Result { NONE, VICTORY, DEFEAT }
    public Result BattleResult
    {
        get { return battleResult; }
        set
        {
            if (BattleResult != value)
            {
                battleResult = value;

                if (BattleResult == Result.VICTORY)
                {
                    roundState.UpdateStageIcon(currentRound, 1, stageInformation.enemy[currentRound].roundType);

                    if (currentRound != stageInformation.enemy.Count - 1)
                    {
                        if (stageInformation.enemy[currentRound].mapType != stageInformation.enemy[currentRound + 1].mapType)
                            Invoke("Fade", 3f);
                        else
                            Invoke("NextRound", 3f);
                    }
                    else
                    {
                        resultPopup.ActiveResultPopup(true);
                        SoundManager.instance.Play("UI/Eff_Round_Win", SoundManager.Sound.Effect);
                    }

                    foreach (var piece in myFilePieceList)
                        piece.VictoryDacnce();
                }
                else if (BattleResult == Result.DEFEAT)
                {
                    ChargeHP(stageInformation.enemy[currentRound].defeatDamage);

                    if (currentRound != stageInformation.enemy.Count - 1)
                    {
                        if (stageInformation.enemy[currentRound].mapType != stageInformation.enemy[currentRound + 1].mapType)
                            Invoke("Fade", 3f);
                        else
                            Invoke("NextRound", 3f);
                    }
                    else
                    {
                        resultPopup.ActiveResultPopup(false);
                        SoundManager.instance.Play("UI/Eff_Round_Lose", SoundManager.Sound.Effect);
                    }

                    roundState.UpdateStageIcon(currentRound, 2, stageInformation.enemy[currentRound].roundType);
                }
            }
        }
    }
    public Result battleResult;

    public EnemyInformationData stageInformation;
    public int currentStage;

    public Camera uiCamera;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }

        StartCoroutine(StartGame());
        //StartCoroutine(CalRoundTime(3));
    }

    [Header("상점")] public PieceShop pieceShop;
    public void AddDPList(Piece target)
    {
        PieceDPList pieceDP = new PieceDPList(target, target.targetTile);
        pieceDpList.Add(pieceDP);

        fieldPieceStatus.UpdateFieldStatus(myFilePieceList.Count, owerPlayer.maxPieceCount[owerPlayer.level]);
    }

    public void RemoveDPList(Piece target)
    {
        for (int i = 0; i < pieceDpList.Count; i++)
        {
            if (pieceDpList[i].piece == target)
            {
                pieceDpList.RemoveAt(i);
                break;
            }
        }

        fieldPieceStatus.UpdateFieldStatus(myFilePieceList.Count, owerPlayer.maxPieceCount[owerPlayer.level]);
    }

    public void FieldInit()
    {
        foreach (Tile tile in battleTileList)
        {
            tile.piece = null;
            tile.IsFull = false;
            tile.walkable = true;
            if (tile.piece != null)
                tile.piece.gameObject.SetActive(false);
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

            dp.piece.transform.position = new Vector3(dp.dpTile.transform.position.x, groundHeight, dp.dpTile.transform.position.z);

            dp.piece.gameObject.SetActive(true);
            dp.piece.pieceData.InitialzePiece(dp.piece);
            dp.piece.mana = dp.piece.mana = dp.piece.pieceData.currentMana;
            dp.piece.PieceState = Piece.State.IDLE;
        }

        foreach (Piece piece in enemyFilePieceList)
            Destroy(piece.gameObject);
        enemyFilePieceList = new List<Piece>();

        bool fusion = false;
        for (int i = 0; i < myFilePieceList.Count; i++)
        {
            if (fusion)
                i = 0;

            fusion = FusionCheck(myFilePieceList[i]);
        }
    }

    public void SpawnEnemy(int stage)
    {
        for (int i = 0; i < stageInformation.enemy[stage].enemyInformation.Count; i++)
        {
            int tileX = ((int)stageInformation.enemy[stage].enemyInformation[i].spawnTile.x);
            int tileY = ((int)stageInformation.enemy[stage].enemyInformation[i].spawnTile.y);

            GameObject enemyGameObject = Instantiate(stageInformation.enemy[stage].enemyInformation[i].piece, Vector3.zero, Quaternion.identity);
            enemyGameObject.transform.parent = enemyParent;

            Piece enemyPiece = enemyGameObject.GetComponent<Piece>();
            Tile targetTile = pathFinding.grid[tileX].tile[tileY];

            targetTile.piece = enemyPiece;
            targetTile.IsFull = true;
            targetTile.walkable = false;

            enemyPiece.currentTile = targetTile;
            enemyPiece.targetTile = targetTile;
            enemyPiece.pieceData.InitialzePiece(enemyPiece); enemyPiece.mana = enemyPiece.pieceData.currentMana;

            enemyGameObject.transform.position = new Vector3(targetTile.transform.position.x, -0.5f, targetTile.transform.position.z);

            enemyFilePieceList.Add(enemyPiece);
        }
    }

    public void NextStage()
    {
        FieldInit();

        for (int i = 0; i < pieceDpList.Count; i++)
            pieceStatus.SetStatus(pieceDpList[i].piece, i);
        pieceStatus.ClearPieceStatusList();

        foreach (var coins in catcoin) coins.MoveCoin();

        roundType = RoundType.Deployment;
        foreach (var effect in sBattleStartEffect) effect(false);
        StopAllCoroutines();

        currentStage++;
        AugmentManager.Instance.CheckAugmentRound(currentStage);
        SpawnEnemy(currentStage);
        ChangeMap(currentStage);

        playerState.UpdateLevel(owerPlayer.level);
        playerState.UpdateMoney(owerPlayer.gold);
        pieceShop.InitSlot();
    }

    #region 라운드(버튼식)
    public void BattleEndCheck(List<Piece> pieceList)
    {
        for (int i = 0; i < pieceList.Count; i++)
        {
            if (!pieceList[i].dead)
                return;

            if (i == pieceList.Count - 1 && pieceList[i].dead)
            {
                if (pieceList[i].isOwned)
                {
                    SoundManager.instance.Play("UI/Eff_Round_Lose", SoundManager.Sound.Effect);
                    if (BattleResult == Result.NONE) BattleResult = Result.DEFEAT;
                }
                else
                {
                    SoundManager.instance.Play("UI/Eff_Round_Win", SoundManager.Sound.Effect);
                    if (BattleResult == Result.NONE) BattleResult = Result.VICTORY;
                }
            }
        }
    }

    public void Fade()
    {
        mapChanger.animator.SetTrigger("MapChange");
    }

    public void NextRound()
    {
        if (currentRound == 5)
        {
            SoundManager.instance.Play("BGM/Bgm_Battle_Boss", SoundManager.Sound.Effect);
        }
        roundType = RoundType.Ready;

        Reward(currentRound, BattleResult);

        BattleResult = Result.NONE;
        NextStage();
        currentRound++;
        ChangeStage(currentRound);
        roundState.UpdateStageIcon(currentRound, 3, stageInformation.enemy[currentRound].roundType);

    }

    public void StartBattle()
    {
        if (roundType == RoundType.Battle)
            return;

        roundType = RoundType.Battle;
        SoundManager.instance.Play("UI/Eff_Button_Positive", SoundManager.Sound.Effect);

        foreach (var list in pieceDpList)
            pieceStatus.AddPieceStatus(list.piece);

        ActiveSynerge();

        if (myFilePieceList.Count == 0)
        {
            if (enemyFilePieceList.Count == 0)
            {
                SoundManager.instance.Play("UI/Eff_Round_Win", SoundManager.Sound.Effect);
                BattleResult = Result.VICTORY;
            }
            else
            {
                SoundManager.instance.Play("UI/Eff_Round_Lose", SoundManager.Sound.Effect);
                BattleResult = Result.DEFEAT;
            }
        }

        foreach (var piece in myFilePieceList)
            piece.StartNextBehavior();
        foreach (var piece in enemyFilePieceList)
            piece.StartNextBehavior();
    }

    private void ChangeStage(int round)
    {
        roundState.NextRound(round);
        roundState.OnRoundPopup(1, round);
    }

    private IEnumerator StartGame()
    {
        if (stageInformation == null)
            stageInformation = GameManager.Instance.selectedStage;
        ChangeMap(currentRound);
        ChangeGold(owerPlayer.gold);
        ChangeHP(owerPlayer.lifePoint);
        ChangeLevel(owerPlayer.level);

        roundState.SetStage(currentRound);
        yield return new WaitForSeconds(1f);
        ChangeStage(currentRound);
        SpawnEnemy(currentRound);
        roundState.InitRoundIcon();
        roundState.UpdateStageIcon(currentRound, 3, stageInformation.enemy[currentRound].roundType);

        fieldPieceStatus.UpdateFieldStatus(myFilePieceList.Count, owerPlayer.maxPieceCount[owerPlayer.level]);
    }
    #endregion

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
        if (roundType == RoundType.Deployment)
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
        if (roundType == RoundType.Battle)
        {
            for (int i = 0; i < readyZoneHexaIndicators.Length; i++)
            {
                readyZoneHexaIndicators[i].SetActive(isactive);
            }
        }
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

            SynergeBoard.instance.SynergeCountUpdate(_plusMyth, _plusAnimal, _plusUnited);
        }
        else if (minus != null) //Change Piece
        {
            PieceData.Myth _minusMyth = minus.pieceData.myth;
            PieceData.Animal _minusAnimal = minus.pieceData.animal;
            PieceData.United _minusUnited = minus.pieceData.united;
            ApplySynerge(_plusMyth, _plusAnimal, _plusUnited);
            ApplySynerge(_minusMyth, _minusAnimal, _minusUnited);

            SynergeBoard.instance.SynergeCountUpdate(_plusMyth, _plusAnimal, _plusUnited);
            SynergeBoard.instance.SynergeCountUpdate(_minusMyth, _minusAnimal, _minusUnited);
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
    public void AddCoroutine(CoroutineEffect handler) { sCoroutineEffect.Add(handler); }
    public void RemoveCoroutine(CoroutineEffect handler) { sCoroutineEffect.Remove(handler); }

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
                                    RemoveBattleStartEffect(buffManager.mythBuff[0].frostyWindBuff[i - 1].BattleStartEffect);
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
                        if (i > 0 && owerPlayer.buffDatas.Contains(buffList[i - 1])) owerPlayer.buffDatas.Remove(buffList[i - 1]);

                        if (!piece.buffList.Contains(buffList[i]))
                        {
                            piece.buffList.Add(buffList[i]);
                            switch (mythType)
                            {
                                case PieceData.Myth.GreatMountain:
                                    AddBattleStartEffect(buffManager.mythBuff[0].greatMoutainBuff[i].BattleStartEffect);
                                    break;
                                case PieceData.Myth.FrostyWind:
                                    AddBattleStartEffect(buffManager.mythBuff[0].frostyWindBuff[i].BattleStartEffect);
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
                        if (!owerPlayer.buffDatas.Contains(buffList[i])) owerPlayer.buffDatas.Add(buffList[i]);
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
                                RemoveBattleStartEffect(buffManager.mythBuff[0].frostyWindBuff[i].BattleStartEffect);
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
                    if (owerPlayer.buffDatas.Contains(buffList[i])) owerPlayer.buffDatas.Remove(buffList[i]);
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
                        if (i > 0 && owerPlayer.buffDatas.Contains(buffList[i - 1])) owerPlayer.buffDatas.Remove(buffList[i - 1]);
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
                        if (!owerPlayer.buffDatas.Contains(buffList[i])) owerPlayer.buffDatas.Add(buffList[i]);
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
                    if (owerPlayer.buffDatas.Contains(buffList[i])) owerPlayer.buffDatas.Remove(buffList[i]);
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

        //Debug.Log("즉시 발동 시너지 효과 : " + sBattleStartEffect.Count);
        //Debug.Log("지연 발동 시너지 효과 : " + sCoroutineEffect.Count);
        InitializingRound();
    }
    #endregion

    public void InitializingRound()
    {
        if (owerPlayer.isGrab)
        {
            owerPlayer.isDrag = true;
            object _controlObject = (owerPlayer.ControlPiece != null) ? owerPlayer.ControlPiece : owerPlayer.ControlEquipment;

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
        int star = piece.star;

        currentPieceList[kind].count[star].count.Add(piece);

        FusionCheck(piece, kind, star);
    }

    public void PieceListCountDown(Piece piece)
    {
        int kind = CheckPieceKind(piece.pieceData);
        int star = piece.star;

        currentPieceList[kind].count[star].count.Remove(piece);
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

    public Piece SpawnPiece(PieceData pieceData, int star, Tile targetTile)
    {
        Vector3 targetPosition = new Vector3(targetTile.transform.position.x, groundHeight, targetTile.transform.position.z);
        GameObject pieceObject = Instantiate(pieceData.piecePrefab, targetPosition, Quaternion.Euler(0, 0, 0));
        pieceObject.transform.parent = pieceParent;
        Piece piece = pieceObject.GetComponent<Piece>();
        piece.currentTile = targetTile;
        piece.targetTile = targetTile;
        piece.star = star;
        piece.isOwned = true;
        targetTile.IsFull = true;
        targetTile.walkable = false;
        targetTile.piece = piece;

        //나중에 지울 거
        piece.pieceData = pieceData;
        //

        PieceListCountUp(piece);

        return piece;
    }

    void FusionCheck(Piece target, int kind, int star)
    {
        if (currentPieceList[kind].count[star].count.Count >= 3)
        {
            FusionPiece(target);
        }
    }

    bool FusionCheck(Piece target)
    {
        int kind = CheckPieceKind(target.pieceData);
        int star = target.star;

        if (currentPieceList[kind].count[star].count.Count >= 3)
        {
            FusionPiece(target);
            return true;
        }
        else
            return false;
    }

    void FusionPiece(Piece piece)
    {
        if (roundType == RoundType.Battle)
            return;

        int kind = CheckPieceKind(piece.pieceData);
        int star = piece.star;

        Piece parentPiece = piece;
        Piece firstChild = null;
        Piece secondChild = null;

        Tile parentTile = parentPiece.currentTile;

        //set parentPiece
        for (int i = 0; i < currentPieceList[kind].count[star].count.Count; i++)
        {
            Tile compareTile = currentPieceList[kind].count[star].count[i].currentTile;

            if (parentTile.isReadyTile)
            {
                if (!compareTile.isReadyTile)
                {
                    parentPiece = currentPieceList[kind].count[star].count[i];
                    parentTile = parentPiece.currentTile;
                }
                else if (compareTile.isReadyTile)
                {
                    if (readyTileList.IndexOf(parentTile) > readyTileList.IndexOf(compareTile))
                    {
                        parentPiece = currentPieceList[kind].count[star].count[i];
                        parentTile = parentPiece.currentTile;
                    }
                }
            }

            if (parentPiece == currentPieceList[kind].count[star].count[i]) continue;

            if (!parentTile.isReadyTile && !compareTile.isReadyTile)
            {
                if (parentTile.gridX > compareTile.gridX)
                {
                    parentPiece = currentPieceList[kind].count[star].count[i];
                    parentTile = parentPiece.currentTile;
                }
                else if (parentTile.gridX == compareTile.gridX)
                {
                    if (parentTile.gridY > compareTile.gridY)
                    {
                        parentPiece = currentPieceList[kind].count[star].count[i];
                        parentTile = parentPiece.currentTile;
                    }
                }
            }
        }
        currentPieceList[kind].count[star].count.Remove(parentPiece);


        //set firstChild
        firstChild = GetChildPiece(kind, star);

        //set secondChild
        secondChild = GetChildPiece(kind, star);

        Piece originPiece = OriginPiece(firstChild, secondChild, parentPiece);

        Tile targetTile = parentPiece.currentTile;

        RemoveDPList(firstChild.currentTile.piece);
        RemoveDPList(secondChild.currentTile.piece);
        RemoveDPList(parentPiece.currentTile.piece);

        DestroyPiece(parentPiece, targetTile);
        DestroyPiece(firstChild, firstChild.currentTile);
        DestroyPiece(secondChild, secondChild.currentTile);

        if (!targetTile.isReadyTile)
        {
            Piece resultPiece = SpawnPiece(piece.pieceData, star + 1, targetTile);
            #region 나중에 문제 없으면 지울 예정입니다!
            //resultPiece.maxHealth = resultPiece.pieceData.health[resultPiece.star];
            ////아이템으로 인한 MAX_HP의 상승분을 여기에 구현
            //resultPiece.name += " " + star + 1 + "Star";
            //resultPiece.healthbar.FusionStarAnim(star);
            //SoundManager.instance.Play("UI/Eff_Upgrade", SoundManager.Sound.Effect);
            //resultPiece.buffList = originPiece.buffList;
            //resultPiece.pieceData.InitialzePiece(resultPiece); resultPiece.mana = resultPiece.pieceData.currentMana;
            //string framePath = string.Format("Sprites/Unit HpBar_UI/{0}Star Frame", resultPiece.star);
            //resultPiece.healthbar.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(framePath);
            //for (int i = 0; i < resultPiece.buffList.Count; i++)
            //{
            //    if (resultPiece.buffList[i].haveDirectEffect == true)
            //        resultPiece.pieceData.CalculateBuff(resultPiece, resultPiece.buffList[i]);
            //}
            //resultPiece.currentTile.gameObject.transform.GetChild(3).gameObject.SetActive(true);
            //myFilePieceList.Add(resultPiece);
            //AddDPList(resultPiece);
            #endregion
            FusionPieceInit(resultPiece, originPiece);
        }
        else
        {
            Piece resultPiece = SpawnPiece(piece.pieceData, star + 1, targetTile);
            #region 나중에 문제 없으면 지울 예정입니다!
            //resultPiece.maxHealth = resultPiece.pieceData.health[resultPiece.star];
            //resultPiece.name += " " + star + 1 + "Star";
            //resultPiece.healthbar.FusionStarAnim(star);
            //SoundManager.instance.Play("UI/Eff_Upgrade", SoundManager.Sound.Effect);
            //resultPiece.pieceData.InitialzePiece(resultPiece); resultPiece.mana = resultPiece.pieceData.currentMana;
            //string framePath = string.Format("Sprites/Unit HpBar_UI/{0}Star Frame", resultPiece.star);
            //resultPiece.healthbar.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(framePath);
            //resultPiece.currentTile.gameObject.transform.GetChild(1).gameObject.SetActive(true);
            #endregion
            FusionPieceInit(resultPiece);
        }

        fieldPieceStatus.UpdateFieldStatus(myFilePieceList.Count, owerPlayer.maxPieceCount[owerPlayer.level]);
    }

    Piece GetChildPiece(int kind, int star)
    {
        Piece childPiece = currentPieceList[kind].count[star].count[0];

        for (int i = 0; i < currentPieceList[kind].count[star].count.Count; i++)
        {
            Tile parentTile = childPiece.currentTile;
            Tile compareTile = currentPieceList[kind].count[star].count[i].currentTile;

            if (!parentTile.isReadyTile && compareTile.isReadyTile)
                childPiece = currentPieceList[kind].count[star].count[i];
            else if (parentTile.isReadyTile && compareTile.isReadyTile)
            {
                if (readyTileList.IndexOf(parentTile) < readyTileList.IndexOf(compareTile))
                    childPiece = currentPieceList[kind].count[star].count[i];
            }

            if (!parentTile.isReadyTile && !compareTile.isReadyTile)
            {
                if (parentTile.gridX < compareTile.gridX)
                    childPiece = currentPieceList[kind].count[star].count[i];
                else if (parentTile.gridX == compareTile.gridX)
                {
                    if (parentTile.gridY > compareTile.gridY)
                        childPiece = currentPieceList[kind].count[star].count[i];
                }
            }
        }
        currentPieceList[kind].count[star].count.Remove(childPiece);
        return childPiece;
    }

    void FusionPieceInit(Piece resultPiece, Piece originPiece = null)
    {
        resultPiece.pieceData.InitialzePiece(resultPiece);
        AugmentManager.Instance.AugmentCheck(resultPiece);
        resultPiece.name += " " + resultPiece.star + "Star";
        resultPiece.maxHealth = resultPiece.pieceData.health[resultPiece.star];
        resultPiece.mana = resultPiece.pieceData.currentMana;
        resultPiece.healthbar.FusionStarAnim(resultPiece.star - 1);
        string framePath = string.Format("Sprites/Unit HpBar_UI/{0}Star Frame", resultPiece.star);
        resultPiece.healthbar.gameObject.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(framePath);
        SoundManager.instance.Play("UI/Eff_Upgrade", SoundManager.Sound.Effect);
        if (originPiece != null)
        {
            resultPiece.buffList = originPiece.buffList;
            for (int i = 0; i < resultPiece.buffList.Count; i++)
            {
                if (resultPiece.buffList[i].haveDirectEffect == true)
                    resultPiece.pieceData.CalculateBuff(resultPiece, resultPiece.buffList[i]);
            }
            myFilePieceList.Add(resultPiece);
            AddDPList(resultPiece);
        }
        TileManager.Instance.ActiveFusionEffect(resultPiece.currentTile.gameObject);
    }

    public void DestroyPiece(Piece piece, Tile targetTile)
    {
        if (!piece.currentTile.isReadyTile)
            myFilePieceList.Remove(piece);
        targetTile.IsFull = false;
        targetTile.walkable = true;
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

    public Tile GetReadyTile()
    {
        Tile targetTile = null;

        targetTile = TileCheck(targetTile, readyTileList);
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

    Piece OriginPiece(Piece first, Piece second, Piece parentPiece)
    {
        Piece piece = null;
        if (first.currentTile.isReadyTile == true && first.currentTile.isReadyTile == true) piece = parentPiece;
        else piece = (first.currentTile.isReadyTile == false) ? first : second;

        return piece;
    }

    public Chest chest;

    public void Reward(int currentRound, Result result)
    {
        ChargeGold(RewardGold(currentRound, result));

        //레벨 업
    }

    public void ChargeGold(int gold)
    {
        pieceShop.DeactiveSlots();
        owerPlayer.gold += gold;
        playerState.UpdateMoney(owerPlayer.gold);
    }

    public void ChangeGold(int gold)
    {
        playerState.UpdateMoney(owerPlayer.gold);
    }

    public int RewardGold(int currentRound, Result result)
    {
        if (result == Result.VICTORY)
            return stageInformation.enemy[currentRound].gold;
        else if (result == Result.DEFEAT)
        {
            return stageInformation.enemy[currentRound].defeatGold;
        }

        return 0;
    }

    public void ChargeHP(int hp)
    {
        owerPlayer.lifePoint += hp;
        playerState.UpdateCurrentHP(owerPlayer.lifePoint);

        if (owerPlayer.lifePoint <= 0)
        {
            resultPopup.ActiveResultPopup(false);
        }
    }

    public void ChangeHP(int hp)
    {
        playerState.UpdateCurrentHP(owerPlayer.lifePoint);

        if (owerPlayer.lifePoint <= 0)
        {
            resultPopup.ActiveResultPopup(false);
        }
    }

    public void ChargeLevel(int level)
    {
        SoundManager.instance.Play("UI/Eff_LevelUp", SoundManager.Sound.Effect);
        owerPlayer.level += level;
        playerState.UpdateLevel(owerPlayer.level);
    }

    public void ChangeLevel(int level)
    {
        playerState.UpdateLevel(owerPlayer.level);
    }

    [Header("로키용 타일 위치")] public Tile lokiPieceSkillPosition;

    public void ChangeMap(int round)
    {
        mapChanger.ChangeMap(stageInformation.enemy[round].mapType);
    }


}