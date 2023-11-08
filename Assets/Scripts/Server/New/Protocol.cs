using BackEnd.Tcp;
using UnityEngine;
using System.Collections.Generic;

namespace Protocol
{
    // �̺�Ʈ Ÿ��
    public enum Type : sbyte
    {
        Key = 0,        // Ű(���� ���̽�ƽ) �Է�
        Button,
        PlayerMove,     // �÷��̾� �̵�
        PlayerRotate,   // �÷��̾� ȸ��
        PlayerAttack,   // �÷��̾� ����
        PlayerDamaged,  // �÷��̾� ������ ����
        PlayerDead, 
        PlayerNoMove,   // �÷��̾� �̵� ����
        PlayerNoRotate, // �÷��̾� ȸ�� ����
        PlayerTest,   // �÷��̾ �Լ� ���� �� �ִ��� Ȯ��
        PlayerBuyPiece,
        PlayerSellPiece,
        playerReroll,
        playerStoreLock,
        PlayerButtonLevelUp,
       
        PlayerTouchMove,
        PlayerBuyPiece0,


        bulletInfo,

        PieceDataRefresh,
        PieceSlotRefresh,

        AIPlayerInfo,       // AI�� �����ϴ� ��� AI ����
        LoadRoomScene,      // �� ������ ��ȯ
        LoadGameScene,      // �ΰ��� ������ ��ȯ
        StartCount,         // ���� ī��Ʈ
        GameStart,          // ���� ����
        InGameEvent,        // ���� ���� ���� �� x-1�� �������� �̺�Ʈ ��
        InGameWating,       // �̺�Ʈ ���� �⹰ ��ġ - ��ȭ ����
        InGameBattleReady,  // ���� ��� �̶� �� ����ȭ
        InGameBattle,       // ����
        InGameWinnerCheck,  //���� ������ �޽������� ������, ���� ����
        GameEnd,            // ���� ����
        GameMatching,
        GameSync,       // �÷��̾� ������ �� ���� ���� ��Ȳ ��ũ
        GamePlayerHpSync,
        Max
    }

    // �ִϸ��̼� ��ũ�� ������� �ʽ��ϴ�.

    public enum AnimIndex
    {
        idle = 0,
        walk,
        walkBack,
        stop,
        max
    }

    public enum PieceIndex
    {
        PieceMove = 0,
        pieceRotate,
        PieceAttack,
        PieceDamaged,
        PieceDead,
        pieceNoMove,
        PieceNoRotate,
    }


    // ���̽�ƽ Ű �̺�Ʈ �ڵ�
    public static class KeyEventCode
    {
        public const int NONE = 0;
        public const int MOVE = 1;      // �̵� �޽���
        public const int ATTACK = 2;    // ���� �޽���
        public const int NO_MOVE = 4;   // �̵� ���� �޽���

        public const int TOUCH_MOVE = 10;
    }

    public static class ButtonEventCode
    {
        public const int NONE = 0;
        public const int TESTBUTTON1 = 1; //�׽�Ʈ ��ư������ ������ ���� �÷��̾ �״°�
        public const int BUY = 2;
        public const int SELL = 3;
        public const int REROLL = 4;
        public const int STORELOCK = 5;
        public const int LEVELUP = 6;
        public const int MATCHING = 11;
        public const int BUYPIECE0 = 20;
    }


    public class Message
    {
        public Type type;

        public Message(Type type)
        {
            this.type = type;
        }
    }

    public class KeyMessage : Message
    {
        public int keyData;
        public float x;
        public float y;
        public float z;

        public KeyMessage(int data, Vector3 pos) : base(Type.Key)
        {
            this.keyData = data;
            this.x = pos.x;
            this.y = pos.y;
            this.z = pos.z;
        }
    }

    public class ButtonMessage : Message
    {
        public int ButtonData;
        public SessionId playerSession;
        public ButtonMessage(int data, SessionId sessionId) : base(Type.Button)
        {
            this.ButtonData = data;
            this.playerSession = sessionId;
        }
    }

    public class PlayerButtonDeadMessage : Message
    {
        public SessionId playerSession;
        public PlayerButtonDeadMessage(SessionId sessionId) : base(Type.PlayerDead)
        {
            this.playerSession = sessionId;
        }
    }

    public class PlayerButtonBuyMessage : Message
    {
        public SessionId playerSession;
        public float x;
        public float y;
        public float z;
        public PlayerButtonBuyMessage(SessionId sessionId, Vector3 pos) : base(Type.PlayerBuyPiece)
        {
            this.playerSession = sessionId;
            x = pos.x;
            y = pos.y;
            z = pos.z;
        }
    }

    public class PlayerBuyPiece0Message : Message
    {
        public SessionId playerSession;
        public PieceData pieceData;
        public int star;
        public Tile tile;
        public PlayerBuyPiece0Message(SessionId sessionId, PieceData pieceData, int star, Tile tile) : base(Type.PlayerBuyPiece0)
        {
            this.playerSession = sessionId;
            this.pieceData = pieceData;
            this.star = star;
            this.tile = tile;
        }
    }


