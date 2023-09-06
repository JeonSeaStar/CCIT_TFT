using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Piece;
using static PieceData;

public class Piece : MonoBehaviour
{
    public PieceData pieceData;

    public string pieceName;
    public Sprite piecePortrait;

    //public List<Synerge> synerges;
    public List<Equipment> Equipments;
    //public int pieceGrade = 1;
    public int star = 0;

    public float health;
    public float mana;
    public float attackDamage;
    public float abilityPower;
    public float armor;
    public float magicResist;
    public float attackSpeed;
    public float criticalChance;
    public float criticalDamage;
    public float attackRange;

    public string owner;
    public bool isOwned;
    // 여기다가 전투존에 배치 되었는지 확인하는거 추가해줭

    public List<CandidatePath> candidatePath;
    public List<Tile> path;
    public Tile currentNode;
    public Tile awayNode; // 원정 시 이동하게 될 타일 away --> 원정
    public Piece target;
    public float moveSpeed;

    void Awake()
    {
        //pieceData.InitialzePiece(this);
    }

    private void Start()
    {
        
    }

    public void Owned()
    {
        isOwned = true;
        FieldManager.instance.privatePieceCount[FieldManager.instance.FindPieceList(this)].PieceCountUp(this);
    }

    protected virtual void Attack()
    {
        print("일반 공격을 합니다.");
    }

    protected virtual void Skill()
    {
        print("스킬을 사용합니다.");
    }

    void Dead()
    {
        print("체력이 0 이하가 되어 사망.");
        DestroyPiece();
    }

    public void DestroyPiece()
    {
        FieldManager.instance.privatePieceCount[FieldManager.instance.FindPieceList(this)].PieceCountDown(this);
        Destroy(gameObject);
    }
    
    public void SetCurrentNode(Tile tile)
    {
        currentNode.node.walkable = true;
        tile.node.walkable = false;
        currentNode = tile;
    }

    public Tile GetTargetNode()
    {
        Tile targetNode = target.currentNode;

        return targetNode;
    }

    bool canMove = true;
    public Ease ease;

    //이동
    public void Move()
    {
        if (path.Count > 0 && canMove)
        {
            canMove = false;
            Vector3 targetTilePos = new Vector3(path[0].transform.position.x, transform.position.y, path[0].transform.position.z);
            transform.DOMove(targetTilePos, moveSpeed).SetEase(ease);

            currentNode = path[0];
            PieceControl pc = GetComponent<PieceControl>();
            pc.currentTile = path[0];
            path.RemoveAt(0);
            canMove = true;

            Invoke("Move", moveSpeed);
        }
    }

    //공격
    public void SetTargetPiece()
    {

    }

    //1. 준비 타일에서 전투 타일로 배치하는 경우 --> PLUS 

    //2. 전투 타일에서 전투 타일로 배치하는 경우 --> NONE
    //4. 준비 타일에서 준비 타일로 배치하는 경우 --> NONE

    //3. 전투 타일에서 준비 타일로 배치하는 경우 --> MINUS
    //5. 준비 타일에서 판매하는 경우 --> MINUS
    //6. 전투 타일에서 판매하는 경우 --> MINUS


    public void SetPiece(Piece currentPiece)
    {
        var a = currentPiece.GetComponent<PieceControl>();
        if(a.currentTile.isReadyTile == true && a.targetTile.isReadyTile == false)
        {
            // Plus
            FieldManager.instance.SynergeMythology[currentPiece.pieceData.mythology]++;
            FieldManager.instance.SynergeSpecies[currentPiece.pieceData.species]++;
            FieldManager.instance.SynergePlusSynerge[currentPiece.pieceData.plusSynerge]++;
            return;
        }
        if(a.currentTile.isReadyTile == false && a.targetTile.isReadyTile == true)
        {
            // Minus
            FieldManager.instance.SynergeMythology[currentPiece.pieceData.mythology]--;
            FieldManager.instance.SynergeSpecies[currentPiece.pieceData.species]--;
            FieldManager.instance.SynergePlusSynerge[currentPiece.pieceData.plusSynerge]--;
        }
    }

    public void SetPiece(Piece currentPiece, Piece targetPiece)
    {
        var a = currentPiece.GetComponent<PieceControl>();
        var b = targetPiece.GetComponent<PieceControl>();

        if (a.currentTile.isReadyTile == true && b.currentTile.isReadyTile == true) return;
        if (a.currentTile.isReadyTile == false && b.currentTile.isReadyTile == false) return;
        if (a.currentTile.isReadyTile == true && b.currentTile.isReadyTile == false)
        {
            FieldManager.instance.SynergeMythology[currentPiece.pieceData.mythology]++;
            FieldManager.instance.SynergeSpecies[currentPiece.pieceData.species]++;
            FieldManager.instance.SynergePlusSynerge[currentPiece.pieceData.plusSynerge]++;

            FieldManager.instance.SynergeMythology[targetPiece.pieceData.mythology]--;
            FieldManager.instance.SynergeSpecies[targetPiece.pieceData.species]--;
            FieldManager.instance.SynergePlusSynerge[targetPiece.pieceData.plusSynerge]--;
        }
        if (a.currentTile.isReadyTile == false && b.currentTile.isReadyTile == true)
        {
            FieldManager.instance.SynergeMythology[currentPiece.pieceData.mythology]--;
            FieldManager.instance.SynergeSpecies[currentPiece.pieceData.species]--;
            FieldManager.instance.SynergePlusSynerge[currentPiece.pieceData.plusSynerge]--;

            FieldManager.instance.SynergeMythology[targetPiece.pieceData.mythology]++;
            FieldManager.instance.SynergeSpecies[targetPiece.pieceData.species]++;
            FieldManager.instance.SynergePlusSynerge[targetPiece.pieceData.plusSynerge]++;
        }

        // Minus -- 
        //FieldManager.instance.SynergeMythology[targetPiece.pieceData.mythology]--;
        //FieldManager.instance.SynergeSpecies[targetPiece.pieceData.species]--;
        //FieldManager.instance.SynergePlusSynerge[targetPiece.pieceData.plusSynerge]--;

        // Plus
        //FieldManager.instance.SynergeMythology[currentPiece.pieceData.mythology]++;
        //FieldManager.instance.SynergeSpecies[currentPiece.pieceData.species]++;
        //FieldManager.instance.SynergePlusSynerge[currentPiece.pieceData.plusSynerge]++;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        if(path.Count > 0)
        {
            foreach (var tile in path)
            {
                Gizmos.DrawCube(new Vector3(tile.transform.position.x, tile.transform.position.y + 0.3f, tile.transform.position.z), new Vector3(0.3f, 0.3f, 0.3f));
            }
        }
    }
}
