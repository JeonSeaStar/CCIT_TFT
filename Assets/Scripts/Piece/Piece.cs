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

    public List<Synerge> synerges;
    public List<Equipment> Equipments;
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
    public int attackRange;

    public bool dead;

    public string owner;
    public bool isOwned;
    // ����ٰ� �������� ��ġ �Ǿ����� Ȯ���ϴ°� �߰��آa

    public List<CandidatePath> candidatePath;
    public List<Tile> path;
    public Tile currentNode;
    public Piece target;
    public float moveSpeed;

    bool canMove = true;
    public Ease ease;

    void Awake()
    {
        pieceData.InitialzePiece(this);
    }

    public void Owned()
    {
        isOwned = true;
    }

    protected virtual void Attack()
    {
        print(target.name + "���� �Ϲ� ������ �մϴ�.");
        Damage();

        Invoke("NextBehavior", attackSpeed);
    }

    protected virtual void Skill()
    {
        print("��ų�� ����մϴ�.");
    }

    protected bool RangeCheck()
    {
        if (attackRange >= FieldManager.instance.pathFinding.GetDistance(currentNode, target.currentNode))
            return true;
        else
            return false;
    }

    protected void Damage()
    {
        target.health -= attackDamage;

        if (target.health <= 0)
        {
            target.Dead();
            target = null;
        }
    }

    void Dead()
    {
        print(gameObject.name + "�� ü���� 0 ���ϰ� �Ǿ� ���.");
        dead = true;
        gameObject.SetActive(false);
    }

    //�̵�
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

            Invoke("NextBehavior", moveSpeed);
        }
    }

    public void NextBehavior()
    {
        currentNode.walkable = true;
        foreach (var enemy in FieldManager.instance.enemyFilePieceList)
            enemy.currentNode.walkable = true;
        FieldManager.instance.pathFinding.SetCandidatePath(this, FieldManager.instance.enemyFilePieceList);

        if (target != null)
        {
            if (RangeCheck())
            {
                Attack();
            }
            else
            {
                Move();
            }
        }
        else
        {
            NextBehavior();
        }
    }

    public void SetPiece(PieceData.Mythology mythology, PieceData.Species species, PieceData.PlusSynerge plussynerge)
    {
        FieldManager.instance.SynergeMythology[mythology]++;
        FieldManager.instance.SynergeSpecies[species]++;
        FieldManager.instance.SynergePlusSynerge[plussynerge]++;
    }

    public void RemovePiece(PieceData.Mythology mythology, PieceData.Species species, PieceData.PlusSynerge plussynerge)
    {
        FieldManager.instance.SynergeMythology[mythology]--;
        FieldManager.instance.SynergeSpecies[species]--;
        FieldManager.instance.SynergePlusSynerge[plussynerge]--;
    }
}
