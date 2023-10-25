using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Protocol;
using BackEnd;
using BackEnd.Tcp;
using TMPro;

/// <summary>
/// ������ ����ϴ� ���� �߿��� �ڵ�� ������
/// �÷��̾��� ����ID�� �÷��̾��� ���� �̺�Ʈ�� �޾ƿ�
/// ���� ���ӸŴ��� + �ΰ���UI�͵� ����Ǿ� ����
/// �����÷��̾�� ��� ������ ó����
/// �÷��̾��� ���� �޼����� ������ ex) �÷��̾� ������, ������, ����, ������ ��
/// �÷��̾��� ��ũ�� �����ϴ� ������ ����
/// </summary>

public class WorldManager : MonoBehaviour
{

    static public WorldManager instance;

    const int START_COUNT = 5;

    private SessionId myPlayerIndex = SessionId.None;

    #region �÷��̾�
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
	 * �÷��̾� ����
	 * ���� ���� �Լ� ����
	 */
    public bool InitializeGame()
    {
        if (!playerPool[0])
        {
            Debug.Log("Player Pool Not Exist!");
            return false;
        }
        Debug.Log("���� �ʱ�ȭ ����");
        gameRecord = new Stack<SessionId>();
        GameManager_Server.OnGameOver += OnGameOver;
        GameManager_Server.OnGameResult += OnGameResult;
        myPlayerIndex = SessionId.None;
        SetPlayerAttribute();
        OnGameStart();
        return true;
    }

