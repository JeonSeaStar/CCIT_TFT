using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;
using DG.Tweening;
using TMPro;

public class Messenger : MonoBehaviour
{
    public PieceInformation pieceInformationUI;
    public List<BuffData> buffDatas = new List<BuffData>();
    public int level;
    public int currentXP;
    public int[] maxXP;
    public int[] levelUpCost;
    public int[] maxPieceCount;
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
    public bool isExpedition;

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

    [SerializeField] Tile controlTile;
    public Tile ControlTile
    {
        get { return controlTile; }
        set
        {
            if (controlTile == value) return;

            controlTile = value;
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

    [SerializeField] private GameObject targetPiece;
    private Ray ray;
    private RaycastHit hit;
    [SerializeField] GameObject chargingParticle;
    [SerializeField] GameObject AttackParticle;

    public MatchingInformation matchingInformation;

    [SerializeField] GraphicRaycaster graphicRaycaster;
    [SerializeField] GameObject pieceSaleSlot;
    [SerializeField] TextMeshProUGUI pieceSaleGoldText;
    PointerEventData pointerEventData;

    public GameObject behindSaleZone;

    private void Awake()
    {
        SoundManager.instance.Clear();
    }

    private void Start()
    {
        SoundManager.instance.Play("BGM/Bgm_Battle_Default", SoundManager.Sound.Bgm);
    } 

    void Update()
    {
        if (FieldManager.Instance.roundType == FieldManager.RoundType.Deployment || FieldManager.Instance.roundType == FieldManager.RoundType.Battle)
        {
            if (Input.GetMouseButtonDown(0))
            {
                pieceInformationUI.ClosePieceInformation();
                if (owned)
                    Targeting();
            }
            if (Input.GetMouseButton(0) && isGrab && !isDrag && owned) Dragging();
            if (Input.GetMouseButtonUp(0))
            {
                StopAllCoroutines();
                if (timer <= grabTime)
                {
                    if (targetPiece.GetComponent<Piece>() != null)
                        pieceInformationUI.OpenPieceInformation(targetPiece.GetComponent<Piece>());
                }
                timer = 0;

                if (isGrab && owned)
                    EndDrag();
            }
        }

        if (Input.GetMouseButton(0) && owned && !isGrab)
            Aim();
    }

    [SerializeField] private float timer = 0;
    [SerializeField] private float grabTime;
    bool timeOver;
    private IEnumerator ClickTimer()
    {
        yield return new WaitForSeconds(0.01f);
        timer += 0.01f;

        if (timer > grabTime && controlPiece == null)
        {
            Targeting(ray, hit);
            StopAllCoroutines();
        }
        else
            StartCoroutine(ClickTimer());
    }

    #region SelectPiece
    void Targeting()
    {
        StartCoroutine(ClickTimer());

        ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, (-1) - (1 << playerMask)))
        {
            targetPiece = hit.transform.gameObject;
        }
    }

    public List<AudioClip> kk;

    void Targeting(Ray ray, RaycastHit hit)
    {
        #region Piece
        bool _isGrapPiece = targetPiece.layer == 6;//Piece
        if (_isGrapPiece && targetPiece.GetComponent<Piece>().isOwned == true)
        {
            controlPiece = hit.transform.gameObject.GetComponent<Piece>();
            if (FieldManager.Instance.roundType == FieldManager.RoundType.Battle && controlPiece.currentTile.isReadyTile == false) return;
            pieceSaleGoldText.text = controlPiece.pieceData.cost[controlPiece.pieceData.grade, controlPiece.star].ToString();
            FreezeRigidbody(controlPiece, _isGrapPiece);
            isGrab = _isGrapPiece;
            fieldManager.ActiveHexaIndicators(_isGrapPiece);
            behindSaleZone.SetActive(false);
            pieceSaleSlot.SetActive(true);

            controlPiece.gameObject.AddComponent<AudioSource>();
            int i = Random.Range(0, kk.Count);
            controlPiece.gameObject.GetComponent<AudioSource>().clip = kk[i];
            controlPiece.gameObject.GetComponent<AudioSource>().Play();
            return;
        }
        #endregion
        #region Equipment
        bool _isGrapEquipment = hit.transform.gameObject.layer == 10;
        if (_isGrapEquipment)
        {
            controlEquipment = hit.transform.gameObject.GetComponent<Equipment>();
            isGrab = _isGrapEquipment;
            return;
        }
        #endregion
    }

    void Dragging()
    {
        GameObject _controlObject = (controlPiece != null) ? controlPiece.gameObject : controlEquipment.gameObject;
        float _distance = Camera.main.WorldToScreenPoint(_controlObject.transform.position).z;
        Vector3 _mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _distance);
        Vector3 _objPos = Camera.main.ScreenToWorldPoint(_mousePos);
        _objPos.x += 1f;
        _objPos.y = 1.5f;
        _objPos.z += 1f; 

