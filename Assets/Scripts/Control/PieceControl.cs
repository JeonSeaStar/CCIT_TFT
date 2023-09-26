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

        if (fm.isDrag) //���� ����� �巡�� ���̴� �⹰�� �־����� Ȯ��
        {
            fm.isDrag = false;
            fm.grab = false;
            fm.controlPiece = null;
            return;
        }
        var currentRound = ArenaManager.instance.roundType;
        if(currentRound != RoundType.Deployment && currentRound != RoundType.Battle) return;

        //if (ArenaManager.instance.roundType == RoundType.Battle && !currentTile.isReadyTile) return; //���� ���忡 ���� �⹰ �̵� ����

        // Plane ���� �巡�� �� ���
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, (-1) - (1 << 6)))
        {
            if(hit.transform.gameObject.layer == 8) //Ÿ�� �� �̵� ���� ex)Plane
            {
                targetTile = null;
                transform.position = new Vector3(currentTile.transform.position.x, 0, currentTile.transform.position.z);
                return;
            }
            if (currentRound == RoundType.Battle && hit.transform.gameObject.GetComponent<Tile>().isReadyTile == false) //���� ���忡 �⹰ ��ġ ����
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
            // �̹� �ش� Ÿ�Ͽ� �⹰�� �����ϴ� ���
            if (targetTile.isFull == true) 
            {
                // �ó��� ���
                ControlPiece.SetPiece(this.controlPiece, targetTile.piece.GetComponent<Piece>());
                
                // �⹰ ��ġ �̵�
                ChangeTileTransform(currentTile, targetTile);
                
                // Piece Tile ���� ��ȯ
                ChangeTileInfo(currentTile, targetTile, targetTile.isFull);
              
                ControlPiece.currentNode = targetTile.GetComponent<Tile>();
                targetTile.piece.GetComponent<Piece>().currentNode = currentTile.GetComponent<Tile>();
            }

            // �ش� Ÿ�Ͽ� �⹰�� ���� ���
            else if (targetTile.isFull == false) 
            {
                // �ó��� ���
                ControlPiece.SetPiece(this.ControlPiece);

                // Piece Tile ���� ��ȯ
                ChangeTileInfo(currentTile, targetTile, targetTile.isFull);

                // �⹰ ��ġ �̵�
                ChangeTileTransform(targetTile);
                
                // ����, �̵� Ÿ�� ��ȯ
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
    /// �⹰ ��ġ �̵�
    /// </summary>
    /// <param name="target"></param>
    void ChangeTileTransform(Tile target) => transform.position = new Vector3(target.transform.position.x, 0, target.transform.position.z);

    /// <summary>
    /// �⹰ ��ġ ����
    /// </summary>
    /// <param name="current"></param>
    /// <param name="target"></param>
    void ChangeTileTransform(Tile current, Tile target)
    {
        target.piece.transform.position = new Vector3(current.transform.position.x, 0, current.transform.position.z);
        current.piece.transform.position = new Vector3(target.transform.position.x, 0, target.transform.position.z);
    }

    /// <summary>
    /// Ÿ�� ���� ����
    /// </summary>
    /// <param name="current"></param>
    /// <param name="target"></param>
    void ChangeTileInfo(Tile current, Tile target, bool isfull)
    {
        if (isfull)
        {
            // ���� Ÿ���� �⹰ �ٲ��ֱ�
            current.piece = target.piece;

            // ��ǥ Ÿ���� (����, ��ǥ) Ÿ�� ���� �ٲ��ֱ�
            var targetPieceControl = target.piece.GetComponent<PieceControl>();
            targetPieceControl.currentTile = current;
            targetPieceControl.targetTile = current;
            target.piece = this.gameObject;

            // ���� �⹰�� ���� Ÿ�� ���� ����
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
