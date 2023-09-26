using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;
using Newtonsoft.Json.Bson;
using System;

public class Piece : MonoBehaviour
{
    public FieldManager fieldManager;
    public PieceData pieceData;

    public int grade;

    public string pieceName;
    public Sprite piecePortrait;
    public List<Equipment> Equipments;
    public int star = 0;

    [Header("Player Stats")]
    public float health;            //ü��
    public float currentHealth;     //���� ü��
    public float mana;              //����
    public float manaRecovery;      //���� ȸ����

    [Space(10f)]
    public float attackPower;       //�⺻ ���ݷ�
    public float damageRise;        //���ݷ� ��º�
    public float DamageRise
    {
        get { return damageRise; }
        set
        {
            if (damageRise != value)
            {
                damageRise = value;
                //Event Detect Change Rise
            }
        }
    }
    public float attackDamage;      //���� ���ݷ�

    [Space(10f)]
    public float abilityPower;      //��ų ���ݷ�
    public float abilityRise;       //��ų ���ݷ� ��º�
    public float AbilityRise
    {
        get { return abilityRise; }
        set
        {
            if (abilityRise != value)
            {
                abilityRise = value;
                //Event Detect Change Rise
            }
        }
    }
    public float abilityDamage;     //���� ��ų ���ݷ�

    [Space(10f)]
    public float armor;             //����
    public float armorRise;         //���� ��º�
    public float ArmorRise
    {
        get { return armorRise; }
        set
        {
            if (armorRise != value)
            {
                armorRise = value;
                //Event Detect Change Rise
            }
        }
    }
    public float armorSum;          //���� ����

    [Space(10f)]
    public float magicResist;       //���� ���׷�
    public float magicResistRise;   //���� ���׷� ��º�
    public float MagicResistRise
    {
        get { return magicResistRise; }
        set
        {
            if (magicResistRise != value)
            {
                magicResistRise = value;
                //Event Detect Change Rise
            }
        }
    }
    public float magicResistSum;     //���� ���� ���׷�

    [Space(10f)]
    public float attackSpeed;       //���ݼӵ�
    public float attackSpeedRise;   //���ݼӵ� ��º�
    public float AttackSpeedRise
    {
        get { return attackSpeedRise; }
        set
        {
            if(attackSpeedRise != value)
            {
                attackSpeedRise = value;
            }
        }
    }
    public float attackSpeedSum;    //���� ���ݼӵ�


    public float criticalChance;    //ũ��Ƽ�� Ȯ��
    public float criticalDamage;    //ũ��Ƽ�� ����
    public int attackRange;         //���ݹ���

    public bool dead;

    public string owner;
    public bool isOwned;

    public List<CandidatePath> candidatePath;
    public List<Tile> path;
    public Tile currentTile;
    public Piece target;
    public float moveSpeed;

    bool canMove = true;
    public Ease ease;

    [SerializeField] GameObject randomBoxObject;

    void Start()
    {
        pieceData.InitialzePiece(this);
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
        if (attackRange >= ArenaManager.instance.fm[0].pathFinding.GetDistance(currentTile, target.currentTile))
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
        SpawnRandomBox();
        gameObject.SetActive(false);
    }

    void SpawnRandomBox()
    {
        GameObject box = Instantiate(randomBoxObject, transform.position, Quaternion.identity);
        RandomBox randomBox = box.GetComponent<RandomBox>();
        randomBox.CurveMove(fieldManager.targetPositions);
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
            currentTile.isFull = false;
            currentTile = path[0];
            currentTile.isFull = true;
            PieceControl pc = GetComponent<PieceControl>();
            pc.currentTile = path[0];
            path.RemoveAt(0);
            canMove = true;

            Invoke("NextBehavior", moveSpeed);
        }
    }

    public void NextBehavior()
    {
        if (CheckSurvival(ArenaManager.instance.fm[0].enemyFilePieceList))
        {
            foreach (var enemy in ArenaManager.instance.fm[0].enemyFilePieceList)
                enemy.currentTile.walkable = true;
            ArenaManager.instance.fm[0].pathFinding.SetCandidatePath(this, ArenaManager.instance.fm[0].enemyFilePieceList);

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

    public void SetPiece(Piece currentPiece, bool isControlPiece = false)
    {
        var _currentPiece = currentPiece.GetComponent<PieceControl>();

        if (_currentPiece.currentTile.isReadyTile == true && _currentPiece.targetTile.isReadyTile == false)
        {
            var _duplicationCheck = fieldManager.myFilePieceList.FirstOrDefault(listPiece => listPiece.pieceName == currentPiece.pieceName);
            if (_duplicationCheck == null) fieldManager.SynergeIncrease(currentPiece);
            fieldManager.myFilePieceList.Add(currentPiece);
            fieldManager.CalSynerge(currentPiece);
        } // Set Ready -> Battle
        else if (_currentPiece.currentTile.isReadyTile == false && _currentPiece.targetTile.isReadyTile == true)
        {
            fieldManager.myFilePieceList.Remove(currentPiece);
            var _duplicationCheck = fieldManager.myFilePieceList.FirstOrDefault(listPiece => listPiece.pieceName == currentPiece.pieceName);
            if (_duplicationCheck == null) fieldManager.SynergeDecrease(currentPiece);
        } // Set Battle -> Ready
    }

    public void SetPiece(Piece currentPiece, Piece targetPiece, bool isControlPiece = false)
    {
        var _currentPiece = currentPiece.GetComponent<PieceControl>();
        var _targetPiece = targetPiece.GetComponent<PieceControl>();

        if (_currentPiece.currentTile.isReadyTile == true && _targetPiece.currentTile.isReadyTile == true) return;
        else if (_currentPiece.currentTile.isReadyTile == false && _targetPiece.currentTile.isReadyTile == false) return;
        else if (_currentPiece.currentTile.isReadyTile == true && _targetPiece.currentTile.isReadyTile == false)
        {
            fieldManager.myFilePieceList.Remove(targetPiece);
            var _duplicationTargetCheck = fieldManager.myFilePieceList.FirstOrDefault(listPiece => listPiece.pieceName == targetPiece.pieceName);
            if(_duplicationTargetCheck == null) fieldManager.SynergeDecrease(targetPiece); //Minus

            var _duplicationCurrentCheck = fieldManager.myFilePieceList.FirstOrDefault(listPiece => listPiece.pieceName == currentPiece.pieceName);
            if (_duplicationCurrentCheck == null) fieldManager.SynergeIncrease(currentPiece); //Plus
            fieldManager.myFilePieceList.Add(currentPiece);
        }  // Change Ready -> Battle
        else if (_currentPiece.currentTile.isReadyTile == false && _targetPiece.currentTile.isReadyTile == true)
        {
            fieldManager.myFilePieceList.Remove(currentPiece);
            var _duplicationCurrentCheck = fieldManager.myFilePieceList.FirstOrDefault(listPiece => listPiece.pieceName == currentPiece.pieceName);
            if (_duplicationCurrentCheck == null) fieldManager.SynergeDecrease(currentPiece); //Minus

            var _duplicationTargetCheck = fieldManager.myFilePieceList.FirstOrDefault(listPiece => listPiece.pieceName == targetPiece.pieceName);
            if(_duplicationTargetCheck == null) fieldManager.SynergeIncrease(targetPiece); //Plus
            fieldManager.myFilePieceList.Add(targetPiece);
        }  // Change Battle -> Ready
    }

    public delegate void Buff(Piece piece, int count, bool isReadyTile);
    public Buff sBuff;

    void ExpeditionTileCheck()
    {

    }
}
