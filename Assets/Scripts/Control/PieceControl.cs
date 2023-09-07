using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static ArenaManager;

public class PieceControl : MonoBehaviour
{
    [SerializeField] public FieldManager fieldManager;
    [SerializeField] private Rigidbody pieceRigidbody;

    public Tile currentTile, targetTile;
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
    Piece controlPiece;

    private void Start()
    {
        if (fieldManager == null) fieldManager = ArenaManager.instance.fm[0];
    }



    private void OnMouseDown()
    {
        fieldManager.ActiveHexaIndicators(true);

        if (pieceRigidbody != null) pieceRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void OnMouseDrag()
    {
        if(ArenaManager.instance.roundType == RoundType.BATTLE)
        {
            if(currentTile.isReadyTile == true)
            {
                float distance = Camera.main.WorldToScreenPoint(transform.position).z;
                Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
                Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);

                objPos.y = 0; //objPos.z = 0; //objPos.x = 0;
                transform.position = objPos;
            }
        }
        else if(ArenaManager.instance.roundType == RoundType.READY)
        {
            float distance = Camera.main.WorldToScreenPoint(transform.position).z;
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
            Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);

            objPos.y = 0; //objPos.z = 0; //objPos.x = 0;
            transform.position = objPos;
        }
    }

    private void OnMouseUp()
    {
        fieldManager.ActiveHexaIndicators(false);

        #region Plane 위로 드래그 한 경우
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, (-1) - (1 << 6)))
        {
            if(hit.transform.gameObject.layer == 8) 
            {
                targetTile = null;
                transform.position = new Vector3(currentTile.transform.position.x, 0, currentTile.transform.position.z);
                return;
            }
            targetTile = hit.transform.gameObject.GetComponent<Tile>();
        }
        else return;
        #endregion

        #region 제자리 이동 
        if (currentTile == targetTile)
        {
            transform.position = new Vector3(currentTile.transform.position.x, 0, currentTile.transform.position.z);
        }
        #endregion
        #region 기물 위치 이동
        else if (currentTile != targetTile)
        {
            #region 이미 해당 타일에 기물이 존재하는 경우
            if (targetTile.isFull == true)
            {
                #region 시너지 계산
                ControlPiece.SetPiece(this.controlPiece, targetTile.piece.GetComponent<Piece>());
                #endregion

                #region 기물 위치 이동
                targetTile.piece.transform.position = new Vector3(currentTile.transform.position.x, 0, currentTile.transform.position.z);
                transform.position = new Vector3(targetTile.transform.position.x, 0, targetTile.transform.position.z);
                #endregion

                #region Piece Tile 정보 교환
                currentTile.piece = targetTile.piece;
                var targetPieceControl = targetTile.piece.GetComponent<PieceControl>();
                targetPieceControl.currentTile = currentTile; targetPieceControl.targetTile = currentTile;
                targetTile.piece = this.gameObject;
                #endregion

                #region 현재, 이동 타일 교환
                this.currentTile = targetTile;
                #endregion

                // JeonSeaStar
                ControlPiece.currentNode = targetTile.GetComponent<Tile>();
                targetTile.piece.GetComponent<Piece>().currentNode = currentTile.GetComponent<Tile>();
            }
            #endregion
            # region 해당 타일에 기물이 없는 경우
            else if (targetTile.isFull == false) // 해당 타일에 기물이 없는 경우
            {
                #region 시너지 계산
                ControlPiece.SetPiece(this.ControlPiece);
                #endregion

                #region Piece Tile 정보 교환
                currentTile.isFull = false; targetTile.isFull = true;
                currentTile.piece = null; targetTile.piece = this.gameObject;
                #endregion

                #region 기물 위치 이동
                transform.position = new Vector3(targetTile.transform.position.x, 0, targetTile.transform.position.z);
                #endregion

                #region 현재, 이동 타일 교환
                currentTile = targetTile;
                #endregion

                ControlPiece.currentNode = targetTile.GetComponent<Tile>();
            }
            #endregion
        }
        #endregion

        if (pieceRigidbody != null) pieceRigidbody.constraints = RigidbodyConstraints.None;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ArenaManager.instance.roundType == RoundType.READY)
        {
            if (other.gameObject.layer == 7)
            {
                if (currentTile == null)
                {
                    currentTile = other.gameObject.GetComponent<Tile>();
                    targetTile = currentTile;
                }
                //targetTile = other.gameObject.GetComponent<Tile>();
            }
        }
        if(ArenaManager.instance.roundType == RoundType.BATTLE)
        {
            if (other.gameObject.layer == 7)
            {
                var tileComponent = other.gameObject.GetComponent<Tile>().isReadyTile;
                if (tileComponent == true)
                {
                    targetTile = other.gameObject.GetComponent<Tile>();
                }
            }
        }
    }
}
