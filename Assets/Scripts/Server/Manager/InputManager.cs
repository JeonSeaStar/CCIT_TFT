using UnityEngine;
using Protocol;

/// <summary>
/// 인풋 매니저는 어떻게 서버와 통신하는지 연결되어 있는지 확인하는 용도 + 슈퍼플레이어와도 어떻게 통신한는지
/// </summary>
public class InputManager : MonoBehaviour
{
    public VirtualStick virtualStick;

    private bool isMove = false;
    void Start()
    {
        GameManager.InGame += MobileInput;
        GameManager.InGame += AttackInput;
        GameManager.AfterInGame += SendNoMoveMessage;
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
        if (GameManager.GetInstance().GetGameState() != GameManager.GameState.InGame)
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
}
