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
    public float health;            //체력
    public float currentHealth;     //현재 체력
    public float mana;              //마나
    public float manaRecovery;      //마나 회복력

    [Space(10f)]
    public float attackPower;       //기본 공격력
    public float damageRise;        //공격력 상승분
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
    public float attackDamage;      //최종 공격력

    [Space(10f)]
    public float abilityPower;      //스킬 공격력
    public float abilityRise;       //스킬 공격력 상승분
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
    public float abilityDamage;     //최종 스킬 공격력

    [Space(10f)]
    public float armor;             //방어력
    public float armorRise;         //방어력 상승분
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
    public float armorSum;          //최종 방어력

    [Space(10f)]
    public float magicResist;       //마법 저항력
    public float magicResistRise;   //마법 저항력 상승분
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
    public float magicResistSum;     //최종 마법 저항력

    [Space(10f)]
    public float attackSpeed;       //공격속도
    public float attackSpeedRise;   //공격속도 상승분
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
    public float attackSpeedSum;    //최종 공격속도


    public float criticalChance;    //크리티컬 확률
    public float criticalDamage;    //크리티컬 배율
    public int attackRange;         //공격범위

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
        print(name + "(이)가 체력이 0 이하가 되어 사망.");
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
