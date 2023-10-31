using UnityEngine;
using Protocol;
using BackEnd.Tcp;
/// <summary>
/// 인풋 매니저는 어떻게 서버와 통신하는지 연결되어 있는지 확인하는 용도 + 슈퍼플레이어와도 어떻게 통신한는지
/// </summary>
public class InputManager : MonoBehaviour
{
    public VirtualStick virtualStick;
    [SerializeField] LayerMask playerMask; //9 - Player
    [SerializeField] LayerMask groundMask; //8 - Plane
    Vector3 test;

    private bool isMove = false;
    private bool isTouchMove = false;
    void Start()
    {
        GameManager_Server.InGame += MobileInput;
        GameManager_Server.InGame += AttackInput;
        //GameManager_Server.InGame += TouchMoveInput;
        GameManager_Server.AfterInGame += SendNoMoveMessage;
    }

    void MobileInput()
    {


        if (!virtualStick)
        {
            return;
        }

        int keyCode = 0;
        isMove = false;

        if (!virtualStick.isInputEnable)
        {
#if !UNITY_EDITOR
			isMove = false;
#endif
            return;
        }

        isMove = true;

        keyCode |= KeyEventCode.MOVE;
        Vector3 moveVector = new Vector3(virtualStick.GetHorizontalValue(), 0, virtualStick.GetVerticalValue());
        moveVector = Vector3.Normalize(moveVector);


        if (keyCode <= 0)
        {
            return;
        }

        KeyMessage msg;
        msg = new KeyMessage(keyCode, moveVector);
        if (BackEndMatchManager.GetInstance().IsHost())
        {
            BackEndMatchManager.GetInstance().AddMsgToLocalQueue(msg);
        }
        else
        {
            BackEndMatchManager.GetInstance().SendDataToInGame<KeyMessage>(msg);
        }
    }



