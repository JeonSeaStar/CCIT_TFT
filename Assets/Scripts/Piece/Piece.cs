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
    public float defaultHealth;     
    public float currentHealth;     
    public float defaultMana;       
    public float currentMana;       
    public float manaRecovery;      

    [Space(10f)]
    public float defaultAttackPower;
    public float damageRise;        
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
    public float attackPower;      

    [Space(10f)]
    public float defaultAbilityPower;
    public float abilityRise;       
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
    public float abilityDamage;     

    [Space(10f)]
    public float defaultArmor;      
    public float armorRise;         
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
    public float armor;             

    [Space(10f)]
    public float defaultMagicResist;
    public float magicResistRise;   
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
    public float magicResist;       

    [Space(10f)]
    public float defaultAttackSpeed;
    public float attackSpeedRise;   
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
    public float attackSpeed;       


    public float defaultCriticalChance;    
    public float defaultCriticalDamage;    
    public int defaultAttackRange;         

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
    [Header("ExpeditionTile Info")]
    public Tile expeditionTile;

    void Start()
    {
        pieceData.InitialzePiece(this);
        fieldManager = ArenaManager.Instance.fm[0];
    }

    public void Owned()
    {
        isOwned = true;
    }

    protected virtual void Attack()
    {
        print(name + "(��)��" + target.name + "���� �Ϲ� ������ �մϴ�.");
        Damage();
        currentMana += manaRecovery;
        Invoke("NextBehavior", attackSpeed);
    }

    protected virtual void Skill()
    {
        print(name + "(��)��" + target.name + "���� ��ų�� ����մϴ�.");

        Invoke("NextBehavior", attackSpeed);
    }

    protected bool RangeCheck()
    {
        if (defaultAttackRange >= ArenaManager.Instance.fm[0].pathFinding.GetDistance(currentTile, target.currentTile))
            return true;
        else
            return false;
    }

    protected void Damage()
    {
        target.defaultHealth -= attackPower;

        if (target.defaultHealth <= 0)
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
        ArenaManager.Instance.fm[0].chest.CurveMove(randomBox.transform, fieldManager.targetPositions);
        ArenaManager.Instance.fm[0].chest.SetBoxContents(randomBox, 0);
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
        if (CheckEnemySurvival(ArenaManager.Instance.fm[0].enemyFilePieceList))
        {
            foreach (var enemy in ArenaManager.Instance.fm[0].enemyFilePieceList)
                enemy.currentTile.walkable = true;
            ArenaManager.Instance.fm[0].pathFinding.SetCandidatePath(this, ArenaManager.Instance.fm[0].enemyFilePieceList);

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

    public bool CheckMyFileSurvival(List<Piece> myFile)
    {
        foreach (Piece my in myFile)
        {
            if (!my.dead)
                return true;
        }

        return false;
    }

    public bool CheckEnemySurvival(List<Piece> enemies)
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

    public void ExpeditionTileCheck()
    {
        var _x = 6 - currentTile.listX;
        var _y = 7 - currentTile.listY;

        expeditionTile = fieldManager.pathFinding.grid[_y].tile[_x];
    }
}
