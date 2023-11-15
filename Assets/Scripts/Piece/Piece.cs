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
    public float bloodBrain;

    public float shield; //임시값
    public float currentShield; //임시값

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

    [Header("버프")]
    public List<BuffData> buffList;

    [Header("상태")]
    public bool immune; //상태면역
    public bool freeze;
    public bool slow;
    public bool airborne;
    public bool faint;
    public bool fear;
    public bool invincible;
    public bool charm; //매혹

    public List<Piece> enemyPieceList = new List<Piece>();
    public List<Piece> myPieceList = new List<Piece>();

    void Awake()
    {
        pieceData.InitialzePiece(this);
        pieceName = pieceData.pieceName;
        fieldManager = ArenaManager.Instance.fieldManagers[0];
    }

    public void Owned()
    {
        isOwned = true;
    }

    public delegate void OnceAttackEffect();
    OnceAttackEffect onceAttackEffect;
    protected virtual void Attack()
    {
        print(name + "(이)가" + target.name + "에게 일반 공격을 합니다.");
        Damage(attackDamage);
        //currentMana += manaRecovery;
        Invoke("NextBehavior", attackSpeed);
    }

    protected virtual void Skill()
    {
        print(name + "(이)가" + target.name + "에게 스킬을 사용합니다.");

        Invoke("NextBehavior", attackSpeed);
    }

    protected virtual void Effect() { }

    protected bool RangeCheck()
    {
        //if (defaultAttackRange >= ArenaManager.Instance.fieldManagers[0].pathFinding.GetDistance(currentTile, target.currentTile))
        //print(attackRange + ", " + ArenaManager.Instance.fieldManagers[0].pathFinding.GetDistance(currentTile, target.currentTile));
        //print(attackRange + ", " + ArenaManager.Instance.fieldManagers[0].pathFinding.GetDistance(currentTile, target.currentTile));
        if (attackRange >= ArenaManager.Instance.fieldManagers[0].pathFinding.GetDistance(currentTile, target.currentTile))
            return true;
        else
            return false;
    }

    public void Damage(Piece piece, float damage)
    {
        piece.health -= damage;
        if (piece.health <= 0) piece.Dead();
    }

    public void Damage(float damage)
    {
        if (target.invincible)
            return;

        target.health -= damage;

        if (target.health <= 0)
        {
            #region 악마 기물 시너지 확인
            var _burningPiece = ArenaManager.Instance.fieldManagers[0].buffManager.mythBuff[0];
            if(buffList.Contains(_burningPiece.burningGroundBuff[0]) || buffList.Contains(_burningPiece.burningGroundBuff[1]))
            {
                var _buff = (ArenaManager.Instance.fieldManagers[0].mythActiveCount[PieceData.Myth.BurningGround] >= 4) ? _burningPiece.burningGroundBuff[1] : _burningPiece.burningGroundBuff[0];
                _buff.DirectEffect(this, true);
                _buff.BattleStartEffect(true);

                int _r = UnityEngine.Random.Range(0, 9);
                if (_r == 0 && _buff == _burningPiece.burningGroundBuff[1])
                {
                    target.SetCharm();
                }
            }
            #endregion
            #region 고양이 기물 시너지 확인
            if (isCatSynergeActiveCheck)
            {
                int _r = (ArenaManager.Instance.fieldManagers[0].animalActiveCount[PieceData.Animal.Cat] >= 4) ? UnityEngine.Random.Range(0, 3) : UnityEngine.Random.Range(0, 2);
                if (_r == 0) 
                { 
                    int _gold = UnityEngine.Random.Range(2, 6);
                    ArenaManager.Instance.fieldManagers[0].DualPlayers[0].gold += _gold;
                } 
                
            }
            #endregion

            target.Dead();
            target = null;
        }
    }
    


    public void Dead()
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

            currentTile.piece = null;
            currentTile.IsFull = false;
            currentTile = path[0];

            currentTile.piece = this;
            currentTile.IsFull = true;

            path.RemoveAt(0);
            canMove = true;

            Invoke("NextBehavior", moveSpeed);
        }
    }
    #region 토끼
    public void RabbitJump()
    {
        var _distance = ArenaManager.Instance.fieldManagers[0].pathFinding.GetDistance(currentTile, target.currentTile);
        List<Tile> _neighbor = new List<Tile>();
        if (attackRange >= _distance) //제자리 점프
        {
            _neighbor = ArenaManager.Instance.fieldManagers[0].pathFinding.GetNeighbor(currentTile);
            Vector3[] Jumppath ={ new Vector3(transform.position.x,transform.position.y,transform.position.z),
                                     new Vector3(transform.position.x,transform.position.y + 3f, transform.position.z),
                                     new Vector3(transform.position.x, transform.position.y, transform.position.z) };
            GetComponent<Rigidbody>().DOPath(Jumppath, 2, PathType.CatmullRom, PathMode.Full3D); //점프구간
        }
        else
        {
            if (_distance <= 4)
            {
                _neighbor = ArenaManager.Instance.fieldManagers[0].pathFinding.GetNeighbor(target.currentTile);
                foreach(var _neighborTile in _neighbor)
                {
                    if (_neighborTile.IsFull == false)
                    {
                        Vector3 targetTilePos = new Vector3(path[0].transform.position.x, 1, path[0].transform.position.z);
                        Vector3 hpos = transform.position + ((targetTilePos - transform.position) / 2);
                        Vector3[] Jumppath = { new Vector3(transform.position.x, transform.position.y, transform.position.z),
                                                 new Vector3(hpos.x, hpos.y + 3f, hpos.z),
                                                 new Vector3(targetTilePos.x, targetTilePos.y, targetTilePos.z) };
                        GetComponent<Rigidbody>().DOPath(Jumppath, 2, PathType.CatmullRom, PathMode.Full3D); //점프구간

                        currentTile.piece = null;
                        currentTile.IsFull = false;
                        currentTile = _neighborTile;

                        currentTile.piece = this;
                        currentTile.IsFull = true;
                    }
                }
            }
            else { Move(); return; } 
        }

        StartCoroutine(RabbitSplashDamage(_neighbor, 3));
        Invoke("NextBehavior", 3); 
    }

    IEnumerator RabbitSplashDamage(List<Tile> neighbor, int time)
    {
        yield return new WaitForSeconds(time);
        int splashDamage = 0;
        var _buff = ArenaManager.Instance.fieldManagers[0].buffManager.animalBuff[0];
        if (ArenaManager.Instance.fieldManagers[0].DualPlayers[0].buffDatas.Contains(_buff.rabbitBuff[0])) { splashDamage = 10; pieceData.CalculateBuff(this, _buff.rabbitBuff[0]); }
        else if (ArenaManager.Instance.fieldManagers[0].DualPlayers[0].buffDatas.Contains(_buff.rabbitBuff[1])) { splashDamage = 15; pieceData.CalculateBuff(this, _buff.rabbitBuff[1]); }
        else if (ArenaManager.Instance.fieldManagers[0].DualPlayers[0].buffDatas.Contains(_buff.rabbitBuff[2])) { splashDamage = 20; pieceData.CalculateBuff(this, _buff.rabbitBuff[2]); }

        Invoke("RsetRabbitStatus", 3);
        foreach (var tile in neighbor)
        {
            if (tile.IsFull && !tile.piece.isOwned) Damage(tile.piece, splashDamage);
        }

        isRabbitSynergeActiveCheck = false;
    }

    void RsetRabbitStatus()
    {
        if (ArenaManager.Instance.roundType != ArenaManager.RoundType.Battle) return;

        var _rabbitCount = ArenaManager.Instance.fieldManagers[0].animalActiveCount[PieceData.Animal.Rabbit];
        int _activeCount = _rabbitCount / 3;
        this.attackSpeed -= pieceData.attackSpeed[star] * 0.1f * _activeCount;
        this.moveSpeed -= pieceData.moveSpeed[star] * 0.1f * _activeCount;
    }

    [HideInInspector] public bool isRabbitSynergeActiveCheck;
    #endregion
    #region 고양이
    [HideInInspector] public bool isCatSynergeActiveCheck;
    #endregion

    void EnemyCheck()
    {
        if (isOwned)
        {
            myPieceList = fieldManager.myFilePieceList;
            enemyPieceList = fieldManager.enemyFilePieceList;
        }
        else
        {
            myPieceList = fieldManager.enemyFilePieceList;
            enemyPieceList = fieldManager.myFilePieceList;
        }
    }

    public void NextBehavior()
    {
        EnemyCheck();

        if (CheckEnemySurvival(enemyPieceList))
        {
            foreach (var enemy in enemyPieceList)
                enemy.currentTile.walkable = true;
            ArenaManager.Instance.fieldManagers[0].pathFinding.SetCandidatePath(this, enemyPieceList);

            if (target != null)
            {
                if (isRabbitSynergeActiveCheck) { RabbitJump(); return; }
                if (isCatSynergeActiveCheck) { }

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
            fieldManager.AddDPList(currentPiece);
            fieldManager.CalSynerge(currentPiece);
        } // Set Ready -> Battle
        else if (currentPiece.currentTile.isReadyTile == false && currentPiece.targetTile.isReadyTile == true)
        {
            fieldManager.RemoveDPList(currentPiece);
            fieldManager.myFilePieceList.Remove(currentPiece);
            currentPiece.buffList.Clear();
            var _duplicationCheck = fieldManager.myFilePieceList.FirstOrDefault(listPiece => listPiece.pieceName == currentPiece.pieceName);
            if (_duplicationCheck == null) fieldManager.SynergeDecrease(currentPiece);
            fieldManager.CalSynerge(currentPiece);
        } // Set Battle -> Ready
    }

    public void SetPiece(Piece currentPiece, Piece targetPiece, bool isControlPiece = false)
    {
        if (currentPiece.currentTile.isReadyTile == true && targetPiece.currentTile.isReadyTile == true) return;
        else if (currentPiece.currentTile.isReadyTile == false && targetPiece.currentTile.isReadyTile == false) return;
        else if (currentPiece.currentTile.isReadyTile == true && targetPiece.currentTile.isReadyTile == false)
        {
            fieldManager.RemoveDPList(currentPiece);
            fieldManager.myFilePieceList.Remove(targetPiece);
            targetPiece.buffList.Clear();
            var _duplicationTargetCheck = fieldManager.myFilePieceList.FirstOrDefault(listPiece => listPiece.pieceName == targetPiece.pieceName);
            if(_duplicationTargetCheck == null) fieldManager.SynergeDecrease(targetPiece); //Minus
            

            var _duplicationCurrentCheck = fieldManager.myFilePieceList.FirstOrDefault(listPiece => listPiece.pieceName == currentPiece.pieceName);
            if (_duplicationCurrentCheck == null) fieldManager.SynergeIncrease(currentPiece); //Plus
            fieldManager.myFilePieceList.Add(currentPiece);
            fieldManager.AddDPList(currentPiece);
            fieldManager.CalSynerge(currentPiece, targetPiece);
        }  // Change Ready -> Battle
        else if (currentPiece.currentTile.isReadyTile == false && targetPiece.currentTile.isReadyTile == true)
        {
            fieldManager.RemoveDPList(currentPiece);
            fieldManager.myFilePieceList.Remove(currentPiece);
            currentPiece.buffList.Clear();
            var _duplicationCurrentCheck = fieldManager.myFilePieceList.FirstOrDefault(listPiece => listPiece.pieceName == currentPiece.pieceName);
            if (_duplicationCurrentCheck == null) fieldManager.SynergeDecrease(currentPiece); //Minus

            var _duplicationTargetCheck = fieldManager.myFilePieceList.FirstOrDefault(listPiece => listPiece.pieceName == targetPiece.pieceName);
            if(_duplicationTargetCheck == null) fieldManager.SynergeIncrease(targetPiece); //Plus
            fieldManager.myFilePieceList.Add(targetPiece);
            fieldManager.AddDPList(currentPiece);
            fieldManager.CalSynerge(targetPiece, currentPiece);
        }  // Change Battle -> Ready
    }

    #region 상태이상
    public void SetFreeze()
    {
        freeze = true;
        //1초 뒤에 빙결을 푸는 기능 추가해주세요.
        //canMove = false;
    }

    public void SetImmune()
    {
        immune = true;
    }

    public void SetSlow()
    {
        slow = true;
    }

    public void SetFaint()
    {
        faint = true;
    }

    public void SetAirborne()
    {
        airborne = true;
    }

    public void SetFear()
    {
        fear = true;
    }

    public void SetInvincible()
    {
        invincible = true;
    }

    public void SetCharm()
    {
        charm = true;
    }

    //void 
    #endregion
}