    void TouchMoveInput()
    {
        int keyCode = 0;
        isTouchMove = false;


        keyCode |= KeyEventCode.TOUCH_MOVE;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, groundMask))
        {
            isTouchMove = true;
            Vector3 moveVector = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
            test = moveVector;
        }
        Debug.Log(test);
        

        if(keyCode <= 0)
        {
            return;
        }

        KeyMessage msg;
        msg = new KeyMessage(keyCode, test);
        if (BackEndMatchManager.GetInstance().IsHost())
        {
            BackEndMatchManager.GetInstance().AddMsgToLocalQueue(msg);
        }
        else
        {
            BackEndMatchManager.GetInstance().SendDataToInGame<KeyMessage>(msg);
        }
    }



    void AttackInput()
    {
        int keyCode = 0;
        keyCode |= KeyEventCode.ATTACK;

        Vector3 aimPos = new Vector3(0, 0, 0);
        if (aimPos.Equals(Vector3.zero))
        {
            return;
        }
//        aimPos += WorldManager.instance.GetMyPlayerPos();

        KeyMessage msg;
        msg = new KeyMessage(keyCode, aimPos);
        if (BackEndMatchManager.GetInstance().IsHost())
        {
            BackEndMatchManager.GetInstance().AddMsgToLocalQueue(msg);
        }
        else
        {
            BackEndMatchManager.GetInstance().SendDataToInGame<KeyMessage>(msg);
        }
    }

    public void AttackInput(Vector3 pos)
    {
        if (GameManager_Server.GetInstance().GetGameState() != GameManager_Server.GameState.InGame)
        {
            return;
        }
        int keyCode = 0;
        keyCode |= KeyEventCode.ATTACK;

        KeyMessage msg;
        msg = new KeyMessage(keyCode, pos);
        if (BackEndMatchManager.GetInstance().IsHost())
        {
            BackEndMatchManager.GetInstance().AddMsgToLocalQueue(msg);
        }
        else
        {
            BackEndMatchManager.GetInstance().SendDataToInGame<KeyMessage>(msg);
        }
    }

    void SendNoMoveMessage()
    {
        int keyCode = 0;
        if (!isMove && WorldManager.instance.IsMyPlayerMove())
        {
            keyCode |= KeyEventCode.NO_MOVE;
        }
        if (keyCode == 0)
        {
            return;
        }
        KeyMessage msg = new KeyMessage(keyCode, Vector3.zero);

        if (BackEndMatchManager.GetInstance().IsHost())
        {
            BackEndMatchManager.GetInstance().AddMsgToLocalQueue(msg);
        }
        else
        {
            BackEndMatchManager.GetInstance().SendDataToInGame<KeyMessage>(msg);
        }
    }

    public void TestButtonInput() //현재 사용중 플레이어 피 다는거
    {
        int ButtonCode = 0;
        ButtonCode |= ButtonEventCode.TESTBUTTON1;

        SessionId Player= WorldManager.instance.GetMyPlayer();

        ButtonMessage msg;
        msg = new ButtonMessage(ButtonCode, Player);

        if (BackEndMatchManager.GetInstance().IsHost())
        {
            BackEndMatchManager.GetInstance().AddMsgToButtonLocalQueue(msg);
        }
        else
        {
            BackEndMatchManager.GetInstance().SendDataToInGame<ButtonMessage>(msg);
        }
    }

    public void BuyButtonInput() //박스 오브젝트 한쪽으로 인스턴스
    {
        int ButtonCode = 0;
        ButtonCode |= ButtonEventCode.BUY;

        SessionId Player = WorldManager.instance.GetMyPlayer();

        ButtonMessage msg;
        msg = new ButtonMessage(ButtonCode, Player);

        if (BackEndMatchManager.GetInstance().IsHost())
        {
            BackEndMatchManager.GetInstance().AddMsgToButtonLocalQueue(msg);
        }
        else
        {
            BackEndMatchManager.GetInstance().SendDataToInGame<ButtonMessage>(msg);
        }
    }

    public void SellButtonInput()
    {
        int ButtonCode = 0;
        ButtonCode |= ButtonEventCode.SELL;

        SessionId Player = WorldManager.instance.GetMyPlayer();

        ButtonMessage msg;
        msg = new ButtonMessage(ButtonCode, Player);

        if (BackEndMatchManager.GetInstance().IsHost())
        {
            BackEndMatchManager.GetInstance().AddMsgToButtonLocalQueue(msg);
        }
        else
        {
            BackEndMatchManager.GetInstance().SendDataToInGame<ButtonMessage>(msg);
        }
    }

    public void RerollButtonInput()
    {
        int ButtonCode = 0;
        ButtonCode |= ButtonEventCode.REROLL;

        SessionId Player = WorldManager.instance.GetMyPlayer();

        ButtonMessage msg;
        msg = new ButtonMessage(ButtonCode, Player);

        if (BackEndMatchManager.GetInstance().IsHost())
        {
            BackEndMatchManager.GetInstance().AddMsgToButtonLocalQueue(msg);
        }
        else
        {
            BackEndMatchManager.GetInstance().SendDataToInGame<ButtonMessage>(msg);
        }
    }

    public void StoreLockButtonInput()
    {
        int ButtonCode = 0;
        ButtonCode |= ButtonEventCode.STORELOCK;

        SessionId Player = WorldManager.instance.GetMyPlayer();

        ButtonMessage msg;
        msg = new ButtonMessage(ButtonCode, Player);

        if (BackEndMatchManager.GetInstance().IsHost())
        {
            BackEndMatchManager.GetInstance().AddMsgToButtonLocalQueue(msg);
        }
        else
        {
            BackEndMatchManager.GetInstance().SendDataToInGame<ButtonMessage>(msg);
        }
    }
    public void LevelUpButtonInput()
    {
        int ButtonCode = 0;
        ButtonCode |= ButtonEventCode.LEVELUP;

        SessionId Player = WorldManager.instance.GetMyPlayer();

        ButtonMessage msg;
        msg = new ButtonMessage(ButtonCode, Player);

        if (BackEndMatchManager.GetInstance().IsHost())
        {
            BackEndMatchManager.GetInstance().AddMsgToButtonLocalQueue(msg);
        }
        else
        {
            BackEndMatchManager.GetInstance().SendDataToInGame<ButtonMessage>(msg);
        }
    }

    
}
