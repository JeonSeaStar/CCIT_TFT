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
    // ����ٰ� �������� ��ġ �Ǿ����� Ȯ���ϴ°� �߰��آa

    public List<Tile> path;
    public Tile currentNode;
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
        print("�Ϲ� ������ �մϴ�.");
    }

    protected virtual void Skill()
    {
        print("��ų�� ����մϴ�.");
    }

    void Dead()
    {
        print("ü���� 0 ���ϰ� �Ǿ� ���.");
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
