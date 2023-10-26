using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Protocol;
using BackEnd.Tcp;
using BackEnd;
using TMPro;
using DG.Tweening;

public class Player : MonoBehaviour
{
    public List<BuffData> buffDatas = new List<BuffData>();
    public int level;
    public int experience;
    [Range(-30, 200)] public int lifePoint = 100;
    public int gold = 0;
    [SerializeField] LayerMask playerMask; //9 - Player
    [SerializeField] LayerMask groundMask; //8 - Plane
    [SerializeField] FieldManager fieldManager;
    [SerializeField] Ease ease;
    [SerializeField] float moveSpeed;
    [SerializeField] bool owned;
    public bool isGrab = false;
    public bool isDrag = false;

    [SerializeField] int[] damagePerPiece = new int[] { 2, 4, 6, 8, 10, 11, 12, 13, 14, 15 };
    [SerializeField] int[] damagePerRound = new int[] { 0, 1, 2, 3, 4, 6, 9, 15 };

    [SerializeField] Piece controlPiece;

    #region 서버 머지
    //serverMerge

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

    private float LegacyrotSpeed = 4.0f;
    private float LegacymoveSpeed = 4.0f;

    private GameObject playerModelObject;
    private Rigidbody rigidBody;
    #endregion

    public Piece ControlPiece
    {
        set { controlPiece = value; }
        get
        {
            if (controlPiece == null)
                controlPiece = GetComponent<Piece>();
            return controlPiece;
        }
    }

    [SerializeField] Equipment controlEquipment;
    public Equipment ControlEquipment
    {
        set { controlEquipment = value; }
        get
        {
            if (controlEquipment == null)
                controlEquipment = GetComponent<Equipment>();
            return controlEquipment;
        }
    }

    [SerializeField] GameObject chargingParticle;
    [SerializeField] GameObject AttackParticle;

    public MatchingInformation matchingInformation;

    private void Awake()
    {
        //fieldManager.DualPlayers[0] = this;
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

        if (ArenaManager.Instance.roundType == ArenaManager.RoundType.Deployment || ArenaManager.Instance.roundType == ArenaManager.RoundType.Battle)
        {
            if (Input.GetMouseButtonDown(0) && owned) Targeting();
            if (Input.GetMouseButton(0) && isGrab && !isDrag && owned) Dragging();
            if (Input.GetMouseButtonUp(0) && isGrab && owned) EndDrag();
        }

        //if (Input.GetMouseButton(0) && owned && !isGrab)
        //    Aim();
    }

