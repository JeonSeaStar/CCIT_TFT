using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PieceControl : MonoBehaviour
{
    [SerializeField]
    private Rigidbody pieceRigidbody;
    public Tile currentTile, targetTile;
    Piece ControlPiece
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

    private void OnMouseDown()
    {
        FieldManager.instance.ActiveHexaIndicators(true);

        if (pieceRigidbody != null) pieceRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void OnMouseDrag()
    {
        if(FieldManager.instance.roundType == FieldManager.RoundType.BATTLE)
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
        else if(FieldManager.instance.roundType == FieldManager.RoundType.READY)
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
        FieldManager.instance.ActiveHexaIndicators(false);

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
        // 기물 제외하고 레이 검출

        if (currentTile == targetTile)
        {
            transform.position = new Vector3(currentTile.transform.position.x, 0, currentTile.transform.position.z);
        }
        else if(currentTile != targetTile)
        {
            if (targetTile.isFull == true) // 이미 해당 타일에 기물이 존재하는 경우
            {
                var targetTileControl = targetTile.piece.GetComponent<PieceControl>();

                // Piece 정보 교환
                currentTile.piece = targetTile.piece;
                targetTile.piece = this.gameObject;
                // Piece 위치 교환
                targetTileControl.transform.position = new Vector3(currentTile.transform.position.x, 0, currentTile.transform.position.z);
                transform.position = new Vector3(targetTile.transform.position.x, 0, targetTile.transform.position.z);
                // 현재, 이동 타일 교환
                this.currentTile = targetTile;
                targetTileControl.currentTile = targetTileControl.targetTile;
                // 
                ControlPiece.currentNode = targetTile.GetComponent<Tile>();
                targetTile.piece.GetComponent<Piece>().currentNode = currentTile.GetComponent<Tile>();
            }
            else if (targetTile.isFull == false) // 해당 타일에 기물이 없는 경우 
            {
                currentTile.isFull = false;
                targetTile.isFull = true;
                currentTile.piece = null;
                targetTile.piece = this.gameObject;

                transform.position = new Vector3(targetTile.transform.position.x, 0, targetTile.transform.position.z);
                currentTile = targetTile;

                ControlPiece.currentNode = targetTile.GetComponent<Tile>();
            }
        }

        if (pieceRigidbody != null) pieceRigidbody.constraints = RigidbodyConstraints.None;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (FieldManager.instance.roundType == FieldManager.RoundType.READY)
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
        if(FieldManager.instance.roundType == FieldManager.RoundType.BATTLE)
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
