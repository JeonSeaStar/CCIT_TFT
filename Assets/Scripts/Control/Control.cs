using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Control : MonoBehaviour
{
    [SerializeField]
    private Rigidbody pieceRigidbody;

    private Vector3 originTilePosition = Vector3.zero;
    private Vector3 targetTilePosition = Vector3.zero;

    

    


    // 나중에 Piece로 붙일 예정

    private void OnMouseEnter()
    {
        
    }
    
    private void OnMouseDrag()
    {
        float distance = Camera.main.WorldToScreenPoint(transform.position).z;

        Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
        Vector3 objPos = Camera.main.ScreenToWorldPoint(mousePos);

        objPos.y = 0;
        //objPos.z = 0;
        //objPos.x = 0;
        transform.position = objPos;

        if (pieceRigidbody != null)
        {
            pieceRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    private void OnMouseUp()
    {
        if (pieceRigidbody != null)
        {
            pieceRigidbody.constraints = RigidbodyConstraints.None;
        }
    }

    private void OnMouseDown()
    {
        
    }

    private void OnMouseExit()
    {
        
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == 7)
        {
            Debug.Log(25);
        }
    }

}
