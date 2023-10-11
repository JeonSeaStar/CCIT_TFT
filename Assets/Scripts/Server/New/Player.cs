using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Protocol;
using BackEnd.Tcp;
using BackEnd;
using TMPro;

public class Player : MonoBehaviour
{

    private SessionId index = 0;
    private string nickName = string.Empty;
    private bool isMe = false;

    // 스테이터스
    public int hp { get; private set; } = 0;
    private const int MAX_HP = 100;
    private bool isLive = false;

    // UI
    public GameObject nameObject;

    private readonly string playerCanvas = "PlayerCanvas";

    // 애니메이터
    // private Animator anim;

    // 이동관련
    public bool isMove { get; private set; }
    public Vector3 moveVector { get; private set; }

    public bool isRotate { get; private set; }

    private float rotSpeed = 4.0f;
    private float moveSpeed = 4.0f;

    private GameObject playerModelObject;
    private Rigidbody rigidBody;

    void Start()
    {
        if (BackEndMatchManager.GetInstance() == null)
        {

        }
    }

    public void Initialize(bool isMe, SessionId index, string nickName, float rot)
    {
        this.isMe = isMe;
        this.index = index;
        this.nickName = nickName;

        var playerUICanvas = GameObject.FindGameObjectWithTag(playerCanvas);
        nameObject = Instantiate(nameObject, Vector3.zero, Quaternion.identity, playerUICanvas.transform);

        nameObject.GetComponent<TMP_Text>().text = nickName;

        if (this.isMe)
        {
            Camera.main.GetComponent<FollowCamera>().target = this.transform;
        }

        this.isLive = true;

        this.isMove = false;
        this.moveVector = new Vector3(0, 0, 0);
        this.isRotate = false;

        //hp
        hp = MAX_HP;

        playerModelObject = this.gameObject;
        playerModelObject.transform.rotation = Quaternion.Euler(0, rot, 0);

        rigidBody = this.GetComponent<Rigidbody>();

        nameObject.transform.position = GetNameUIPos();
    }

    #region 이동관련 함수
    /*
     * 변화량만큼 이동
     * 특정 좌표로 이동
     */
    public void SetMoveVector(float move)
    {
        SetMoveVector(this.transform.forward * move);
    }
    public void SetMoveVector(Vector3 vector)
    {
        moveVector = vector;

        if (vector == Vector3.zero)
        {
            isMove = false;
        }
        else
        {
            isMove = true;
        }
    }

    public void Move()
    {
        Move(moveVector);
    }
    public void Move(Vector3 var)
    {
        if (!isLive)
        {
            return;
        }
        // 회전
        if (var.Equals(Vector3.zero))
        {
            isRotate = false;
        }
        else
        {
            if (Quaternion.Angle(playerModelObject.transform.rotation, Quaternion.LookRotation(var)) > Quaternion.kEpsilon)
            {
                isRotate = true;
            }
            else
            {
                isRotate = false;
            }
        }

        //playerModelObject.transform.rotation = Quaternion.LookRotation(var);

        // 이동
        var pos = gameObject.transform.position + playerModelObject.transform.forward * moveSpeed * Time.deltaTime;
        //gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, pos, Time.deltaTime * smoothVal);
        SetPosition(pos);
    }

    public void Rotate()
    {
        if (moveVector.Equals(Vector3.zero))
        {
            isRotate = false;
            return;
        }
        if (Quaternion.Angle(playerModelObject.transform.rotation, Quaternion.LookRotation(moveVector)) < Quaternion.kEpsilon)
        {
            isRotate = false;
            return;
        }
        playerModelObject.transform.rotation = Quaternion.Lerp(playerModelObject.transform.rotation, Quaternion.LookRotation(moveVector), Time.deltaTime * rotSpeed);
    }

    public void SetPosition(Vector3 pos)
    {
        if (!isLive)
        {
            return;
        }
        gameObject.transform.position = pos;
    }

    // isStatic이 true이면 해당 위치로 바로 이동
    public void SetPosition(float x, float y, float z)
    {
        if (!isLive)
        {
            return;
        }
        Vector3 pos = new Vector3(x, y, z);
        SetPosition(pos);
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }

    public Vector3 GetRotation()
    {
        //return gameObject.transform.rotation;
        return gameObject.transform.rotation.eulerAngles;
    }
    #endregion


    public bool GetIsLive()
    {
        return isLive;
    }

    public void Damaged()
    {
        hp -= 100;
    }

    public void SetHP(int hp)
    {
        this.hp = hp;
    }

    private void PlayerDie()
    {
        isLive = false;
        nameObject.SetActive(false);
    }

    Vector3 GetNameUIPos()
    {
        return this.transform.position + (Vector3.forward * 1.6f) + (Vector3.up * 2.5f);
    }

    void Update()
    {
        if (BackEndMatchManager.GetInstance() == null)
        {

        }

        if (!isLive)
        {
            return;
        }

        if (isMove)
        {
            Move();
        }

        if (isRotate)
        {
            Rotate();
        }

        if (transform.position.y < -10.0f)
        {
            PlayerDie();
            WorldManager.instance.dieEvent(index);
        }

        if (hp <= 0)
        {
            PlayerDie();
            WorldManager.instance.dieEvent(index);
        }

        if (nameObject.activeSelf)
        {
            nameObject.transform.position = GetNameUIPos();
        }
    }

    public SessionId GetIndex()
    {
        return index;
    }

    public bool IsMe()
    {
        return isMe;
    }

    public string GetNickName()
    {
        return nickName;
    }

    public void Test()
    {

    }
}