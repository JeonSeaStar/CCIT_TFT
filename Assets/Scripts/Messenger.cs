using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Messenger : MonoBehaviour
{
    public int level;
    public int experience;
    public int lifePoint = 100;
    public int gold = 0;
    [SerializeField] LayerMask playerMaks;
    [SerializeField] LayerMask groundMask;
    [SerializeField] FieldManager fieldManager;
    [SerializeField] Ease ease;
    [SerializeField] float moveSpeed;
    [SerializeField] bool owned;
    public bool isGrab = false;
    public bool isDrag = false;

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

    private void Awake()
    {
        fieldManager.DualPlayers[0] = this;
    }

    void Update()
    {
        if (ArenaManager.Instance.roundType == ArenaManager.RoundType.Deployment || ArenaManager.Instance.roundType == ArenaManager.RoundType.Battle)
        {
            if (Input.GetMouseButtonDown(0) && owned) Targeting();
            if (Input.GetMouseButton(0) && isGrab && owned) Dragging();
            if (Input.GetMouseButtonUp(0) && isGrab && owned) EndDrag();
        }

        if (Input.GetMouseButton(0) && owned && !isGrab)
            Aim();
    }
    
    void Targeting()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, (-1) - (1 << playerMaks)))
        {
            # region Equipment
            bool _isGrapEquipment = hit.transform.gameObject.layer == 10;
            if (_isGrapEquipment)
            {
                controlEquipment = hit.transform.gameObject.GetComponent<Equipment>();
                isGrab = _isGrapEquipment;
                return;
            }
            #endregion
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
        }
    }

    
    void Dragging()
    {
        GameObject _controlObject = (controlPiece != null) ? controlPiece.gameObject : controlEquipment.gameObject;
        float _distance = Camera.main.WorldToScreenPoint(_controlObject.transform.position).z;
        Vector3 _mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, _distance);
        Vector3 _objPos = Camera.main.ScreenToWorldPoint(_mousePos);
        _objPos.y = 0;

        
        if(controlPiece == null) controlPiece = _controlObject.GetComponent< Piece >();
        else if (controlPiece != null)
        {
            if (ArenaManager.Instance.roundType == ArenaManager.RoundType.Battle && controlPiece.currentTile.isReadyTile)
            {
                controlPiece.transform.gameObject.transform.position = _objPos; return;
            }
            else controlPiece.transform.position = _objPos; return;
        }

        if(controlEquipment == null) controlEquipment = _controlObject.GetComponent< Equipment >();
        else if (controlEquipment != null)
        {
            controlEquipment.transform.position = _objPos;
        }
    }
    
    void EndDrag()
    {
        if (isDrag) { ResetDragState(!isDrag); return; }

        object _controlObject = (controlPiece != null) ? controlPiece : controlEquipment;

        controlPiece = _controlObject as Piece;
        if (controlPiece != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, (-1) - (1 << 6)) && hit.transform.gameObject.layer == 7)
            {
                var _currentRound = ArenaManager.Instance.roundType;
                Tile _currentTileInfo = controlPiece.currentTile; 
                Tile _targetTileInfo = hit.transform.gameObject.GetComponent<Tile>();
                if (_currentRound == ArenaManager.RoundType.Battle && controlPiece.currentTile.isReadyTile == false) ResetPositionToCurrentTile(controlPiece);
                else
                {
                    if (_currentTileInfo != _targetTileInfo)
                    {
                        //if(_targetTileInfo.is)
                    }
                }
            }
            ResetPositionToCurrentTile(controlPiece);
            return;
        }

        controlEquipment = _controlObject as Equipment;
        if (controlEquipment != null)
        {

            return;
        }

        ResetDragState(false);
        fieldManager.ActiveHexaIndicators(false);
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
        if (isFreeze) { Debug.Log(piece.transform.gameObject.name); piece.transform.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation; }
        else if (!isFreeze) piece.transform.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
    }

    private void ResetPositionToCurrentTile(Piece piece)
    {
        transform.position = new Vector3(piece.currentTile.transform.position.x, 0, piece.currentTile.transform.position.z);
    }

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
        var (success, position) = GetMousePosition();
        if(success)
        {
            var direction = position - transform.position;

            transform.forward = direction;

            if(!fieldManager.grab)
            {
                DOTween.Kill(transform);
                transform.DOMove(position, moveSpeed * Vector3.Distance(transform.position, position)).SetEase(ease);
                transform.Rotate(90, transform.rotation.y, transform.rotation.z);
            }
        }
    }
}