    public class PlayerButtonSellMessage : Message
    {
        public SessionId playerSession;
        public PlayerButtonSellMessage(SessionId sessionId) : base(Type.PlayerSellPiece)
        {
            this.playerSession = sessionId;
        }
    }
    public class PlayerButtonRerollMessage : Message
    {
        public SessionId playerSession;
        public PieceData pieceData;
        public PlayerButtonRerollMessage(SessionId sessionId, PieceData pieceData) : base(Type.playerReroll)
        {
            this.playerSession = sessionId;
            this.pieceData = pieceData;
        }
    }
    public class PlayerButtonStoreLockMessage : Message
    {
        public SessionId playerSession;
        public PlayerButtonStoreLockMessage(SessionId sessionId) : base(Type.playerStoreLock)
        {
            this.playerSession = sessionId;
        }
    }

    public class PlayerButtonLevelUpMessage : Message
    {
        public SessionId playerSession;
        public PlayerButtonLevelUpMessage(SessionId sessionId) : base(Type.PlayerButtonLevelUp)
        {
            this.playerSession = sessionId;
        }
    }

    public class PlayerMoveMessage : Message
    {
        public SessionId playerSession;
        public float xPos;
        public float yPos;
        public float zPos;
        public float xDir;
        public float yDir;
        public float zDir;
        public PlayerMoveMessage(SessionId session, Vector3 pos, Vector3 dir) : base(Type.PlayerMove)
        {
            this.playerSession = session;
            this.xPos = pos.x;
            this.yPos = pos.y;
            this.zPos = pos.z;
            this.xDir = dir.x;
            this.yDir = dir.y;
            this.zDir = dir.z;
        }
    }

    public class PlayerTouchMoveMessaeg : Message
    {
        public SessionId playerSession;
        public float xPos;
        public float yPos;
        public float zPos;
        public float xDir;
        public float yDir;
        public float zDir;
        public PlayerTouchMoveMessaeg(SessionId session, Vector3 pos, Vector3 dir) : base(Type.PlayerTouchMove)
        {
            this.playerSession = session;
            this.xPos = pos.x;
            this.yPos = pos.y;
            this.zPos = pos.z;
            this.xDir = dir.x;
            this.yDir = dir.y;
            this.zDir = dir.z;
        }
    }

    public class PlayerAttackMessage : Message
    {
        public SessionId playerSession;
        public float dir_x;
        public float dir_y;
        public float dir_z;
        public PlayerAttackMessage(SessionId session, Vector3 pos) : base(Type.PlayerAttack)
        {
            this.playerSession = session;
            dir_x = pos.x;
            dir_y = pos.y;
            dir_z = pos.z;
        }
    }

    public class PlayerDamegedMessage : Message
    {
        public SessionId playerSession;
        public float hit_x;
        public float hit_y;
        public float hit_z;
        public PlayerDamegedMessage(SessionId session, float x, float y, float z) : base(Type.PlayerDamaged)
        {
            this.playerSession = session;
            this.hit_x = x;
            this.hit_y = y;
            this.hit_z = z;
        }
    }

    public class PlayerDeadMessage : Message
    {
        public SessionId playerSession;
        public PlayerDeadMessage(SessionId session) : base(Type.PlayerDead)
        {
            this.playerSession = session;
        }
    }

    public class PlayerNoMoveMessage : Message
    {
        public SessionId playerSession;
        public float xPos;
        public float yPos;
        public float zPos;
        public PlayerNoMoveMessage(SessionId session, Vector3 pos) : base(Type.PlayerNoMove)
        {
            this.playerSession = session;
            this.xPos = pos.x;
            this.yPos = pos.y;
            this.zPos = pos.z;
        }
    }

    public class PlayerBuyPiece : Message
    {
        public SessionId playerSession;

        public PlayerBuyPiece(SessionId session) : base(Type.PlayerBuyPiece)
        {
            this.playerSession = session;
        }
    }

    public class PieceMessage
    {
        public PieceIndex piece;

        public PieceMessage(PieceIndex pieceIndex)
        {
            this.piece = pieceIndex;
        }
    }

    public class PieceMoveMessage : PieceMessage
    {
        public int test;
        public PieceMoveMessage(int test) : base(PieceIndex.PieceMove)
        {
            this.test = test;
        }
    }
    public class AIPlayerInfo : Message
    {
        public SessionId m_sessionId;
        public string m_nickname;
        public byte m_teamNumber;
        public int m_numberOfMatches;
        public int m_numberOfWin;
        public int m_numberOfDraw;
        public int m_numberOfDefeats;
        public int m_points;
        public int m_mmr;

