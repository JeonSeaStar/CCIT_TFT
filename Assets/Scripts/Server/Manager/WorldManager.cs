using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
using BackEnd;
using BackEnd.Tcp;
using TMPro;

/// <summary>
/// 서버와 통신하는 가장 중요한 코드라 생각됨
/// 플레이어의 세션ID와 플레이어의 상태 이벤트를 받아옴
/// 또한 게임매니저 + 인게임UI와도 연결되어 있음
/// 슈퍼플레이어에서 모든 연산을 처리함
/// 플레이어의 상태 메세지를 전달함 ex) 플레이어 움직임, 공격함, 죽음, 데미지 등
/// 플레이어의 싱크도 관리하는 것으로 보임
/// </summary>

public class WorldManager : MonoBehaviour
{

    static public WorldManager instance;

    const int START_COUNT = 5;

    private SessionId myPlayerIndex = SessionId.None;

    #region 플레이어
    public GameObject[] playerPool;
    public GameObject playerPrefeb;
    public int numOfPlayer = 0;
    public GameObject particle;
    private const int MAXPLAYER = 8;
    public int alivePlayer { get; set; }
    private Dictionary<SessionId, Player> players;
    public GameObject[] startPointObject;
    private List<Vector4> statringPoints;

    private Stack<SessionId> gameRecord;
    public delegate void PlayerDie(SessionId index);
    public PlayerDie dieEvent;

