using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ArenaManager : MonoBehaviour
{
    private static ArenaManager instance;
    public static ArenaManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ArenaManager>();
                if (instance == null)
                {
                    GameObject _arena = new GameObject();
                    _arena.name = "ArenaManager";
                    instance = _arena.AddComponent<ArenaManager>();
                    //DontDestroyOnLoad(_arena);
                }
            }
            return instance;
        }
    }

    public List<FieldManager> fieldManagers;

    public List<PlayerPair> playerMatchingPair;

    public enum RoundType
    {
        None = -1,
        Deployment,     //배치
        Ready,          //대기
        Battle,         //전투
        Event,
        Overtime,
        Duel,           //None
        Dead,
        Max
    };
    public RoundType roundType = RoundType.None;

    public float deploymentTime = 30;
    public float readyTime = 6f;
    public float battleTime = 60f;
    public float battleOverTime = 60f;
    public float groundEventTime = 10000f;
    public float duelTime = 60f;

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
                    roundState.UpdateStageIcon(currentRound, 1, fieldManagers[0].stageInformation.enemy[currentRound].roundType);

                    if (currentRound != fieldManagers[0].stageInformation.enemy.Count - 1)
                    {
                        if (fieldManagers[0].stageInformation.enemy[currentRound].mapType != fieldManagers[0].stageInformation.enemy[currentRound + 1].mapType)
                            Invoke("Fade", 3f);
                        else
                            Invoke("NextRound", 3f);
                    }
                    else
                    {
                        resultPopup.ActiveResultPopup(true);
                        SoundManager.instance.Play("UI/Eff_Win", SoundManager.Sound.Effect);
                    }

                    foreach (var piece in fieldManagers[0].myFilePieceList)
                        piece.VictoryDacnce();
                }
                else if (BattleResult == Result.DEFEAT)
                {
                    fieldManagers[0].ChargeHP(fieldManagers[0].stageInformation.enemy[currentRound].defeatDamage);

                    if (currentRound != fieldManagers[0].stageInformation.enemy.Count - 1)
                    {
                        if (fieldManagers[0].stageInformation.enemy[currentRound].mapType != fieldManagers[0].stageInformation.enemy[currentRound + 1].mapType)
                            Invoke("Fade", 3f);
                        else
                            Invoke("NextRound", 3f);
                    }
                    else
                    {
                        resultPopup.ActiveResultPopup(false);
                        SoundManager.instance.Play("UI/Eff_Lose", SoundManager.Sound.Effect);
                    }

                    roundState.UpdateStageIcon(currentRound, 2, fieldManagers[0].stageInformation.enemy[currentRound].roundType);
                }
            }
        }
    }
    public Result battleResult;

    public MapChanger mapChanger;

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
    #region 라운드(타이머식)
    IEnumerator CalRoundTime(float time, string nextRound = "Deployment")
    {
        yield return new WaitForSeconds(1f);
        var _restTime = time - 1f;
        if (_restTime == 0)
        {
            fieldManagers[0].InitializingRound();
            switch (roundType)
            {
                case RoundType.Deployment:
                    roundType = RoundType.Ready;
                    StartCoroutine(CalRoundTime(readyTime, "Battle"));
                    Debug.Log("현재 배치 라운드 입니다~!");
                    yield break;
                case RoundType.Ready:
                    if (nextRound == "Deployment") //다음 라운드가 배치 라운드인 경우
                    {
                        roundType = RoundType.Deployment;
                        StartCoroutine(CalRoundTime(deploymentTime));
                        Debug.Log("현재 배치 라운드 입니다~!");
                    }
                    else if (nextRound == "Battle") //다음 라운드가 전투 라운드인 경우
                    {
                        roundType = RoundType.Battle;
                        StartCoroutine(CalRoundTime(battleTime));
                        Debug.Log("현재 전투 라운드 입니다~!");
                    }
                    yield break;
                case RoundType.Battle:
                    if (fieldManagers[0].myFilePieceList.Count > 0 && fieldManagers[0].enemyFilePieceList.Count > 0)
                        roundType = RoundType.Overtime;
                    yield break;
                case RoundType.Overtime:
                    roundType = RoundType.Ready;
                    yield break;
            }
        }
        StartCoroutine(CalRoundTime(_restTime));
    }
    #endregion

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
                    BattleResult = Result.DEFEAT;
                }
                else
                {
                    SoundManager.instance.Play("UI/Eff_Round_Win", SoundManager.Sound.Effect);
                    BattleResult = Result.VICTORY;
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
        fieldManagers[0].fieldPieceStatus.ActiveFieldStatus();

        fieldManagers[0].Reward(currentRound, BattleResult);

        BattleResult = Result.NONE;
        fieldManagers[0].NextStage();
        currentRound++;
        ChangeStage(currentRound);
        roundState.UpdateStageIcon(currentRound, 3, fieldManagers[0].stageInformation.enemy[currentRound].roundType);
        
    }

    public void StartBattle()
    {
        if (roundType == RoundType.Battle)
            return;

        roundType = RoundType.Battle;
        SoundManager.instance.Play("UI/Eff_Button_Positive", SoundManager.Sound.Effect);
        fieldManagers[0].fieldPieceStatus.ActiveFieldStatus();

        foreach (var list in fieldManagers[0].pieceDpList)
            fieldManagers[0].pieceStatus.AddPieceStatus(list.piece);

        fieldManagers[0].ActiveSynerge();

        if (fieldManagers[0].myFilePieceList.Count == 0)
        {
            if (fieldManagers[0].enemyFilePieceList.Count == 0)
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

        foreach (var piece in fieldManagers[0].myFilePieceList)
            piece.StartNextBehavior();
        foreach (var piece in fieldManagers[0].enemyFilePieceList)
            piece.StartNextBehavior();
    }

    private void ChangeStage(int round)
    {
        roundState.NextRound(round);
        roundState.OnRoundPopup(1, round);
    }

    private IEnumerator StartGame()
    {
        fieldManagers[0].ChangeMap(currentRound);
        fieldManagers[0].ChangeGold(fieldManagers[0].owerPlayer.gold);
        fieldManagers[0].ChangeHP(fieldManagers[0].owerPlayer.lifePoint);
        fieldManagers[0].ChangeLevel(fieldManagers[0].owerPlayer.level);

        roundState.SetStage(currentRound);
        yield return new WaitForSeconds(1f);
        ChangeStage(currentRound);
        fieldManagers[0].SpawnEnemy(currentRound);
        roundState.InitRoundIcon();
        roundState.UpdateStageIcon(currentRound, 3, fieldManagers[0].stageInformation.enemy[currentRound].roundType);

        fieldManagers[0].fieldPieceStatus.UpdateFieldStatus(fieldManagers[0].myFilePieceList.Count, fieldManagers[0].owerPlayer.maxPieceCount[fieldManagers[0].owerPlayer.level]);
    }
    #endregion

    #region 매칭
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) { Matching(); }
        if (Input.GetKeyDown(KeyCode.S)) { InitMatchingHistory(); }
    }

    private void Matching()
    {
        float playerCountHalf = fieldManagers.Count / 2;
        int pairCount = Mathf.RoundToInt(playerCountHalf);

        List<Messenger> messengerList = new List<Messenger>();
        playerMatchingPair = new List<PlayerPair>();

        for (int i = 0; i < fieldManagers.Count; i++)
        {
            fieldManagers[i].owerPlayer.isExpedition = false;
            fieldManagers[i].owerPlayer.matchingInformation.pairings = false;
            messengerList.Add(fieldManagers[i].owerPlayer);
        }

        List<Messenger> aGroup = new List<Messenger>();
        List<Messenger> bGroup = new List<Messenger>();

        for (int i = 0; i < pairCount; i++)
        {
            int m = Random.Range(0, messengerList.Count);
            aGroup.Add(messengerList[m]);
            messengerList.RemoveAt(m);
        }
        bGroup = messengerList;

        for (int i = 0; i < aGroup.Count; i++)
        {
            DFS(aGroup[i], bGroup);
        }
    }

    private bool DFS(Messenger homePlayer, List<Messenger> awayPlayerList)
    {
        List<Messenger> canPairingsPlayer = new List<Messenger>();
        canPairingsPlayer = awayPlayerList;

        for (int i = 0; i < homePlayer.matchingInformation.matchingHistroy.Count; i++)
        {
            for (int j = 0; j < canPairingsPlayer.Count; j++)
            {
                if (homePlayer.matchingInformation.matchingHistroy[i] == canPairingsPlayer[j].matchingInformation.myIndex)
                {
                    canPairingsPlayer.RemoveAt(j);
                    break;
                }
            }
        }

        for (int i = 0; i < canPairingsPlayer.Count; i++)
        {
            int t = canPairingsPlayer[i].matchingInformation.myIndex;
            Messenger targetPlayer = GetMessenger(t);
            if (targetPlayer.matchingInformation.pairings) continue;
            targetPlayer.matchingInformation.pairings = true;

            if (!homePlayer.matchingInformation.pairings || DFS(targetPlayer, awayPlayerList))
            {
                PlayerMatching(homePlayer, targetPlayer);
                return true;
            }
        }

        return false;
    }

    private bool CheckPairings(int playerIndex)
    {
        for (int i = 0; i < playerMatchingPair.Count; i++)
        {
            if (playerMatchingPair[i].awayPlayer.matchingInformation.myIndex == playerIndex)
                return true;
        }

        return false;
    }

    private Messenger GetMessenger(int index)
    {
        for (int i = 0; i < fieldManagers.Count; i++)
        {
            if (fieldManagers[i].owerPlayer.matchingInformation.myIndex == index)
                return fieldManagers[i].owerPlayer;
        }

        return null;
    }

    private void InitMatchingHistory()
    {
        for (int i = 0; i < fieldManagers.Count; i++)
            fieldManagers[i].owerPlayer.matchingInformation.HistoryIniti();
    }

    private void PlayerMatching(Messenger home, Messenger away)
    {
        PlayerPair pair = new PlayerPair(home, away);
        playerMatchingPair.Add(pair);

        home.matchingInformation.matchingHistroy.Add(away.matchingInformation.myIndex);
        away.matchingInformation.matchingHistroy.Add(home.matchingInformation.myIndex);
    }
    #endregion
}

[System.Serializable]
public class PlayerPair
{
    public Messenger homePlayer;
    public Messenger awayPlayer;

    public PlayerPair(Messenger home, Messenger away)
    {
        homePlayer = home;
        awayPlayer = away;
    }
}