    #region SelectPiece
    void Targeting()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, (-1) - (1 << playerMask)))
        {
            #region Piece
            bool _isGrapPiece = hit.transform.gameObject.layer == 6;//Piece
            if (_isGrapPiece)
            {
                controlPiece = hit.transform.gameObject.GetComponent<Piece>();
                FreezeRigidbody(controlPiece, _isGrapPiece);
                isGrab = _isGrapPiece;
                fieldManager.ActiveHexaIndicators(_isGrapPiece);
                return;
            }
            #endregion
            # region Equipment
            bool _isGrapEquipment = hit.transform.gameObject.layer == 10;
            if (_isGrapEquipment)
            {
                controlEquipment = hit.transform.gameObject.GetComponent<Equipment>();
                isGrab = _isGrapEquipment;
                return;
            }
            #endregion
        }
    }

    void Dragging()
    {
        GameObject _controlObject = (controlPiece != null) ? controlPiece.gameObject : controlEquipment.gameObject;
        float _distance = Camera.main.WorldToScreenPoint(_controlObject.transform.position).z;
        Vector3 _mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _distance);
        Vector3 _objPos = Camera.main.ScreenToWorldPoint(_mousePos);
        _objPos.y = 0;

        #region Piece
        if (controlPiece == null) controlPiece = _controlObject.GetComponent<Piece>();
        else if (controlPiece != null)
        {
            if (ArenaManager.Instance.roundType == ArenaManager.RoundType.Battle && controlPiece.currentTile.isReadyTile)
            {
                controlPiece.transform.gameObject.transform.position = _objPos; return;
            }
            else controlPiece.transform.position = _objPos; return;
        }
        #endregion
        #region
        if (controlEquipment == null) controlEquipment = _controlObject.GetComponent<Equipment>();
        else if (controlEquipment != null)
        {
            controlEquipment.transform.position = _objPos;
        }
        #endregion
    }

    void EndDrag()
    {
        if (isDrag) { ResetDragState(!isDrag); return; }
        object _controlObject = (controlPiece != null) ? controlPiece : controlEquipment;
        #region Piece
        controlPiece = _controlObject as Piece;
        if (controlPiece != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, (-1) - (1 << 6)) && hit.transform.gameObject.layer == 7)
            {
                var _currentRound = ArenaManager.Instance.roundType;
                Tile _currentTileInformation = controlPiece.currentTile;
                Tile _targetTileInformation = hit.transform.gameObject.GetComponent<Tile>();
                controlPiece.targetTile = _targetTileInformation;
                if (_currentRound == ArenaManager.RoundType.Battle && controlPiece.currentTile.isReadyTile == false) ResetPositionToCurrentTile(controlPiece);
                else
                {
                    if (_currentTileInformation != _targetTileInformation)
                    {
                        if (_targetTileInformation.IsFull == false)
                        {
                            controlPiece.SetPiece(controlPiece);
                            ChangeTileTransform(controlPiece, controlPiece.targetTile);
                            controlPiece.currentTile = controlPiece.targetTile;
                            //Piece
                            _currentTileInformation.piece = null;
                            _currentTileInformation.IsFull = false;
                            _targetTileInformation.piece = controlPiece.gameObject;
                            _targetTileInformation.IsFull = true;
                        }
                        else
                        {
                            controlPiece.SetPiece(controlPiece, controlPiece.targetTile.piece);
                            var _targetPieceInformation = _targetTileInformation.piece.GetComponent<Piece>();
                            ChangeTileTransform(controlPiece, controlPiece.targetTile);
                            ChangeTileTransform(_targetPieceInformation, controlPiece.currentTile);
                            //Piece
                            _currentTileInformation.piece = _targetTileInformation.piece;
                            _targetTileInformation.piece = controlPiece.gameObject;
                            //Tile
                            _targetPieceInformation.currentTile = controlPiece.currentTile;
                            _targetPieceInformation.targetTile = controlPiece.currentTile;
                            controlPiece.currentTile = controlPiece.targetTile;
                        }
                    }
                    ResetDragState(false);
                    fieldManager.ActiveHexaIndicators(false);
                    return;
                }
            }
            ResetPositionToCurrentTile(controlPiece);
            return;
        }
        #endregion
        #region Equipment
        controlEquipment = _controlObject as Equipment;
        if (controlEquipment != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, (-1) - (1 << 10)))
            {
                if (hit.transform.gameObject.layer == 6)
                {
                    //
                    if (controlEquipment.targetPiece != null)
                    {
                        ArenaManager.Instance.fieldManagers[0].chest.AddEquipment(controlEquipment.targetPiece, controlEquipment);
                        print(111);
                    }
                }
            }
            return;
        }
        #endregion
    }

    void ResetDragState(bool isDragState)
    {
        isDrag = isDragState;
        isGrab = isDragState;
        controlPiece = null;
        ControlPiece = null;
        controlEquipment = null;
        ControlEquipment = null;
    }

    void FreezeRigidbody(Piece piece, bool isFreeze)
    {
        if (isFreeze) { piece.transform.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation; }
        else if (!isFreeze) piece.transform.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    private void ResetPositionToCurrentTile(Piece piece)
    {
        controlPiece.transform.position = new Vector3(piece.currentTile.transform.position.x, 0, piece.currentTile.transform.position.z);
    }

    void ChangeTileTransform(Piece piece, Tile target)
    {
        piece.transform.position = new Vector3(target.transform.position.x, 0, target.transform.position.z);
    }
    #endregion

    //이동 좌표
    (bool success, Vector3 position) GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, groundMask))
        {
            Vector3 targetPosition = new Vector3(hitInfo.point.x, transform.position.y, hitInfo.point.z);
            return (success: true, position: targetPosition);
        }
        else
            return (success: false, position: Vector3.zero);
    }

    //
    void Aim()
    {
        var (success, position) = GetMousePosition();
        if (success)
        {
            var direction = position - transform.position;

            transform.forward = direction;

            if (!fieldManager.grab)
            {
                DOTween.Kill(transform);
                transform.DOMove(position, moveSpeed * Vector3.Distance(transform.position, position)).SetEase(ease);
                transform.Rotate(90, transform.rotation.y, transform.rotation.z);
            }
        }
    }

    int MessengerDamage()
    {
        int activePiece = 0;
        foreach (var piece in ArenaManager.Instance.fieldManagers[0].myFilePieceList)
        {
            if (!piece.dead)
                activePiece++;
        }

        int damage = damagePerPiece[activePiece] + damagePerRound[ArenaManager.Instance.currentRound];

        return damage;
    }

    void spawnChargingParticle()
    {
        foreach (var piece in ArenaManager.Instance.fieldManagers[0].myFilePieceList)
        {
            if (!piece.dead)
            {

            }
        }
    }

    #region 서버 머지
    public void Initialize(bool isMe, SessionId index, string nickName, float rot, FieldManager fieldManage)
    {
        this.isMe = isMe;
        this.index = index;
        this.nickName = nickName;
        this.fieldManager = fieldManage;

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
        var pos = gameObject.transform.position + playerModelObject.transform.forward * LegacymoveSpeed * Time.deltaTime;
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
        playerModelObject.transform.rotation = Quaternion.Lerp(playerModelObject.transform.rotation, Quaternion.LookRotation(moveVector), Time.deltaTime * LegacyrotSpeed);
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

    public GameObject piecePrefeb;
    public void BuyPiece(Vector3 firstPos)
    {
        if (!isLive)
            return;
        firstPos = Vector3.zero;
        Instantiate(piecePrefeb, firstPos, Quaternion.identity);
    }

    public void SellPiece()
    {
        //todo..
    }

    public void PieceReroll()
    {
        //todo..
    }

    public void StoreLock()
    {
        //todo..
    }

    public void ButtonLevelUp()
    {
        //todo..
    }

    #endregion
}