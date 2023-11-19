using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Messenger : MonoBehaviour
{
    public List<BuffData> buffDatas = new List<BuffData>();
    public int level;
    public int currentXP;
    public int[] maxXP;
    [Range(-30,200)] public int lifePoint = 100;
    public int gold = 0;
    [SerializeField] LayerMask playerMask; //9 - Player
    [SerializeField] LayerMask groundMask; //8 - Plane
    [SerializeField] FieldManager fieldManager;
    [SerializeField] Ease ease;
    [SerializeField] float moveSpeed;
    [SerializeField] bool owned;
    public bool isGrab = false;
    public bool isDrag = false;
    public bool isExpedition;

    [SerializeField] int[] damagePerPiece = new int[] { 2, 4, 6, 8, 10, 11, 12, 13, 14, 15};
    [SerializeField] int[] damagePerRound = new int[] { 0, 1, 2, 3, 4, 6, 9, 15 };

    [SerializeField] Piece controlPiece;
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
        fieldManager.DualPlayers[0] = this;
    }

    void Update()
    {
        if (ArenaManager.Instance.roundType == ArenaManager.RoundType.Deployment || ArenaManager.Instance.roundType == ArenaManager.RoundType.Battle)
        {
            if (Input.GetMouseButtonDown(0) && owned) Targeting();
            if (Input.GetMouseButton(0) && isGrab && !isDrag &&owned) Dragging();
            if (Input.GetMouseButtonUp(0) && isGrab && owned) EndDrag();
        }

        if (Input.GetMouseButton(0) && owned && !isGrab)
            Aim();
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
        if (controlPiece == null) controlPiece = _controlObject.GetComponent< Piece >();
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
        if (controlEquipment == null) controlEquipment = _controlObject.GetComponent< Equipment >();
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
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, (-1) - (1 << 6)) && hit.transform.gameObject.layer == 7 && hit.transform.gameObject.GetComponent<Tile>().myTile)
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
                            _currentTileInformation.walkable = true;
                            _targetTileInformation.piece = controlPiece;
                            _targetTileInformation.IsFull = true;
                            _targetTileInformation.walkable = false;
                        }
                        else
                        {
                            controlPiece.SetPiece(controlPiece, controlPiece.targetTile.piece);
                            var _targetPieceInformation = _targetTileInformation.piece;
                            ChangeTileTransform(controlPiece, controlPiece.targetTile);
                            ChangeTileTransform(_targetPieceInformation, controlPiece.currentTile);
                            //Piece
                            _currentTileInformation.piece = _targetTileInformation.piece;
                            _targetTileInformation.piece = controlPiece;
                            //Tile
                            _targetPieceInformation.currentTile = controlPiece.currentTile;
                            _targetPieceInformation.targetTile = controlPiece.currentTile;
                            controlPiece.currentTile = controlPiece.targetTile;
                        }
                    }
                    else if (_currentTileInformation == _targetTileInformation) ChangeTileTransform(controlPiece, controlPiece.targetTile);
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

    void FreezeRigidbody(Piece piece,bool isFreeze)
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
        piece.transform.position = new Vector3(target.transform.position.x, -0.5f, target.transform.position.z);
    }
    #endregion

    //ÀÌµ¿ ÁÂÇ¥
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
        //var (success, position) = GetMousePosition();
        //if(success)
        //{
        //    var direction = position - transform.position;

        //    transform.forward = direction;

        //    if(!fieldManager.grab)
        //    {
        //        DOTween.Kill(transform);
        //        transform.DOMove(position, moveSpeed * Vector3.Distance(transform.position, position)).SetEase(ease);
        //        transform.Rotate(90, transform.rotation.y, transform.rotation.z);
        //    }
        //}
    }

    int MessengerDamage()
    {
        int activePiece = 0;
        foreach(var piece in ArenaManager.Instance.fieldManagers[0].myFilePieceList)
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
}

[System.Serializable]
public class MatchingInformation
{
    public int myIndex;
    public List<int> matchingHistroy = new List<int>();
    public void HistoryIniti()
    {
        matchingHistroy = new List<int>();
    }
    public bool pairings;
}