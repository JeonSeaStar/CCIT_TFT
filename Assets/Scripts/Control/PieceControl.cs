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

                objPos.y = 0;
                //objPos.z = 0;
                //objPos.x = 0;
                transform.position = objPos;
            }
        }
        else if(FieldManager.instance.roundType == FieldManager.RoundType.READY)
        {
            float distance = Camera.main.WorldToScreenPoint(transform.position).z;
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
            Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);

            objPos.y = 0;
            //objPos.z = 0;
            //objPos.x = 0;
            transform.position = objPos;
        }

    }

    private void OnMouseUp()
    {
        FieldManager.instance.ActiveHexaIndicators(false);

        if(currentTile == targetTile)
        {
            transform.position = new Vector3(currentTile.transform.position.x, 0, currentTile.transform.position.z);
        }
        else if(currentTile != targetTile)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit , Mathf.Infinity, (-1) - (1 << 6)))
            {
                if (hit.transform.gameObject.layer == 8)
                {
                    transform.position = new Vector3(currentTile.transform.position.x, 0, currentTile.transform.position.z);
                    targetTile = null;
                    return;
                }
            }
            if (targetTile.isFull == true) // 이미 해당 타일에 기물이 존재하는 경우
            {
                var targetTileControl = targetTile.piece.GetComponent<PieceControl>();
                currentTile.piece = targetTile.piece;
                targetTileControl.currentTile = null; targetTileControl.targetTile = null;

                var targetPosition = targetTile.piece.transform.position;
                targetTile.piece.transform.position = new Vector3(currentTile.transform.position.x, 0, currentTile.transform.position.z);
                transform.position = targetPosition;

                currentTile = targetTile;

                currentTile.piece = this.gameObject;

                ControlPiece.currentNode = targetTile.GetComponent<Tile>();
                targetTile.piece.GetComponent<Piece>().currentNode = currentTile.GetComponent<Tile>();
            }
            else if (targetTile.isFull == false) // 해당 타일에 기물이 없는 경우 
            {
                currentTile.isFull = false; targetTile.isFull = true;
                currentTile.piece = null; targetTile.piece = this.gameObject;

                transform.position = new Vector3(targetTile.transform.position.x, 0, targetTile.transform.position.z);
                currentTile = targetTile;
                Debug.Log(currentTile.name);

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
                if (currentTile == null) currentTile = other.gameObject.GetComponent<Tile>();
                targetTile = other.gameObject.GetComponent<Tile>();
                
            }
        }
        if(FieldManager.instance.roundType == FieldManager.RoundType.BATTLE)
        {
            if (other.gameObject.layer == 7)
            {
                var tileComponent = other.gameObject.GetComponent<Tile>().isReadyTile;
                if(tileComponent == true)
                {
                    targetTile  = other.gameObject.GetComponent<Tile>();
                }

            }
        }
    }
}
