using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Piece;

public class Piece : MonoBehaviour
{
    public PieceData pieceData;

    public string pieceName;
    public Sprite piecePortrait;

    public enum Mythology { NONE = -1, A, B, C, D, E , MAX }
    public Mythology mythology = Mythology.NONE;
    public enum Species { NONE = -1, HAMSTER, CAT, DOG, FROG, RABBIT, MAX }
    public Species species = Species.NONE;
    public enum PlusSynerge { NONE, A, B, C, D, E , MAX }
    public PlusSynerge plusSynerge = PlusSynerge.NONE;
    //

    public List<Synerge> synerges;
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

    public List<Tile> path;
    public Tile currentNode;
    public Tile targetNode;
    public Piece target;
    public float moveSpeed;

    void Awake()
    {
        //pieceData.InitialzePiece(this);
    }

    private void Start()
    {
        
    }

    private void Update()
    {
        if(path.Count > 0 && canMove) { SetMoveTile(); }
        if(targetNode != null) { Move(); }
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

    public void SetMoveTile()
    {
        canMove = false;
        //Vector3 targetTilePos = new Vector3(path[0].transform.position.x, transform.position.y, path[0].transform.position.z);
        currentNode = path[0];
        PieceControl pc = GetComponent<PieceControl>();
        pc.currentTile = path[0];
        path.RemoveAt(0);
        canMove = true;
    }

    public void Move()
    {
        Tile target = targetNode;
        Vector3 StartTilePos = new Vector3(currentNode.transform.position.x, transform.position.y, currentNode.transform.position.z);
        Vector3 targetTilePos = new Vector3(path[0].transform.position.x, transform.position.y, path[0].transform.position.z);
        Vector3.MoveTowards(targetTilePos, targetTilePos, moveSpeed * Time.deltaTime);
    }

    void WalkMove(Tile currentTile, Tile targetTile)
    {
        Vector3 targetTilePos = targetTile.transform.position;
        transform.DOMove(new Vector3(targetTilePos.x, transform.position.y, targetTilePos.z), moveSpeed);
    }

    public void SetPiece(Mythology mythology, Species species, PlusSynerge plussynerge)
    {
        FieldManager.instance.SynergeMythology[mythology]++;
        FieldManager.instance.SynergeSpecies[species]++;
        FieldManager.instance.SynergePlusSynerge[plussynerge]++;
    }

    public void RemovePiece(Mythology mythology, Species species, PlusSynerge plussynerge)
    {
        FieldManager.instance.SynergeMythology[mythology]--;
        FieldManager.instance.SynergeSpecies[species]--;
        FieldManager.instance.SynergePlusSynerge[plussynerge]--;
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
