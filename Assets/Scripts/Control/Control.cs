using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Control : MonoBehaviour
{
    [SerializeField]
    private Rigidbody pieceRigidbody;
    public Tile currentTile,targetTile;
   
    
    // 나중에 Piece로 붙일 예정

    private void OnMouseDrag()
    {
        if (FieldManager.instance.roundType == FieldManager.RoundType.WAITING
            || FieldManager.instance.roundType == FieldManager.RoundType.BATTLE)
        {
            float distance = Camera.main.WorldToScreenPoint(transform.position).z;
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
            Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);

            objPos.y = 0;
            //objPos.z = 0;
            //objPos.x = 0;
            if(FieldManager.instance.roundType == FieldManager.RoundType.BATTLE
                && currentTile.isWaitingZone == true)
            transform.position = objPos;

            if (pieceRigidbody != null)
            {
                pieceRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }
    }

    private void OnMouseDown()
    {
        
    }

    private void OnMouseUp()
    {
        if (FieldManager.instance.isBattle == false)
        {
            if (currentTile == targetTile) // No Translation
            {
                transform.position = new Vector3(
                    currentTile.transform.position.x,
                    0,
                    currentTile.transform.position.z);
                //
                currentTile = targetTile; targetTile = null;
            }
            
            if(targetTile != null)
            {
                if(targetTile.isFull == true) // piece is there
                {
                    transform.position = targetTile.piece.transform.position;

                    targetTile.piece.transform.position = new Vector3(
                        currentTile.transform.position.x,
                        0,
                        currentTile.transform.position.z);

                    currentTile.piece = targetTile.piece;
                    targetTile.piece = this.gameObject;
                    currentTile = targetTile; targetTile = null;
                }
                else if(targetTile.isFull == false)
                {
                    transform.position = new Vector3(
                        targetTile.transform.position.x,
                        0,
                        targetTile.transform.position.z);

                    currentTile.isFull = false; 
                    currentTile.piece = null;

                    targetTile.isFull = true; 
                    targetTile.piece = this.gameObject;

                    currentTile = targetTile; targetTile = null;
                }
            }
            


            if (pieceRigidbody != null)
            {
                pieceRigidbody.constraints = RigidbodyConstraints.None;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (FieldManager.instance.isBattle == false)
        {
            if (other.gameObject.layer == 7) // Tile Collider
            {
                if(currentTile == null)
                {
                    currentTile = other.gameObject.GetComponent<Tile>();
                }
                targetTile = other.gameObject.GetComponent<Tile>();
            }
        }
    }
}
