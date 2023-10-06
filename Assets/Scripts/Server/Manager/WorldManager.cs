using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
using BackEnd;
using BackEnd.Tcp;

/// <summary>
/// 서버와 통신하는 가장 중요한 코드라 생각됨
/// 플레이어의 세션ID와 플레이어의 상태 이벤트를 받아옴
/// 또한 게임매니저 + 인게임UI와도 연결되어 있음
/// 여기도 비슷하게 서버와 슈퍼플레이어로 나누어서 처리가 진행됨
/// 플레이어의 상태 메세지를 전달함 ex) 플레이어 움직임, 공격함, 죽음, 데미지 등
/// 플레이어의 싱크도 관리하는 것으로 보임
/// </summary>
public class WorldManager : MonoBehaviour
{

    static public WorldManager instance;

    const int START_COUNT = 5;

    private SessionId myPlayerIndex = SessionId.None;

    #region 플레이어
    public GameObject playerPool;
    public GameObject playerPrefeb;
    public int numOfPlayer = 0;
    private const int MAXPLAYER = 8;
    public int alivePlayer { get; set; }
    private Dictionary<SessionId, Player> players;
    public GameObject startPointObject;
    private List<Vector4> statringPoints;

    private Stack<SessionId> gameRecord;
    public delegate void PlayerDie(SessionId index);
    public PlayerDie dieEvent;
    #endregion

    public List<float> playerTestFloatData = new List<float>();
    private readonly string playerTestField = "Player";


