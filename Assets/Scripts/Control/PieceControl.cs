using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PieceControl : MonoBehaviour
{
    [SerializeField]
    private Rigidbody pieceRigidbody;
    public Tile currentTile, targetTile;


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
            }
            else if(targetTile.isFull == false) // 해당 타일에 기물이 없는 경우 
            {
                currentTile.isFull = false; targetTile.isFull = true;
                currentTile.piece = null; targetTile.piece = this.gameObject;

                transform.position = new Vector3(targetTile.transform.position.x, 0, targetTile.transform.position.z);
                Debug.Log("index");
            }
            
            //transform.position = new Vector3(targetTile.transform.position.x, 0 , targetTile.transform.position.z); 
            
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