        #region Piece
        if (controlPiece == null) controlPiece = _controlObject.GetComponent<Piece>();
        else if (controlPiece != null)
        {
            controlPiece.transform.position = _objPos; 
            
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, (-1) - (1 << 6)) && hit.transform.gameObject.layer == 7)
            {
                if (hit.transform.gameObject.GetComponent<Tile>().myTile && ControlTile != hit.transform.GetComponent<Tile>()) ControlTile = hit.transform.GetComponent<Tile>();
            }
            return;
            //if (FieldManager.Instance.roundType == FieldManager.RoundType.Battle && controlPiece.currentTile.isReadyTile)
            //{
            //    controlPiece.transform.gameObject.transform.position = _objPos; return;
            //}
            //else controlPiece.transform.position = _objPos; return;
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
            #region 기물 판매
            if (Physics.Raycast(ray, out RaycastHit saleSlot, Mathf.Infinity, (-1) - (1 << 6)))
            {
                pointerEventData = new PointerEventData(null);
                pointerEventData.position = Input.mousePosition;
                List<RaycastResult> _results = new List<RaycastResult>();
                graphicRaycaster.Raycast(pointerEventData, _results);
                if (_results.Count > 0 && _results[0].gameObject.name == "PieceSaleSlot")
                {
                    if (fieldManager.myFilePieceList.Contains(controlPiece))
                    {
                        fieldManager.myFilePieceList.Remove(controlPiece);
                        fieldManager.RemoveDPList(controlPiece);
                        var _duplicationCheck = fieldManager.myFilePieceList.FirstOrDefault(listPiece => listPiece.pieceName == controlPiece.pieceName);
                        if (_duplicationCheck == null) fieldManager.SynergeDecrease(controlPiece);
                        fieldManager.CalSynerge(controlPiece);
                    }
                    Tile _currentTile = controlPiece.currentTile;
                    _currentTile.IsFull = false;
                    _currentTile.piece = null;
                    isGrab = false; 
                    SoundManager.instance.Play("UI/Eff_Gold_Pos", SoundManager.Sound.Effect);
                    //gold += controlPiece.pieceData.cost[controlPiece.pieceData.grade, controlPiece.star];
                    fieldManager.ChargeGold(controlPiece.pieceData.cost[controlPiece.pieceData.grade, controlPiece.star]);
                    Destroy(controlPiece.gameObject);
                    behindSaleZone.SetActive(true);
                    pieceSaleSlot.SetActive(false);
                    fieldManager.ActiveHexaIndicators(false);
                    return;
                }
            }
            #endregion
            #region 기물 배치
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, (-1) - (1 << 6)) && hit.transform.gameObject.layer == 7 && hit.transform.gameObject.GetComponent<Tile>().myTile)
            {
                var _currentRound = FieldManager.Instance.roundType;
                Tile _currentTileInformation = controlPiece.currentTile;
                Tile _targetTileInformation = hit.transform.gameObject.GetComponent<Tile>();
                controlPiece.targetTile = _targetTileInformation;
                if (_currentRound == FieldManager.RoundType.Battle && controlPiece.currentTile.isReadyTile == false) ResetPositionToCurrentTile(controlPiece);
                else
                {
                    if (_currentTileInformation != _targetTileInformation)
                    {
                        if (_targetTileInformation.IsFull == false)
                        {
                            if (!_targetTileInformation.isReadyTile && fieldManager.myFilePieceList.Count >= maxPieceCount[level])
                            {
                                if(_currentTileInformation.isReadyTile)
                                {
                                    fieldManager.fieldPieceStatus.UpdateFieldStatus(fieldManager.myFilePieceList.Count, fieldManager.owerPlayer.maxPieceCount[fieldManager.owerPlayer.level]);
                                    ResetPositionToCurrentTile(controlPiece);
                                    behindSaleZone.SetActive(true);
                                    pieceSaleSlot.SetActive(false);
                                    return;
                                }
                            }

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
                        if (!controlPiece.currentTile.isReadyTile) TileManager.Instance.ActiveSetEffect(controlPiece.currentTile.gameObject);
                    }
                    else if (_currentTileInformation == _targetTileInformation) ChangeTileTransform(controlPiece, controlPiece.targetTile);
                    ResetDragState(false);
                    fieldManager.ActiveHexaIndicators(false);
                    behindSaleZone.SetActive(true);
                    pieceSaleSlot.SetActive(false);

                    fieldManager.fieldPieceStatus.UpdateFieldStatus(fieldManager.myFilePieceList.Count, fieldManager.owerPlayer.maxPieceCount[fieldManager.owerPlayer.level]);
                    return;
                }
            }
            fieldManager.fieldPieceStatus.UpdateFieldStatus(fieldManager.myFilePieceList.Count, fieldManager.owerPlayer.maxPieceCount[fieldManager.owerPlayer.level]);
            ResetPositionToCurrentTile(controlPiece);
            fieldManager.ActiveHexaIndicators(false);
            behindSaleZone.SetActive(true);
            pieceSaleSlot.SetActive(false);
            ResetDragState(false);
            return;
            #endregion
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
                        FieldManager.Instance.chest.AddEquipment(controlEquipment.targetPiece, controlEquipment);
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
        controlPiece.transform.position = new Vector3(piece.currentTile.transform.position.x, fieldManager.groundHeight, piece.currentTile.transform.position.z);
    }

    void ChangeTileTransform(Piece piece, Tile target)
    {
        piece.transform.position = new Vector3(target.transform.position.x, fieldManager.groundHeight, target.transform.position.z);
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

    void spawnChargingParticle()
    {
        foreach (var piece in FieldManager.Instance.myFilePieceList)
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