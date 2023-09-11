using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Piece;
using static PieceData;
using System.Linq;
using Newtonsoft.Json.Bson;

public class Piece : MonoBehaviour
{
    public FieldManager fieldManager;
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

    private void Start()
    {
        // �ʱ�ȭ ���� �� Start ���� �־��ּ���.
        fieldManager = ArenaManager.instance.fm[0];
    }

    public void Owned()
    {
        isOwned = true;
    }

    protected virtual void Attack()
    {
        print(name + "(��)��" + target.name + "���� �Ϲ� ������ �մϴ�.");
        Damage();
        mana += manaRecovery;
        Invoke("NextBehavior", attackSpeed);
    }

    protected virtual void Skill()
    {
        print(name + "(��)��" + target.name + "���� ��ų�� ����մϴ�.");

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
        print(name + "(��)�� ü���� 0 ���ϰ� �Ǿ� ���.");
        dead = true;
        gameObject.SetActive(false);
    }

    //�̵�
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
            print(name + "(��)�� �¸��� �� �ߴ� ��.");
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

    /// <summary>
    /// �⹰ ��ġ�� �ó��� ���,
    /// �ó��� ��� ��� On/Off�� ���� Boolean �߰�
    /// </summary>
    /// <param name="currentPiece"></param>
    /// <param name="isControlPiece"></param>
    public void SetPiece(Piece currentPiece, bool isControlPiece = false)
    {
        for(int i = 0; i < fieldManager.myFilePieceList.Count;i++)
        {
            if (fieldManager.myFilePieceList[i].pieceName == currentPiece.pieceName) return;
        }

        var _currentPiece = currentPiece.GetComponent<PieceControl>();

        if (_currentPiece.currentTile.isReadyTile == true && _currentPiece.targetTile.isReadyTile == false)
        {
            SynergeIncrease(currentPiece); // Plus
            fieldManager.myFilePieceList.Add(currentPiece);
            return;
        }
        if (_currentPiece.currentTile.isReadyTile == false && _currentPiece.targetTile.isReadyTile == true)
        {
            SynergeDecrease(currentPiece); // Minus
            fieldManager.myFilePieceList.Remove(currentPiece);
        }
    }
    public void SetPiece(Piece currentPiece, Piece targetPiece , bool isControlPiece = false)
    {
        var _currentPiece = currentPiece.GetComponent<PieceControl>();
        var _targetPiece = targetPiece.GetComponent<PieceControl>();

        if (_currentPiece.currentTile.isReadyTile == true && _targetPiece.currentTile.isReadyTile == true) return;
        if (_currentPiece.currentTile.isReadyTile == false && _targetPiece.currentTile.isReadyTile == false) return;
        if (_currentPiece.currentTile.isReadyTile == true && _targetPiece.currentTile.isReadyTile == false)
        {
            fieldManager.myFilePieceList.Remove(targetPiece);

            //�ߺ� Ȯ��
            foreach(Piece targetPieceCheck in fieldManager.myFilePieceList)
            {
                if (targetPieceCheck.pieceName == targetPiece.pieceName) break;
                if (targetPieceCheck == fieldManager.myFilePieceList.Last() && targetPieceCheck.pieceName != targetPiece.pieceName)
                {
                    SynergeDecrease(targetPiece); //Minus
                }
            }
            foreach(Piece currentPieceCheck in fieldManager.myFilePieceList)
            {
                if (currentPieceCheck.pieceName == currentPiece.pieceName) break;
                if (currentPieceCheck == fieldManager.myFilePieceList.Last() && currentPieceCheck.pieceName != currentPiece.pieceName)
                {
                    SynergeIncrease(currentPiece); //Plus
                }
            }

            fieldManager.myFilePieceList.Add(currentPiece);
        }
        if (_currentPiece.currentTile.isReadyTile == false && _targetPiece.currentTile.isReadyTile == true)
        {
            fieldManager.myFilePieceList.Remove(currentPiece);
            foreach(Piece currentPieceCheck in fieldManager.myFilePieceList)
            {
                if (currentPieceCheck.pieceName == currentPiece.pieceName) break;
                if (currentPieceCheck == fieldManager.myFilePieceList.Last() && currentPieceCheck.pieceName != currentPiece.pieceName)
                {
                    SynergeDecrease(currentPiece);//Miuns
                }
            }
            foreach(Piece targetPieceCheck in fieldManager.myFilePieceList)
            {
                if (targetPieceCheck.pieceName == targetPiece.pieceName) break;
                if (targetPieceCheck == fieldManager.myFilePieceList.Last() && targetPieceCheck.pieceName != targetPiece.pieceName)
                {
                    SynergeIncrease(targetPiece);//Plus
                }
            }

        }
    }

    /// <summary>
    /// �ش� �⹰�� �ó��� ���� ������ŵ�ϴ�.
    /// </summary>
    /// <param name="piece"></param>
    void SynergeIncrease(Piece piece)
    {
        fieldManager.mythActiveCount[piece.pieceData.myth]++;
        fieldManager.animalActiveCount[piece.pieceData.animal]++;
        fieldManager.unitedActiveCount[piece.pieceData.united]++;
    }

    /// <summary>
    /// �ش� �⹰�� �ó��� ���� ���ҽ�ŵ�ϴ�.
    /// </summary>
    /// <param name="piece"></param>
    void SynergeDecrease(Piece piece)
    {
        fieldManager.mythActiveCount[piece.pieceData.myth]--;
        fieldManager.animalActiveCount[piece.pieceData.animal]--;
        fieldManager.unitedActiveCount[piece.pieceData.united]--;

        SynergeCalculateDelegate test = MythSynergeCalculate;
    }

    delegate void SynergeCalculateDelegate();
    delegate void SynergeAnimalDelegate(ref FieldManager fieldManager);
    delegate void SynergeUnitedDelegate(ref FieldManager fieldManager);

    void MythSynergeCalculate()
    {

    }

    void AnimalSynergeCalculate()
    {

    }

    void UnitedSynergeCalculate()
    {

    }
}
