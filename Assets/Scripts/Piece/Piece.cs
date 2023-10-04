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
    
    public float shield; //임시값
    public float currentShield; //임시값

    [Space(10f)]
    public float defaultAttackPower;
    private float damageRise;        
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
    private float abilityRise;       
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
    private float armorRise;         
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
    private float magicResistRise;   
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
    private float attackSpeedRise;   
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
    public Tile targetTile;
    public Piece target;
    public float moveSpeed;
    

    bool canMove = true;
    public Ease ease;

    [Header("장비")]
    public List<EquipmentData> equipmentDatas;

    [SerializeField] GameObject randomBoxObject;

    [Header("ExpeditionTile Information")]
    public Tile expeditionTile;

    #region //Just For Check
    enum Synergies
    {
        //MYTH
        GreatMountain_1, GreatMountain_2, GreatMountain_3, GreatMountain_4,
        FrostyWind_1, FrostyWind_2, FrostyWind_3,
        SandKingdom_1, SandKingdom_2, SandKingdom_3,
        HeavenGround_1, HeavenGround_2,
        BurningGround_1, BurningGround_2,
        //ANIMALS
        Hamster_1, Hamster_2, Hamster_3,
        Cat_1, Cat_2,
        Frog_1, Frog_2, Frog_3,
        Rabbit_1, Rabbit_2, Rabbit_3,
        Dog_1, Dog_2, Dog_3,
        //UNITED
        UnderWorld_1, UnderWorld_2,
        Faddist_1, Faddist_2,
        WarMachine_1, WarMachine_2,
        Creature_1, Creature_2, Creature_3, Creature_4,
    }
    #endregion
    [Header("Synerge")]
    public List<string> sReceivedBuff;

    public enum MonsterStatus
    {
        StatusImmunity,
        Freeze,
        Fear,
        Slow,
        Destruction
    }

    //public List<Coroutine> sBattleEffectInStart;
    //public List<Coroutine> sBattleEffectInProgress;

    void Awake()
    {
        pieceData.InitialzePiece(this);
        fieldManager = ArenaManager.Instance.fieldManagers[0];
    }

    public void Owned()
    {
        isOwned = true;
    }

    protected virtual void Attack()
    {
        print(name + "(이)가" + target.name + "에게 일반 공격을 합니다.");
        Damage();
        currentMana += manaRecovery;
        Invoke("NextBehavior", attackSpeed);
    }

    protected virtual void Skill()
    {
        print(name + "(이)가" + target.name + "에게 스킬을 사용합니다.");

        Invoke("NextBehavior", attackSpeed);
    }

    protected bool RangeCheck()
    {
        if (defaultAttackRange >= ArenaManager.Instance.fieldManagers[0].pathFinding.GetDistance(currentTile, target.currentTile))
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
        print(name + "(이)가 체력이 0 이하가 되어 사망.");
        dead = true;
        SpawnRandomBox();
        gameObject.SetActive(false);
    }

    void SpawnRandomBox()
    {
        GameObject box = Instantiate(randomBoxObject, transform.position, Quaternion.identity);
        RandomBox randomBox = box.GetComponent<RandomBox>();
        ArenaManager.Instance.fieldManagers[0].chest.CurveMove(randomBox.transform, fieldManager.targetPositions);
        ArenaManager.Instance.fieldManagers[0].chest.SetBoxContents(randomBox, 0);
    }

    //이동
    public void Move()
    {
        if (path.Count > 0 && canMove)
        {
            canMove = false;
            if (path[0].IsFull)
            {
                Invoke("NextBehavior", moveSpeed);
                return;
            }

            Vector3 targetTilePos = new Vector3(path[0].transform.position.x, transform.position.y, path[0].transform.position.z);
            transform.DOMove(targetTilePos, moveSpeed).SetEase(ease);
            currentTile.IsFull = false;
            currentTile = path[0];
            currentTile.IsFull = true;
            PieceControl pc = GetComponent<PieceControl>();
            pc.currentTile = path[0];
            path.RemoveAt(0);
            canMove = true;

            Invoke("NextBehavior", moveSpeed);
        }
    }

    public void NextBehavior()
    {
        if (CheckEnemySurvival(ArenaManager.Instance.fieldManagers[0].enemyFilePieceList))
        {
            foreach (var enemy in ArenaManager.Instance.fieldManagers[0].enemyFilePieceList)
                enemy.currentTile.walkable = true;
            ArenaManager.Instance.fieldManagers[0].pathFinding.SetCandidatePath(this, ArenaManager.Instance.fieldManagers[0].enemyFilePieceList);

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
        if (currentPiece.currentTile.isReadyTile == true && currentPiece.targetTile.isReadyTile == false)
        {
            var _duplicationCheck = fieldManager.myFilePieceList.FirstOrDefault(listPiece => listPiece.pieceName == currentPiece.pieceName);
            if (_duplicationCheck == null) fieldManager.SynergeIncrease(currentPiece);
            fieldManager.myFilePieceList.Add(currentPiece);
            fieldManager.CalSynerge(currentPiece);
        } // Set Ready -> Battle
        else if (currentPiece.currentTile.isReadyTile == false && currentPiece.targetTile.isReadyTile == true)
        {
            fieldManager.myFilePieceList.Remove(currentPiece);
            var _duplicationCheck = fieldManager.myFilePieceList.FirstOrDefault(listPiece => listPiece.pieceName == currentPiece.pieceName);
            if (_duplicationCheck == null) fieldManager.SynergeDecrease(currentPiece);
        } // Set Battle -> Ready
    }

    public void SetPiece(Piece currentPiece, Piece targetPiece, bool isControlPiece = false)
    {
        if (currentPiece.currentTile.isReadyTile == true && targetPiece.currentTile.isReadyTile == true) return;
        else if (currentPiece.currentTile.isReadyTile == false && targetPiece.currentTile.isReadyTile == false) return;
        else if (currentPiece.currentTile.isReadyTile == true && targetPiece.currentTile.isReadyTile == false)
        {
            fieldManager.myFilePieceList.Remove(targetPiece);
            var _duplicationTargetCheck = fieldManager.myFilePieceList.FirstOrDefault(listPiece => listPiece.pieceName == targetPiece.pieceName);
            if(_duplicationTargetCheck == null) fieldManager.SynergeDecrease(targetPiece); //Minus

            var _duplicationCurrentCheck = fieldManager.myFilePieceList.FirstOrDefault(listPiece => listPiece.pieceName == currentPiece.pieceName);
            if (_duplicationCurrentCheck == null) fieldManager.SynergeIncrease(currentPiece); //Plus
            fieldManager.myFilePieceList.Add(currentPiece);
        }  // Change Ready -> Battle
        else if (currentPiece.currentTile.isReadyTile == false && targetPiece.currentTile.isReadyTile == true)
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