    public List<GameObject> playerTestPlayers = new List<GameObject>();
    public GameObject[] TestPlayers;
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
        CheckTestPlayerManagerData();
    }

    /*
	 * 플레이어 설정
	 * 게임 상태 함수 설정
	 */
    public bool InitializeGame()
    {
        if (!playerPool)
        {
            Debug.Log("Player Pool Not Exist!");
            return false;
        }
        Debug.Log("게임 초기화 진행");
        gameRecord = new Stack<SessionId>();
        GameManager.OnGameOver += OnGameOver;
        GameManager.OnGameResult += OnGameResult;
        myPlayerIndex = SessionId.None;
        SetPlayerAttribute();
        OnGameStart();
        return true;
    }

    public void SetPlayerAttribute()
    {
        // 시작점
        statringPoints = new List<Vector4>();

        int num = startPointObject.transform.childCount;
        for (int i = 0; i < num; ++i)
        {
            var child = startPointObject.transform.GetChild(i);
            Vector4 point = child.transform.position;
            point.w = child.transform.rotation.eulerAngles.y;
            statringPoints.Add(point);
        }
        dieEvent += PlayerDieEvent;
    }

    private void PlayerDieEvent(SessionId index)
    {
        alivePlayer -= 1;
        //players[index].gameObject.SetActive(false);
        players[index].gameObject.transform.position = new Vector3(0, -10, 0);
        Rigidbody tt = players[index].gameObject.GetComponent<Rigidbody>();
        tt.isKinematic = true;
        BoxCollider bc = players[index].gameObject.GetComponent<BoxCollider>();
        bc.isTrigger = true;
        InGameUiManager.GetInstance().SetScoreBoard(alivePlayer);
        gameRecord.Push(index);

        Debug.Log(string.Format("Player Die : " + players[index].GetNickName()));
        var PlayerTestManagers = players[index].gameObject.GetComponent<PlayerTestManager>();
        PlayerTestManagers.PlayerDieTest();//죽은 자신의 매니저에게 값을 보냄
        CheckTestPlayerManagerData();

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

    private void SendGameEndOrder()
    {
        // 게임 종료 전환 메시지는 호스트에서만 보냄----------------- 이것이 매우 중요 게임종료 메세지는 호스트에서만 보냄
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

    //플레이어의 정보를 설정
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
            GameObject player = Instantiate(playerPrefeb, new Vector3(statringPoints[index].x, statringPoints[index].y, statringPoints[index].z), Quaternion.identity, playerPool.transform);
            players.Add(sessionId, player.GetComponent<Player>());

            if (BackEndMatchManager.GetInstance().IsMySessionId(sessionId))
            {
                myPlayerIndex = sessionId;
                players[sessionId].Initialize(true, myPlayerIndex, BackEndMatchManager.GetInstance().GetNickNameBySessionId(sessionId), statringPoints[index].w);
            }
            else
            {
                players[sessionId].Initialize(false, sessionId, BackEndMatchManager.GetInstance().GetNickNameBySessionId(sessionId), statringPoints[index].w);
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

    //게임 결과
    public void OnGameResult()
    {
        Debug.Log("Game Result");
        //BackEndMatchManager.GetInstance().LeaveInGameRoom();

        if (GameManager.GetInstance().IsLobbyScene())
        {
            GameManager.GetInstance().ChangeState(GameManager.GameState.MatchLobby);
        }
    }

    //데이터를 받는다
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
        {
            return;
        }
        if (players == null)
        {
            Debug.LogError("Players 정보가 존재하지 않습니다.");
            return;
        }
        switch (msg.type) //메시지 이벤트 별로 데이터 처리
        {
            case Protocol.Type.StartCount:
                StartCountMessage startCount = DataParser.ReadJsonData<StartCountMessage>(args.BinaryUserData);
                Debug.Log("wait second : " + (startCount.time));
                InGameUiManager.GetInstance().SetStartCount(startCount.time);
                break;
            case Protocol.Type.GameStart:
                InGameUiManager.GetInstance().SetStartCount(0, false);
                GameManager.GetInstance().ChangeState(GameManager.GameState.InGame);
                break;
            case Protocol.Type.GameEnd:
                GameEndMessage endMessage = DataParser.ReadJsonData<GameEndMessage>(args.BinaryUserData);
                SetGameRecord(endMessage.count, endMessage.sessionList);
                GameManager.GetInstance().ChangeState(GameManager.GameState.Over);
                break;

            case Protocol.Type.Key:
                KeyMessage keyMessage = DataParser.ReadJsonData<KeyMessage>(args.BinaryUserData);
                ProcessKeyEvent(args.From.SessionId, keyMessage);
                break;
            case Protocol.Type.PlayerMove:
                PlayerMoveMessage moveMessage = DataParser.ReadJsonData<PlayerMoveMessage>(args.BinaryUserData);
                ProcessPlayerData(moveMessage);
                break;

            case Protocol.Type.PlayerDamaged:
                PlayerDamegedMessage damegedMessage = DataParser.ReadJsonData<PlayerDamegedMessage>(args.BinaryUserData);
                ProcessPlayerData(damegedMessage);
                break;

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

    public void OnRecieveForLocal(KeyMessage keyMessage)
    {
        ProcessKeyEvent(myPlayerIndex, keyMessage);
    }

    public void OnRecieveForLocal(PlayerNoMoveMessage message)
    {
        ProcessPlayerData(message);
    }

    // 키 입력 이벤트 관리하여 서버로 전송
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
        bool isDead = false;

        int keyData = keyMessage.keyData;

        Vector3 moveVecotr = Vector3.zero;
//        Vector3 attackPos = Vector3.zero;
        Vector3 playerPos = players[index].GetPosition();
        if ((keyData & KeyEventCode.MOVE) == KeyEventCode.MOVE)
        {
            moveVecotr = new Vector3(keyMessage.x, keyMessage.y, keyMessage.z);
            moveVecotr = Vector3.Normalize(moveVecotr);
            isMove = true;
        }
        if ((keyData & KeyEventCode.NO_MOVE) == KeyEventCode.NO_MOVE)
        {
            isNoMove = true;
        }

        if (isMove)
        {
            players[index].SetMoveVector(moveVecotr);
            PlayerMoveMessage msg = new PlayerMoveMessage(index, playerPos, moveVecotr);
            BackEndMatchManager.GetInstance().SendDataToInGame<PlayerMoveMessage>(msg);
        }
        if (isNoMove)
        {
            PlayerNoMoveMessage msg = new PlayerNoMoveMessage(index, playerPos);
            BackEndMatchManager.GetInstance().SendDataToInGame<PlayerNoMoveMessage>(msg);
        }
    }

    //플레이어 움직임 메세지 전송
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

    //플레이어 움직이지 않을때 전송 메세지
    private void ProcessPlayerData(PlayerNoMoveMessage data)
    {
        players[data.playerSession].SetPosition(data.xPos, data.yPos, data.zPos);
        players[data.playerSession].SetMoveVector(Vector3.zero);
    }

    //플레이어 공격 전송 메세지


    //플레이어 데미지 입었을때 전송 메세지
    private void ProcessPlayerData(PlayerDamegedMessage data)
    {
        //players[data.playerSession].Damaged();
        //EffectManager.instance.EnableEffect(data.hit_x, data.hit_y, data.hit_z);
    }

    //플레이어의 데이터를 동기화하는 것
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
//            player.Value.SetHP(syncMessage.hpValue[index]);
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

    public GameSyncMessage GetNowGameState(SessionId hostSession) //게임 싱크 위치 체력
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
            index++;
        }
        return new GameSyncMessage(hostSession, numOfClient, xPos, zPos, hp, online);
    }

    //public Vector3 GetMyPlayerPos()
    //{
    //    return players[myPlayerIndex].GetPosition();
    //}

    public void CheckTestPlayerManagerData()
    {
        var PlayerTestFields = GameObject.FindGameObjectsWithTag(playerTestField);
        for (int i = 0; i < PlayerTestFields.Length; i++)
        {
            playerTestFloatData.Add(PlayerTestFields[i].GetComponent<PlayerTestManager>().f); //순서대로 플레이어들이 들어와서 들어온 순서대로 게임오브젝트를 알고 있음
        }
    }

    public void KillPlayer()
    {
        var PlayerTestFields = GameObject.FindGameObjectsWithTag(playerTestField);
        for (int i = 0; i < PlayerTestFields.Length; i++)
        {
            playerTestPlayers.Add(PlayerTestFields[i]);
        }

        int k = Random.Range(0, 8);
        playerTestPlayers[k].transform.position = new Vector3(0, -3, 0);
    }
}
