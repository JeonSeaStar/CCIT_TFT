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
    [SerializeField] private FieldManager fm;
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
        if (fm == null) fm = ArenaManager.instance.fm[0];
    }



    private void OnMouseDown()
    {
        fm.ActiveHexaIndicators(true);
        if (ArenaManager.instance.roundType == RoundType.Ready) return;
        fm.grab = true;
        fm.controlPiece = this.ControlPiece;

        if (pieceRigidbody != null) pieceRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void OnMouseDrag()
    {
        if(ArenaManager.instance.roundType == RoundType.Battle)
        {
            if(currentTile.isReadyTile == true && !fm.isDrag)
            {
                float distance = Camera.main.WorldToScreenPoint(transform.position).z;
                Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
                Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);

                objPos.y = 0; //objPos.z = 0; //objPos.x = 0;
                transform.position = objPos;
            }
        }
        else if(ArenaManager.instance.roundType == RoundType.Deployment)
        {
            float distance = Camera.main.WorldToScreenPoint(transform.position).z;
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
            Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);

            objPos.y = 0; //objPos.z = 0; //objPos.x = 0;
            transform.position = objPos;
        }
        else if(ArenaManager.instance.roundType == RoundType.Ready)
        {
            return;
        }
    }

    private void OnMouseUp()
    {
        fm.ActiveHexaIndicators(false);

        if (fm.isDrag) //라운드 변경시 드래그 중이던 기물이 있었는지 확인
        {
            fm.isDrag = false;
            fm.grab = false;
            fm.controlPiece = null;
            return;
        }
        var currentRound = ArenaManager.instance.roundType;
        if(currentRound != RoundType.Deployment && currentRound != RoundType.Battle) return;

        //if (ArenaManager.instance.roundType == RoundType.Battle && !currentTile.isReadyTile) return; //전투 라운드에 전투 기물 이동 금지

        // Plane 위로 드래그 한 경우
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, (-1) - (1 << 6)))
        {
            if(hit.transform.gameObject.layer == 8) //타일 외 이동 금지 ex)Plane
            {
                targetTile = null;
                transform.position = new Vector3(currentTile.transform.position.x, 0, currentTile.transform.position.z);
                return;
            }
            if (currentRound == RoundType.Battle && hit.transform.gameObject.GetComponent<Tile>().isReadyTile == false) //전투 라운드에 기물 배치 금지
            {
                transform.position = new Vector3(currentTile.transform.position.x, 0, currentTile.transform.position.z);
                return;
            }
            targetTile = hit.transform.gameObject.GetComponent<Tile>();
        }
        else return;

        if (currentTile == targetTile)
        {
            transform.position = new Vector3(currentTile.transform.position.x, 0, currentTile.transform.position.z);
        }
        else if (currentTile != targetTile)
        {
            // 이미 해당 타일에 기물이 존재하는 경우
            if (targetTile.isFull == true) 
            {
                // 시너지 계산
                ControlPiece.SetPiece(this.controlPiece, targetTile.piece.GetComponent<Piece>());
                
                // 기물 위치 이동
                ChangeTileTransform(currentTile, targetTile);
                
                // Piece Tile 정보 교환
                ChangeTileInfo(currentTile, targetTile, targetTile.isFull);
              
                ControlPiece.currentNode = targetTile.GetComponent<Tile>();
                targetTile.piece.GetComponent<Piece>().currentNode = currentTile.GetComponent<Tile>();
            }

            // 해당 타일에 기물이 없는 경우
            else if (targetTile.isFull == false) 
            {
                // 시너지 계산
                ControlPiece.SetPiece(this.ControlPiece);

                // Piece Tile 정보 교환
                ChangeTileInfo(currentTile, targetTile, targetTile.isFull);

                // 기물 위치 이동
                ChangeTileTransform(targetTile);
                
                // 현재, 이동 타일 교환
                currentTile = targetTile;
                
                ControlPiece.currentNode = targetTile.GetComponent<Tile>();
            }
        }
        if (pieceRigidbody != null) pieceRigidbody.constraints = RigidbodyConstraints.None;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (ArenaManager.instance.roundType == RoundType.Deployment)
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
        if(ArenaManager.instance.roundType == RoundType.Battle)
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

    /// <summary>
    /// 기물 위치 이동
    /// </summary>
    /// <param name="target"></param>
    void ChangeTileTransform(Tile target) => transform.position = new Vector3(target.transform.position.x, 0, target.transform.position.z);

    /// <summary>
    /// 기물 위치 변경
    /// </summary>
    /// <param name="current"></param>
    /// <param name="target"></param>
    void ChangeTileTransform(Tile current, Tile target)
    {
        target.piece.transform.position = new Vector3(current.transform.position.x, 0, current.transform.position.z);
        current.piece.transform.position = new Vector3(target.transform.position.x, 0, target.transform.position.z);
    }

    /// <summary>
    /// 타일 정보 변경
    /// </summary>
    /// <param name="current"></param>
    /// <param name="target"></param>
    void ChangeTileInfo(Tile current, Tile target, bool isfull)
    {
        if (isfull)
        {
            // 현재 타일의 기물 바꿔주기
            current.piece = target.piece;

            // 목표 타일의 (현재, 목표) 타일 정보 바꿔주기
            var targetPieceControl = target.piece.GetComponent<PieceControl>();
            targetPieceControl.currentTile = current;
            targetPieceControl.targetTile = current;
            target.piece = this.gameObject;

            // 현재 기물의 현재 타일 정보 변경
            this.currentTile = targetTile;
        }
        else if(!isfull)
        {
            current.isFull = false; 
            target.isFull = true;
            currentTile.piece = null; 
            targetTile.piece = this.gameObject;
        }
    }
}