    //�� �÷��̾� ó�� ������ ����
    public void SetPlayerAttribute()
    {
        // ������
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

    //�÷��̾� ���� �̺�Ʈ ó��
    private void PlayerDieEvent(SessionId index)
    {
        alivePlayer -= 1;
        players[index].gameObject.SetActive(false);

        InGameUiManager.GetInstance().SetScoreBoard(alivePlayer);
        gameRecord.Push(index);

        Debug.Log(string.Format("Player Die : " + players[index].GetNickName()));

        // ȣ��Ʈ�� �ƴϸ� �ٷ� ����
        if (!BackEndMatchManager.GetInstance().IsHost())
        {
            return;
        }

        // 1�� ���Ϸ� �÷��̾ ������ �ٷ� ���� üũ
        if (alivePlayer <= 1)
        {
            SendGameEndOrder();
        }
    }

    //���� ���� �޼��� �̺�Ʈ �߻�
    private void SendGameEndOrder()
    {
        // ���� ���� ��ȯ �޽����� ȣ��Ʈ������ ����
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

    //���� ���۽� �÷��̾� ���� ����
    public void SetPlayerInfo()
    {
        if (BackEndMatchManager.GetInstance().sessionIdList == null)
        {
            // ���� ����ID ����Ʈ�� �������� ������, 0.5�� �� �ٽ� ����
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

            if (BackEndMatchManager.GetInstance().IsMySessionId(sessionId)) //���ΰ�
            {
                myPlayerIndex = sessionId;
                players[sessionId].Initialize(true, myPlayerIndex, BackEndMatchManager.GetInstance().GetNickNameBySessionId(sessionId), statringPoints[index].w, playerPool[index].GetComponentInChildren<FieldManager>());
            }
            else //�� �ƴѰ�
            {
                players[sessionId].Initialize(false, sessionId, BackEndMatchManager.GetInstance().GetNickNameBySessionId(sessionId), statringPoints[index].w, playerPool[index].GetComponentInChildren<FieldManager>());
            }
            index += 1;
        }
        Debug.Log("Num Of Current Player : " + size);

        // ���ھ� ���� ����
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
            // ī��Ʈ �ٿ� : ����
            InGameUiManager.GetInstance().SetStartCount(0, false);
            return;
        }
        if (BackEndMatchManager.GetInstance().IsHost())
        {
            Debug.Log("�÷��̾� �������� Ȯ��");

            if (BackEndMatchManager.GetInstance().IsSessionListNull())
            {
                Debug.Log("Player Index Not Exist!");
                // ȣ��Ʈ ���� ���ǵ����Ͱ� ������ ������ �ٷ� �����Ѵ�.
                foreach (var session in BackEndMatchManager.GetInstance().sessionIdList)
                {
                    // ���� ������� ���ÿ� �߰�
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

        // ī��Ʈ �ٿ�
        for (int i = 0; i < START_COUNT + 1; ++i)
        {
            msg.time = START_COUNT - i;
            BackEndMatchManager.GetInstance().SendDataToInGame<StartCountMessage>(msg);
            yield return new WaitForSeconds(1); //1�� ����
        }

        // ���� ���� �޽����� ����
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
            Debug.LogError("��ġ�Ŵ����� null �Դϴ�.");
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

    //������ ��ε�ĳ���� ��, �����͸� ���Ź޴� �κ�
    public void OnRecieve(MatchRelayEventArgs args)
    {
        if (args.BinaryUserData == null)
        {
            Debug.LogWarning(string.Format("�� �����Ͱ� ��ε�ĳ���� �Ǿ����ϴ�.\n{0} - {1}", args.From, args.ErrInfo));
            // �����Ͱ� ������ �׳� ����
            return;
        }
        Message msg = DataParser.ReadJsonData<Message>(args.BinaryUserData);
        if (msg == null)
        {
            return;
        }
        if (BackEndMatchManager.GetInstance().IsHost() != true && args.From.SessionId == myPlayerIndex)
        //ȣ��Ʈ�� �ƴϰ� �� Index ��ȣ�� �ƴϱ� ������ �ٸ� ������ index��� �۵��Ѵ�. ���� �����͸� ���� �޴� �κ�
        //��, ��ȣ��Ʈ�� �޼����� ó��
        {
            return;
        }
        if (players == null)
        {
            Debug.LogError("Players ������ �������� �ʽ��ϴ�.");
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

            //��ư �׽�Ʈ��
            case Protocol.Type.Button:
                ButtonMessage buttonMessage = DataParser.ReadJsonData<ButtonMessage>(args.BinaryUserData);
                ProceButtonEvent(args.From.SessionId, buttonMessage);
                break;
            //��ư �׽�Ʈ��
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

    //ȣ��Ʈ�� �����͸� ����
    public void OnRecieveForLocal(KeyMessage keyMessage)
    {
        ProcessKeyEvent(myPlayerIndex, keyMessage);
    }

    //ȣ��Ʈ�� �����͸� ����
    public void OnRecieveForLocal(PlayerNoMoveMessage message)
    {
        ProcessPlayerData(message);
    }

    //��ư �׽�Ʈ�� ȣ��Ʈ�� �����͸� ����
    public void OnRecieveButtonForLocal(ButtonMessage buttonMessage)
    {
        ProceButtonEvent(myPlayerIndex, buttonMessage);
    }

    //ȣ��Ʈ�� ����
    private void ProceButtonEvent(SessionId index, ButtonMessage buttonMessage)
    {
        if (BackEndMatchManager.GetInstance().IsHost() == false)
        {
            //ȣ��Ʈ�� ����
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
        if(buyPiece) //vector3 piecepos �޴���
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

    //ȣ��Ʈ���� Ű�޼����� �����ϴ� �κ�
    private void ProcessKeyEvent(SessionId index, KeyMessage keyMessage)
    {
        if (BackEndMatchManager.GetInstance().IsHost() == false)
        {
            //ȣ��Ʈ�� ����
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
            players[index].SetMoveVector(moveVecotr); //����
            PlayerMoveMessage msg = new PlayerMoveMessage(index, playerPos, moveVecotr); //���� ������ �޼����� ����
            BackEndMatchManager.GetInstance().SendDataToInGame<PlayerMoveMessage>(msg);  //������ ������ �޼����� ����
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
    
    //�� ���� ���� ȣ��Ʈ�� ������
    private void ProcessAttackKeyData(SessionId session, Vector3 pos)
    {
        //players[session].Attack(pos);                                                 //����
        PlayerAttackMessage msg = new PlayerAttackMessage(session, pos);                //������ ������ �޼����� ����
        BackEndMatchManager.GetInstance().SendDataToInGame<PlayerAttackMessage>(msg);   //�޼����� ȣ��
    }

    //��ȣ��Ʈ�� ���� ����? --��ȣ��Ʈ���� ���� �ϴ� �κ�
    private void ProcessPlayerData(PlayerMoveMessage data)
    {
        if (BackEndMatchManager.GetInstance().IsHost() == true)
        {
            //ȣ��Ʈ�� ����
            return;
        }
        Vector3 moveVecotr = new Vector3(data.xDir, data.yDir, data.zDir);
        // moveVector�� ������ ���� & �̵��� �����Ƿ� ���� ���� ����
        if (!moveVecotr.Equals(players[data.playerSession].moveVector))
        {
            players[data.playerSession].SetPosition(data.xPos, data.yPos, data.zPos);
            players[data.playerSession].SetMoveVector(moveVecotr);
        }
    }

    //��ư �׽�Ʈ�� ��ȣ��Ʈ�� ���� ����?
    private void ProcessPlayerData(PlayerButtonDeadMessage data) //������ �׾
    {
        if (BackEndMatchManager.GetInstance().IsHost() == true)
        {
            //ȣ��Ʈ�� ����
            return;
        }
        players[data.playerSession].Damaged();
    }
    private void ProcessPlayerData(PlayerButtonBuyMessage data) // ������ �⹰ ����
    {
        if(BackEndMatchManager.GetInstance().IsHost() == true)
        {
            return;
        }
        players[data.playerSession].BuyPiece(new Vector3(data.x, data.y, data.z));
    }
    private void ProcessPlayerData(PlayerButtonSellMessage data) // ������ �⹰ �Ǹ�
    {
        if(BackEndMatchManager.GetInstance().IsHost() == true)
        {
            return;
        }
        players[data.playerSession].SellPiece();
    }
    private void ProcessPlayerData(PlayerButtonRerollMessage data) // ������ �⹰ ����
    {
        if (BackEndMatchManager.GetInstance().IsHost() == true)
        {
            return;
        }
        players[data.playerSession].PieceReroll();
    }
    private void ProcessPlayerData(PlayerButtonStoreLockMessage data) // ������ ����� ��
    {
        if (BackEndMatchManager.GetInstance().IsHost() == true)
        {
            return;
        }
        players[data.playerSession].StoreLock();
    }
    private void ProcessPlayerData(PlayerButtonLevelUpMessage data) // ������ �÷��̾� ������
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

    //��ȣ��Ʈ�� ���� ����?
    private void ProcessPlayerData(PlayerAttackMessage data)
    {
        if (BackEndMatchManager.GetInstance().IsHost() == true)
        {
            //ȣ��Ʈ�� ���� keyEvent�� �ޱ� ������ 2�� ���� �ʿ䰡 ���� ������
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
        // �÷��̾� ������ ����ȭ
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
        // ���ÿ� �־�� �ϹǷ� ���� �ڿ��� ���� ���ÿ� push
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


    public SessionId GetMyPlayer() //�׽�Ʈ������ inputmanager���� �����
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