    //public Player[] TestPlayers;
    #endregion
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        InitializeGame();
        var matchInstance = BackEndMatchManager.GetInstance();
        if (matchInstance == null)
        {
            return;
        }
        if (matchInstance.isReconnectProcess)
        {
            InGameUiManager.GetInstance().SetStartCount(0, false);
            InGameUiManager.GetInstance().SetReconnectBoard(BackEndServerManager.GetInstance().myNickName);
        }
    }

    /*
	 * 플레이어 설정
	 * 게임 상태 함수 설정
	 */
    public bool InitializeGame()
    {
        if (!playerPool[0])
        {
            Debug.Log("Player Pool Not Exist!");
            return false;
        }
        Debug.Log("게임 초기화 진행");
        gameRecord = new Stack<SessionId>();
        GameManager_Server.OnGameOver += OnGameOver;
        GameManager_Server.OnGameResult += OnGameResult;
        myPlayerIndex = SessionId.None;
        SetPlayerAttribute();
        OnGameStart();
        return true;
    }

    //각 플레이어 처음 시작점 설정
    public void SetPlayerAttribute()
    {
        // 시작점
        statringPoints = new List<Vector4>();

        int num = startPointObject.Length;
        for (int i = 0; i < num; ++i)
        {
            var child = startPointObject[i].transform;
            Vector4 point = child.transform.position;
            point.w = child.transform.rotation.eulerAngles.y;
            statringPoints.Add(point);
        }

        dieEvent += PlayerDieEvent;
    }

    //플레이어 죽음 이벤트 처리
    private void PlayerDieEvent(SessionId index)
    {
        alivePlayer -= 1;
        players[index].gameObject.SetActive(false);

        InGameUiManager.GetInstance().SetScoreBoard(alivePlayer);
        gameRecord.Push(index);

        Debug.Log(string.Format("Player Die : " + players[index].GetNickName()));

        // 호스트가 아니면 바로 리턴
        if (!BackEndMatchManager.GetInstance().IsHost())
        {
            return;
        }

        // 1명 이하로 플레이어가 남으면 바로 종료 체크
        if (alivePlayer <= 1)
        {
            SendGameEndOrder();
        }
    }

    //게임 종료 메세지 이벤트 발생
    private void SendGameEndOrder()
    {
        // 게임 종료 전환 메시지는 호스트에서만 보냄
        Debug.Log("Make GameResult & Send Game End Order");
        foreach (SessionId session in BackEndMatchManager.GetInstance().sessionIdList)
        {
            if (players[session].GetIsLive() && !gameRecord.Contains(session))
            {
                gameRecord.Push(session);
            }
        }
        GameEndMessage message = new GameEndMessage(gameRecord);
        BackEndMatchManager.GetInstance().SendDataToInGame<GameEndMessage>(message);
    }

    public SessionId GetMyPlayerIndex()
    {
        return myPlayerIndex;
    }

    //게임 시작시 플레이어 정보 셋팅
    public void SetPlayerInfo()
    {
        if (BackEndMatchManager.GetInstance().sessionIdList == null)
        {
            // 현재 세션ID 리스트가 존재하지 않으면, 0.5초 후 다시 실행
            Invoke("SetPlayerInfo", 0.5f);
            return;
        }
        var gamers = BackEndMatchManager.GetInstance().sessionIdList;
        int size = gamers.Count;
        if (size <= 0)
        {
            Debug.Log("No Player Exist!");
            return;
        }
        if (size > MAXPLAYER)
        {
            Debug.Log("Player Pool Exceed!");
            return;
        }

        players = new Dictionary<SessionId, Player>();
        BackEndMatchManager.GetInstance().SetPlayerSessionList(gamers);

        int index = 0;
        foreach (var sessionId in gamers)
        {
            GameObject player = Instantiate(playerPrefeb, new Vector3(statringPoints[index].x, statringPoints[index].y, statringPoints[index].z), Quaternion.identity, playerPool[index].transform);
            players.Add(sessionId, player.GetComponent<Player>());

            if (BackEndMatchManager.GetInstance().IsMySessionId(sessionId)) //나인거
            {
                myPlayerIndex = sessionId;
                players[sessionId].Initialize(true, myPlayerIndex, BackEndMatchManager.GetInstance().GetNickNameBySessionId(sessionId), statringPoints[index].w, playerPool[index].GetComponentInChildren<FieldManager>());
            }
            else //나 아닌거
            {
                players[sessionId].Initialize(false, sessionId, BackEndMatchManager.GetInstance().GetNickNameBySessionId(sessionId), statringPoints[index].w, playerPool[index].GetComponentInChildren<FieldManager>());
            }
            index += 1;
        }
        Debug.Log("Num Of Current Player : " + size);

        // 스코어 보드 설정
        alivePlayer = size;
        InGameUiManager.GetInstance().SetScoreBoard(alivePlayer);

        if (BackEndMatchManager.GetInstance().IsHost())
        {
            StartCoroutine("StartCount");
        }
    }

    public void OnGameStart()
    {
        if (BackEndMatchManager.GetInstance() == null)
        {
            // 카운트 다운 : 종료
            InGameUiManager.GetInstance().SetStartCount(0, false);
            return;
        }
        if (BackEndMatchManager.GetInstance().IsHost())
        {
            Debug.Log("플레이어 세션정보 확인");

            if (BackEndMatchManager.GetInstance().IsSessionListNull())
            {
                Debug.Log("Player Index Not Exist!");
                // 호스트 기준 세션데이터가 없으면 게임을 바로 종료한다.
                foreach (var session in BackEndMatchManager.GetInstance().sessionIdList)
                {
                    // 세션 순서대로 스택에 추가
                    gameRecord.Push(session);
                }
                GameEndMessage gameEndMessage = new GameEndMessage(gameRecord);
                BackEndMatchManager.GetInstance().SendDataToInGame<GameEndMessage>(gameEndMessage);
                return;
            }
        }
        SetPlayerInfo();
    }

    IEnumerator StartCount()
    {
        StartCountMessage msg = new StartCountMessage(START_COUNT);

        // 카운트 다운
        for (int i = 0; i < START_COUNT + 1; ++i)
        {
            msg.time = START_COUNT - i;
            BackEndMatchManager.GetInstance().SendDataToInGame<StartCountMessage>(msg);
            yield return new WaitForSeconds(1); //1초 단위
        }

        // 게임 시작 메시지를 전송
        GameStartMessage gameStartMessage = new GameStartMessage();
        BackEndMatchManager.GetInstance().SendDataToInGame<GameStartMessage>(gameStartMessage);
    }

    public void PreInGame()
    {
        foreach (var player in players)
        {
            player.Value.SetMoveVector(Vector3.zero);
        }
    }

    public void OnGameOver()
    {
        Debug.Log("Game End");
        if (BackEndMatchManager.GetInstance() == null)
        {
            Debug.LogError("매치매니저가 null 입니다.");
            return;
        }
        BackEndMatchManager.GetInstance().MatchGameOver(gameRecord);
    }

    public void OnGameResult()
    {
        Debug.Log("Game Result");
        //BackEndMatchManager.GetInstance().LeaveInGameRoom();

        if (GameManager_Server.GetInstance().IsLobbyScene())
        {
            GameManager_Server.GetInstance().ChangeState(GameManager_Server.GameState.MatchLobby);
        }
    }

    //데이터 브로드캐스팅 죽, 데이터를 수신받는 부분
    public void OnRecieve(MatchRelayEventArgs args)
    {
        if (args.BinaryUserData == null)
        {
            Debug.LogWarning(string.Format("빈 데이터가 브로드캐스팅 되었습니다.\n{0} - {1}", args.From, args.ErrInfo));
            // 데이터가 없으면 그냥 리턴
            return;
        }
        Message msg = DataParser.ReadJsonData<Message>(args.BinaryUserData);
        if (msg == null)
        {
            return;
        }
        if (BackEndMatchManager.GetInstance().IsHost() != true && args.From.SessionId == myPlayerIndex)
        //호스트도 아니고 내 Index 번호도 아니기 때문에 다른 유저의 index라면 작동한다. 따라서 데이터를 수신 받는 부분
        //즉, 비호스트가 메세지를 처리
        {
            return;
        }
        if (players == null)
        {
            Debug.LogError("Players 정보가 존재하지 않습니다.");
            return;
        }
        switch (msg.type)
        {
            case Protocol.Type.StartCount:
                StartCountMessage startCount = DataParser.ReadJsonData<StartCountMessage>(args.BinaryUserData);
                Debug.Log("wait second : " + (startCount.time));
                InGameUiManager.GetInstance().SetStartCount(startCount.time);
                break;
            case Protocol.Type.GameStart:
                InGameUiManager.GetInstance().SetStartCount(0, false);
                GameManager_Server.GetInstance().ChangeState(GameManager_Server.GameState.InGame);
                break;
            case Protocol.Type.GameEnd:
                GameEndMessage endMessage = DataParser.ReadJsonData<GameEndMessage>(args.BinaryUserData);
                SetGameRecord(endMessage.count, endMessage.sessionList);
                GameManager_Server.GetInstance().ChangeState(GameManager_Server.GameState.Over);
                break;

            case Protocol.Type.Key:
                KeyMessage keyMessage = DataParser.ReadJsonData<KeyMessage>(args.BinaryUserData);
                ProcessKeyEvent(args.From.SessionId, keyMessage);
                break;

            //버튼 테스트용
            case Protocol.Type.Button:
                ButtonMessage buttonMessage = DataParser.ReadJsonData<ButtonMessage>(args.BinaryUserData);
                ProceButtonEvent(args.From.SessionId, buttonMessage);
                break;
            //버튼 테스트용
            case Protocol.Type.PlayerDead:
                PlayerButtonDeadMessage playerButtonDeadMessage = DataParser.ReadJsonData<PlayerButtonDeadMessage>(args.BinaryUserData);
                ProcessPlayerData(playerButtonDeadMessage);
                break;
            case Protocol.Type.PlayerBuyPiece:
                PlayerButtonBuyMessage playerButtonBuyMessage = DataParser.ReadJsonData<PlayerButtonBuyMessage>(args.BinaryUserData);
                ProcessPlayerData(playerButtonBuyMessage);
                break;
            case Protocol.Type.PlayerSellPiece:
                PlayerButtonSellMessage playerButtonSellMessage = DataParser.ReadJsonData<PlayerButtonSellMessage>(args.BinaryUserData);
                ProcessPlayerData(playerButtonSellMessage);
                break;
            case Protocol.Type.playerReroll:
                PlayerButtonRerollMessage playerButtonRerollMessage = DataParser.ReadJsonData<PlayerButtonRerollMessage>(args.BinaryUserData);
                ProcessPlayerData(playerButtonRerollMessage);
                break;
            case Protocol.Type.playerStoreLock:
                PlayerButtonStoreLockMessage playerButtonStoreLockMessage = DataParser.ReadJsonData<PlayerButtonStoreLockMessage>(args.BinaryUserData);
                ProcessPlayerData(playerButtonStoreLockMessage);
                break;
            case Protocol.Type.PlayerButtonLevelUp:
                PlayerButtonLevelUpMessage playerButtonLevelUpMessage = DataParser.ReadJsonData<PlayerButtonLevelUpMessage>(args.BinaryUserData);
                ProcessPlayerData(playerButtonLevelUpMessage);
                break;


            case Protocol.Type.PlayerMove:
                PlayerMoveMessage moveMessage = DataParser.ReadJsonData<PlayerMoveMessage>(args.BinaryUserData);
                ProcessPlayerData(moveMessage);
                break;
            case Protocol.Type.PlayerAttack:
                PlayerAttackMessage attackMessage = DataParser.ReadJsonData<PlayerAttackMessage>(args.BinaryUserData);
                ProcessPlayerData(attackMessage);
                break;
            case Protocol.Type.PlayerDamaged:
                PlayerDamegedMessage damegedMessage = DataParser.ReadJsonData<PlayerDamegedMessage>(args.BinaryUserData);
                ProcessPlayerData(damegedMessage);
                break;
            //case Protocol.Type.PlayerDead:
            //    PlayerDeadMessage deadMessage = DataParser.ReadJsonData<PlayerDeadMessage>(args.BinaryUserData);
            //    ProcessPlayerData(deadMessage);
            //    break;
                //

            case Protocol.Type.PlayerNoMove:
                PlayerNoMoveMessage noMoveMessage = DataParser.ReadJsonData<PlayerNoMoveMessage>(args.BinaryUserData);
                ProcessPlayerData(noMoveMessage);
                break;
            case Protocol.Type.GameSync:
                GameSyncMessage syncMessage = DataParser.ReadJsonData<GameSyncMessage>(args.BinaryUserData);
                ProcessSyncData(syncMessage);
                break;
            default:
                Debug.Log("Unknown protocol type");
                return;
        }
    }

    //호스트만 데이터를 보냄
    public void OnRecieveForLocal(KeyMessage keyMessage)
    {
        ProcessKeyEvent(myPlayerIndex, keyMessage);
    }

    //호스트만 데이터를 보냄
    public void OnRecieveForLocal(PlayerNoMoveMessage message)
    {
        ProcessPlayerData(message);
    }

    //버튼 테스트용 호스트만 데이터를 보냄
    public void OnRecieveButtonForLocal(ButtonMessage buttonMessage)
    {
        ProceButtonEvent(myPlayerIndex, buttonMessage);
    }

    //호스트만 수행
    private void ProceButtonEvent(SessionId index, ButtonMessage buttonMessage)
    {
        if (BackEndMatchManager.GetInstance().IsHost() == false)
        {
            //호스트만 수행
            return;
        }
        bool button1 = false;
        bool buyPiece = false;
        bool sellPiece = false;
        bool PieceReroll = false;
        bool storeLock = false;
        bool levelUp = false;

        Vector3 piecePos = Vector3.zero;

        int ButtonData = buttonMessage.ButtonData;
        if((ButtonData & ButtonEventCode.TESTBUTTON1) == ButtonEventCode.TESTBUTTON1)
        {
            button1 = true;
        }
        if((ButtonData & ButtonEventCode.BUY) == ButtonEventCode.BUY)
        {
            buyPiece = true;
        }
        if ((ButtonData & ButtonEventCode.SELL) == ButtonEventCode.SELL)
        {
            sellPiece = true;
        }
        if ((ButtonData & ButtonEventCode.REROLL) == ButtonEventCode.REROLL)
        {
            PieceReroll = true;
        }
        if ((ButtonData & ButtonEventCode.STORELOCK) == ButtonEventCode.STORELOCK)
        {
            storeLock = true;
        }
        if ((ButtonData & ButtonEventCode.LEVELUP) == ButtonEventCode.LEVELUP)
        {
            levelUp = true;
        }

        if (button1)
        {
            players[index].Damaged();
            PlayerButtonDeadMessage msg = new PlayerButtonDeadMessage(index);
            BackEndMatchManager.GetInstance().SendDataToInGame<PlayerButtonDeadMessage>(msg);
        }
        if(buyPiece) //vector3 piecepos 받는중
        {
            players[index].BuyPiece(piecePos);
            PlayerButtonBuyMessage msg = new PlayerButtonBuyMessage(index, piecePos);
            BackEndMatchManager.GetInstance().SendDataToInGame<PlayerButtonBuyMessage>(msg);
        }
        if(sellPiece)
        {
            players[index].SellPiece();
            PlayerButtonSellMessage msg = new PlayerButtonSellMessage(index);
            BackEndMatchManager.GetInstance().SendDataToInGame<PlayerButtonSellMessage>(msg);
        }
        if(PieceReroll)
        {
            players[index].PieceReroll();
            PlayerButtonRerollMessage msg = new PlayerButtonRerollMessage(index);
            BackEndMatchManager.GetInstance().SendDataToInGame<PlayerButtonRerollMessage>(msg);
        }
        if(storeLock)
        {
            players[index].StoreLock();
            PlayerButtonStoreLockMessage msg = new PlayerButtonStoreLockMessage(index);
            BackEndMatchManager.GetInstance().SendDataToInGame<PlayerButtonStoreLockMessage>(msg);
        }
        if(levelUp)
        {
            players[index].ButtonLevelUp();
            PlayerButtonLevelUpMessage msg = new PlayerButtonLevelUpMessage(index);
            BackEndMatchManager.GetInstance().SendDataToInGame<PlayerButtonLevelUpMessage>(msg);
        }
    }

    //호스트에서 키메세지를 연산하는 부분
    private void ProcessKeyEvent(SessionId index, KeyMessage keyMessage)
    {
        if (BackEndMatchManager.GetInstance().IsHost() == false)
        {
            //호스트만 수행
            return;
        }
        bool isMove = false;
        bool isAttack = false;
        bool isNoMove = false;

        int keyData = keyMessage.keyData;

        Vector3 moveVecotr = Vector3.zero;
        Vector3 attackPos = Vector3.zero;
        Vector3 playerPos = players[index].GetPosition();
        if ((keyData & KeyEventCode.MOVE) == KeyEventCode.MOVE)
        {
            moveVecotr = new Vector3(keyMessage.x, keyMessage.y, keyMessage.z);
            moveVecotr = Vector3.Normalize(moveVecotr);
            isMove = true;
        }
        if ((keyData & KeyEventCode.ATTACK) == KeyEventCode.ATTACK)
        {
            attackPos = new Vector3(keyMessage.x, keyMessage.y, keyMessage.z);
            //players[index].Attack(attackPos);
            isAttack = true;
        }

        if ((keyData & KeyEventCode.NO_MOVE) == KeyEventCode.NO_MOVE)
        {
            isNoMove = true;
        }

        if (isMove)
        {
            players[index].SetMoveVector(moveVecotr); //연산
            PlayerMoveMessage msg = new PlayerMoveMessage(index, playerPos, moveVecotr); //연산 데이터 메세지로 변경
            BackEndMatchManager.GetInstance().SendDataToInGame<PlayerMoveMessage>(msg);  //연산한 데이터 메세지로 보냄
        }
        if (isNoMove)
        {
            PlayerNoMoveMessage msg = new PlayerNoMoveMessage(index, playerPos);
            BackEndMatchManager.GetInstance().SendDataToInGame<PlayerNoMoveMessage>(msg);
        }
        if (isAttack)
        {
            PlayerAttackMessage msg = new PlayerAttackMessage(index, attackPos);
            BackEndMatchManager.GetInstance().SendDataToInGame<PlayerAttackMessage>(msg);
        }
    }
    
    //구 버전 공격 호스트가 연산함
    private void ProcessAttackKeyData(SessionId session, Vector3 pos)
    {
        //players[session].Attack(pos);                                                 //연산
        PlayerAttackMessage msg = new PlayerAttackMessage(session, pos);                //연산한 데이터 메세지로 변경
        BackEndMatchManager.GetInstance().SendDataToInGame<PlayerAttackMessage>(msg);   //메세지를 호출
    }

    //비호스트에 대한 연산? --비호스트들의 적용 하는 부분
    private void ProcessPlayerData(PlayerMoveMessage data)
    {
        if (BackEndMatchManager.GetInstance().IsHost() == true)
        {
            //호스트면 리턴
            return;
        }
        Vector3 moveVecotr = new Vector3(data.xDir, data.yDir, data.zDir);
        // moveVector가 같으면 방향 & 이동량 같으므로 적용 굳이 안함
        if (!moveVecotr.Equals(players[data.playerSession].moveVector))
        {
            players[data.playerSession].SetPosition(data.xPos, data.yPos, data.zPos);
            players[data.playerSession].SetMoveVector(moveVecotr);
        }
    }

    //버튼 테스트용 비호스트에 대한 연산?
    private void ProcessPlayerData(PlayerButtonDeadMessage data) //누르면 죽어염
    {
        if (BackEndMatchManager.GetInstance().IsHost() == true)
        {
            //호스트면 리턴
            return;
        }
        players[data.playerSession].Damaged();
    }
    private void ProcessPlayerData(PlayerButtonBuyMessage data) // 누르면 기물 생성
    {
        if(BackEndMatchManager.GetInstance().IsHost() == true)
        {
            return;
        }
        players[data.playerSession].BuyPiece(new Vector3(data.x, data.y, data.z));
    }
    private void ProcessPlayerData(PlayerButtonSellMessage data) // 누르면 기물 판매
    {
        if(BackEndMatchManager.GetInstance().IsHost() == true)
        {
            return;
        }
        players[data.playerSession].SellPiece();
    }
    private void ProcessPlayerData(PlayerButtonRerollMessage data) // 누르면 기물 리롤
    {
        if (BackEndMatchManager.GetInstance().IsHost() == true)
        {
            return;
        }
        players[data.playerSession].PieceReroll();
    }
    private void ProcessPlayerData(PlayerButtonStoreLockMessage data) // 누르면 스토어 락
    {
        if (BackEndMatchManager.GetInstance().IsHost() == true)
        {
            return;
        }
        players[data.playerSession].StoreLock();
    }
    private void ProcessPlayerData(PlayerButtonLevelUpMessage data) // 누르면 플레이어 레벨업
    {
        if (BackEndMatchManager.GetInstance().IsHost() == true)
        {
            return;
        }
        players[data.playerSession].ButtonLevelUp();
    }
         


    private void ProcessPlayerData(PlayerNoMoveMessage data)
    {
        players[data.playerSession].SetPosition(data.xPos, data.yPos, data.zPos);
        players[data.playerSession].SetMoveVector(Vector3.zero);
    }

    //비호스트에 대한 연산?
    private void ProcessPlayerData(PlayerAttackMessage data)
    {
        if (BackEndMatchManager.GetInstance().IsHost() == true)
        {
            //호스트면 리턴 keyEvent로 받기 때문에 2번 받을 필요가 없어 리턴함
            return;
        }
        //players[data.playerSession].Attack(new Vector3(data.dir_x, data.dir_y, data.dir_z));
    }


    private void ProcessPlayerData(PlayerDamegedMessage data)
    {
        players[data.playerSession].Damaged();
        //EffectManager.instance.EnableEffect(data.hit_x, data.hit_y, data.hit_z);
    }


    private void ProcessPlayerData(PlayerDeadMessage data)
    {
        players[data.playerSession].Damaged();
    }


    private void ProcessSyncData(GameSyncMessage syncMessage)
    {
        // 플레이어 데이터 동기화
        int index = 0;
        if (players == null)
        {
            Debug.LogError("Player Poll is null!");
            return;
        }
        foreach (var player in players)
        {
            var y = player.Value.GetPosition().y;
            player.Value.SetPosition(new Vector3(syncMessage.xPos[index], y, syncMessage.zPos[index]));
            player.Value.SetHP(syncMessage.hpValue[index]);
            index++;
        }
        BackEndMatchManager.GetInstance().SetHostSession(syncMessage.host);
    }

    public bool IsMyPlayerMove()
    {
        return players[myPlayerIndex].isMove;
    }

    public bool IsMyPlayerRotate()
    {
        return players[myPlayerIndex].isRotate;
    }

    private void SetGameRecord(int count, int[] arr)
    {
        gameRecord = new Stack<SessionId>();
        // 스택에 넣어야 하므로 제일 뒤에서 부터 스택에 push
        for (int i = count - 1; i >= 0; --i)
        {
            gameRecord.Push((SessionId)arr[i]);
        }
    }

    public GameSyncMessage GetNowGameState(SessionId hostSession)
    {
        int numOfClient = players.Count;

        float[] xPos = new float[numOfClient];
        float[] zPos = new float[numOfClient];
        int[] hp = new int[numOfClient];
        bool[] online = new bool[numOfClient];
        int index = 0;
        foreach (var player in players)
        {
            xPos[index] = player.Value.GetPosition().x;
            zPos[index] = player.Value.GetPosition().z;
            hp[index] = player.Value.hp;
            index++;
        }
        return new GameSyncMessage(hostSession, numOfClient, xPos, zPos, hp, online);
    }

    public Vector3 GetMyPlayerPos()
    {
        return players[myPlayerIndex].GetPosition();
    }


    public SessionId GetMyPlayer() //테스트용으로 inputmanager에서 사용중
    {
        return myPlayerIndex;
    }
    //public void GetKillPlayer()
    //{
    //    for (int i = 0; i < playerPool.transform.childCount; i++)
    //    {
    //        TestPlayers[i] = playerPool.gameObject.transform.GetChild(i).gameObject.GetComponent<Player>();
    //    }
    //}

    //public void KillPlayer()
    //{
    //    int k = Random.Range(0, 8);
    //    if (TestPlayers[k])
    //    {
    //        Protocol.PlayerDeadMessage message =
    //            new Protocol.PlayerDeadMessage(TestPlayers[k].GetIndex());
    //        BackEndMatchManager.GetInstance().SendDataToInGame<Protocol.PlayerDeadMessage>(message);
    //    }

    //}
}
