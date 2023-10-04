using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Protocol;
using BackEnd.Tcp;
using BackEnd;
using TMPro;

/// <summary>
/// 플레이어 스크립트 세션 초기화 및 플레이어가 설정해야 되는 함수들이 있음
/// 유일하게 통신하는 것은 BackEndMatchManager와 연결됨
/// </summary>
public class Player : MonoBehaviour
{
    [SerializeField] private SessionId index = 0;
    [SerializeField] private string nickName = string.Empty;
    [SerializeField] private bool isMe = false;
    
    // 스테이터스
    public int hp { get; private set; } = 0;
    private const int MAX_HP = 5;
    public bool isLive = false;

    // UI
    public GameObject nameObject;

    private readonly string playerCanvas = "PlayerCanvas";

    // 이동관련
    public bool isMove { get; private set; }
    public Vector3 moveVector { get; private set; }

    public bool isRotate { get; private set; }

    private GameObject playerModelObject;

    //테스트 값들
    public List<GameObject> playerTestFieldGameObjectList = new List<GameObject>();
    public List<PlayerTestManager> playerTestFieldList = new List<PlayerTestManager>();
    private readonly string playerTestField = "Player";

    void Start()
    {
        if (BackEndMatchManager.GetInstance() == null)
        {
            // 매칭 인스턴스가 존재하지 않을 경우 (인게임 테스트 용도)
            Initialize(true, SessionId.None, "testPlayer", 0);
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
            Debug.Log(gameObject.transform.position);
        }

        this.isLive = true;

        this.isMove = false;
        this.moveVector = new Vector3(0, 0, 0);
        this.isRotate = false;

        //hp
        hp = MAX_HP;

        playerModelObject = this.gameObject;
        playerModelObject.transform.rotation = Quaternion.Euler(0, rot, 0);

        nameObject.transform.position = GetNameUIPos();

        StartCoroutine(CheckTestField());
    }

    IEnumerator CheckTestField()
    {
        yield return new WaitForSeconds(5f);
        var PlayerTestFields = GameObject.FindGameObjectsWithTag(playerTestField);
        for (int i = 0; i < PlayerTestFields.Length; i++)
        {
            playerTestFieldGameObjectList.Add(PlayerTestFields[i]); //순서대로 플레이어들이 들어와서 들어온 순서대로 게임오브젝트를 알고 있음
            playerTestFieldList.Add(PlayerTestFields[i].GetComponent<PlayerTestManager>());
        }
    }

    #region 이동관련 함수
    /*
     * 변화량만큼 이동
     * 특정 좌표로 이동
     */
    public void SetMoveVector(Vector3 vector)
    {

    }

    public void SetPosition(Vector3 pos)
    {

    }

    // isStatic이 true이면 해당 위치로 바로 이동
    public void SetPosition(float x, float y, float z)
    {
        //if (!isLive)
        //{
        //    return;
        //}
        //Vector3 pos = new Vector3(x, y, z);
        //SetPosition(pos);
    }

    public Vector3 GetPosition()
    {
        return gameObject.transform.position;
    }

    public Vector3 GetRotation()
    {
        return gameObject.transform.rotation.eulerAngles;
    }
    #endregion

    public void Attack(Vector3 target)
    {
        //if (!isLive)
        //{
        //    return;
        //}
        //if (coolTime > 0.0f)
        //{
        //    return;
        //}
        ////StartAnimation(AnimIndex.stop);
        //coolTime = MAX_COOLTIME;
        //target.y = this.transform.position.y;
        //Vector3 dir = Vector3.Normalize(target - this.transform.position);
        //Vector3 pos = this.transform.position + (dir * 2);
        ////BulletManager.Instance.ShootBullet(pos, dir);
    }

    public bool GetIsLive()
    {
        return isLive;
    }

    public void Damaged()
    {
        hp -= 1;
    }

    public void SetHP(int hp)
    {
        this.hp = hp;
    }

    public void PlayerDie()
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
            // 매칭 인스턴스가 존재하지 않는 경우 (인게임 테스트 용도)
 //           Vector3 tmp = new Vector3(TESTONLY_vertualStick.GetHorizontalValue(), 0, TESTONLY_vertualStick.GetVerticalValue());
 //           tmp = Vector3.Normalize(tmp);
 //           SetMoveVector(tmp);
            //Move();


 //           if (TESTONLY_attackStick.isInputEnable)
 //           {
 //               Vector3 tmp2 = new Vector3(TESTONLY_attackStick.GetHorizontalValue(), 0, TESTONLY_attackStick.GetVerticalValue());
 //               if (!tmp2.Equals(Vector3.zero))
 //               {
 //                   tmp2 += GetPosition();
 //                  Attack(tmp2);
 //               }
 //           }
        }

        if (!isLive)
        {
            return;
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
}