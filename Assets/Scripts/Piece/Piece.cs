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
    public List<Equipment> Equipments;
    public int star = 0;

    public float health;
    public float mana;
    public float manaRecovery;
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
    // 여기다가 전투존에 배치 되었는지 확인하는거 추가해줭

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
        print(name + "(이)가" + target.name + "에게 일반 공격을 합니다.");
        Damage();
        mana += manaRecovery;
        Invoke("NextBehavior", attackSpeed);
    }

    protected virtual void Skill()
    {
        print(name + "(이)가" + target.name + "에게 스킬을 사용합니다.");

        Invoke("NextBehavior", attackSpeed);
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
        print(name + "(이)가 체력이 0 이하가 되어 사망.");
        dead = true;
        gameObject.SetActive(false);
    }

    //이동
    public void Move()
    {
        if (path.Count > 0 && canMove)
        {
            canMove = false;
            if (path[0].isFull)
            {
                Invoke("NextBehavior", moveSpeed);
                return;
            }

            Vector3 targetTilePos = new Vector3(path[0].transform.position.x, transform.position.y, path[0].transform.position.z);
            transform.DOMove(targetTilePos, moveSpeed).SetEase(ease);
            currentNode.isFull = false;
            currentNode = path[0];
            currentNode.isFull = true;
            PieceControl pc = GetComponent<PieceControl>();
            pc.currentTile = path[0];
            path.RemoveAt(0);
            canMove = true;

            Invoke("NextBehavior", moveSpeed);
        }
    }

    public void NextBehavior()
    {
        if (CheckSurvival(FieldManager.instance.enemyFilePieceList))
        {
            foreach (var enemy in FieldManager.instance.enemyFilePieceList)
                enemy.currentNode.walkable = true;
            FieldManager.instance.pathFinding.SetCandidatePath(this, FieldManager.instance.enemyFilePieceList);

            if (target != null)
            {
                if (RangeCheck())
                    Attack();
                else
                    Move();
            }
            else
                NextBehavior();
        }
        else
            print(name + "(이)가 승리의 춤 추는 중.");
    }

    bool CheckSurvival(List<Piece> enemies)
    {
        foreach (Piece enemy in enemies)
        {
            if (!enemy.dead)
                return true;
        }

        return false;
    }
    public void SetPiece(Piece currentPiece)
    {
        var a = currentPiece.GetComponent<PieceControl>();
        if (a.currentTile.isReadyTile == true && a.targetTile.isReadyTile == false)
        {
            // Plus
            FieldManager.instance.SynergeMythology[currentPiece.pieceData.mythology]++;
            FieldManager.instance.SynergeSpecies[currentPiece.pieceData.species]++;
            FieldManager.instance.SynergePlusSynerge[currentPiece.pieceData.plusSynerge]++;
            return;
        }
        if (a.currentTile.isReadyTile == false && a.targetTile.isReadyTile == true)
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

}