        public AIPlayerInfo(MatchUserGameRecord gameRecord) : base(Type.AIPlayerInfo)
        {
            this.m_sessionId = gameRecord.m_sessionId;
            this.m_nickname = gameRecord.m_nickname;
            this.m_teamNumber = gameRecord.m_teamNumber;
            this.m_numberOfWin = gameRecord.m_numberOfWin;
            this.m_numberOfDraw = gameRecord.m_numberOfDraw;
            this.m_numberOfDefeats = gameRecord.m_numberOfDefeats;
            this.m_points = gameRecord.m_points;
            this.m_mmr = gameRecord.m_mmr;
            this.m_numberOfMatches = gameRecord.m_numberOfMatches;
        }

        public MatchUserGameRecord GetMatchRecord()
        {
            MatchUserGameRecord gameRecord = new MatchUserGameRecord();
            gameRecord.m_sessionId = this.m_sessionId;
            gameRecord.m_nickname = this.m_nickname;
            gameRecord.m_numberOfMatches = this.m_numberOfMatches;
            gameRecord.m_numberOfWin = this.m_numberOfWin;
            gameRecord.m_numberOfDraw = this.m_numberOfDraw;
            gameRecord.m_numberOfDefeats = this.m_numberOfDefeats;
            gameRecord.m_mmr = this.m_mmr;
            gameRecord.m_points = this.m_points;
            gameRecord.m_teamNumber = this.m_teamNumber;

            return gameRecord;
        }
    }

    public class LoadRoomSceneMessage : Message
    {
        public LoadRoomSceneMessage() : base(Type.LoadRoomScene)
        {

        }
    }

    public class LoadGameSceneMessage : Message
    {
        public LoadGameSceneMessage() : base(Type.LoadGameScene)
        {

        }
    }

    public class StartCountMessage : Message
    {
        public int time;
        public StartCountMessage(int time) : base(Type.StartCount)
        {
            this.time = time;
        }
    }
    public class InGameEventCountMessage : Message
    {
        public int time;
        public InGameEventCountMessage(int time) : base(Type.InGameEvent)
        {
            this.time = time;
        }
    }
    public class InGameWatingCountMessage : Message
    {
        public int time;
        public InGameWatingCountMessage(int time) : base(Type.InGameWating)
        {
            this.time = time;
        }
    }
    public class InGameBattleReadyCountMessage : Message
    {
        public int time;
        public InGameBattleReadyCountMessage(int time) : base(Type.InGameBattleReady)
        {
            this.time = time;
        }
    }
    public class InGameBattleCountMessage : Message
    {
        public int time;
        public InGameBattleCountMessage(int time) : base(Type.InGameBattle)
        {
            this.time = time;
        }
    }
    public class InGameWinnerCheckCountMessage : Message
    {
        public int time;
        public InGameWinnerCheckCountMessage(int time) : base(Type.InGameWinnerCheck)
        {
            this.time = time;
        }
    }

    public class InGamePieceRefreshSlotsMessage : Message
    {
        //public SessionId Player;
        public PieceData pieceData;
        public InGamePieceRefreshSlotsMessage(PieceData pieceData) : base (Type.PieceDataRefresh)
        {
            //this.Player = sessionId;
            this.pieceData = pieceData;
        }
    }

    public class InGamePieceSlotRefreshMessage : Message
    {
        public PieceBuySlot pieceBuySlot;
        public InGamePieceSlotRefreshMessage(PieceBuySlot pieceBuySlot) : base (Type.PieceSlotRefresh)
        {
            this.pieceBuySlot = pieceBuySlot;
        }
    }


    //
    public class PlayerMatchingMessage : Message
    {
        public SessionId otherPlayerSession;
        public PlayerMatchingMessage(SessionId sessionId) : base(Type.GameMatching)
        {
            this.otherPlayerSession = sessionId;
        }
    }

    public class GameStartMessage : Message
    {
        public GameStartMessage() : base(Type.GameStart) { }
    }

    public class GameEndMessage : Message
    {
        public int count;
        public int[] sessionList;
        public GameEndMessage(Stack<SessionId> result) : base(Type.GameEnd)
        {
            count = result.Count;
            sessionList = new int[count];
            for (int i = 0; i < count; ++i)
            {
                sessionList[i] = (int)result.Pop();
            }
        }
    }

    public class GameSyncMessage : Message
    {
        public SessionId host;
        public int count = 0;
        public float[] xPos = null;
        public float[] zPos = null;
        public int[] hpValue = null;
        public bool[] onlineInfo = null;

        public GameSyncMessage(SessionId host, int count, float[] x, float[] z, int[] hp, bool[] online) : base(Type.GameSync)
        {
            this.host = host;
            this.count = count;
            this.xPos = new float[count];
            this.zPos = new float[count];
            this.hpValue = new int[count];
            this.onlineInfo = new bool[count];

            for (int i = 0; i < count; ++i)
            {
                xPos[i] = x[i];
                zPos[i] = z[i];
                hpValue[i] = hp[i];
                onlineInfo[i] = online[i];
            }
        }
    }
}
