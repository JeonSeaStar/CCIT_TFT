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

    public string pieceName;
    public Sprite piecePortrait;
    public List<Equipment> Equipments;
    public int star = 0;

    [Header("Player Stats")]
    public float health;
    public float mana;
    public float manaRecovery;
    public float attackDamage;
    public float abilityPower;
    public float abilityPowerCoefficient;
    public float armor;
    public float magicResist;
    public float attackSpeed;
    public float criticalChance;
    public float criticalDamage;
    public float attackRange;
    public float bloodBrain;

    public float shield; //�ӽð�
    public float currentShield; //�ӽð�

    public bool dead;

    public string owner;
    public bool isOwned;

    public List<CandidatePath> candidatePath;
    public List<Tile> path;
    public Tile currentTile;
    public Tile targetTile;
    public Piece target;
    public float moveSpeed = 0.7f;


    bool canMove = true;
    [Header("�̵� DoTween")] public Ease ease;
    [Header("�䳢 DoTween")] public Ease rabbitEase;

    [Header("���")]
    public List<EquipmentData> equipmentDatas;

    [SerializeField] GameObject randomBoxObject;

    [Header("����")]
    public List<BuffData> buffList;

    [Header("����")]
    public bool immune; //���¸鿪
    public bool freeze;
    public bool slow;
    public bool airborne;
    public bool faint;
    public bool fear;
    public bool invincible;
    public bool charm; //��Ȥ
    public bool blind;
    public bool stun;

    public List<Piece> enemyPieceList = new List<Piece>();
    public List<Piece> myPieceList = new List<Piece>();

    public Animator animator;

    [Header("���»���")]
    public float basicAttackDamage;
    public float basicShield;
    public float basicAttackSpeed;
    public float basicHealth;


    public enum State
    {
        NONE,
        IDLE,
        MOVE,
        ATTACK,
        SKILL,
        DANCE,
        DEAD
    }
    public State PieceState
    {
        get { return pieceState; }
        set
        {
            if (PieceState != value)
            {
                pieceState = value;
                animator.SetTrigger(PieceState.ToString());
            }
        }
    }
    public State pieceState;

    [Header("����Ʈ")]
    public bool isEquip; // ��� ���� ����
    public GameObject handAttackEffects; // �Ǽ� Ÿ��
    public GameObject weaponAttackEffects; // ���� Ÿ��
    public GameObject skillEffects;

    public PieceHealthBar healthbar;

    public Tile nextTile;

    void Awake()
    {
        pieceData.InitialzePiece(this);
        pieceName = pieceData.pieceName;
        fieldManager = ArenaManager.Instance.fieldManagers[0];
        PieceState = State.IDLE;
    }

    public void Owned()
    {
        isOwned = true;
    }

    public delegate void OnceAttackEffect();
    OnceAttackEffect onceAttackEffect;
    public void AttackState()
    {
        transform.DOLookAt(target.transform.position, 0.1f);
        PieceState = State.ATTACK;
    }

    public void SkillState()
    {
        PieceState = State.SKILL;
    }

    public void Effect() { }

    protected bool RangeCheck()
    {
        if (attackRange >= ArenaManager.Instance.fieldManagers[0].pathFinding.GetDistance(currentTile, target.currentTile))
            return true;
        else
            return false;
    }

    public void Damage(Piece piece, float damage)
    {
        if (shield > 0)
        {
            float shieldPoint = piece.shield;
            if (shieldPoint < damage)
            {
                damage = damage - shieldPoint;
                piece.health -= damage;
                shieldPoint = 0;
            }
            else
            {
                shieldPoint -= damage;
            }
            piece.shield = shieldPoint;
        }
        else
        {
            piece.health -= damage;
        }
        if (piece.health <= 0) piece.DeadState();
    }

    public void Damage(float damage)
    {
        if (target == null)
            return;

        if (target.invincible)
            return;

        if (target.shield > 0)
        {
            float shieldPoint = target.shield;
            if (shieldPoint < damage)
            {
                damage = damage - shieldPoint;
                target.health -= damage;
                shieldPoint = 0;
            }
            else
            {
                shieldPoint -= damage;
            }
            target.shield = shieldPoint;
        }
        else
        {
            target.health -= damage;
        }

        if (target.health <= 0)
        {
            //#region �Ǹ� �⹰ �ó��� Ȯ��
            //var _burningPiece = ArenaManager.Instance.fieldManagers[0].buffManager.mythBuff[0];
            //if (buffList.Contains(_burningPiece.burningGroundBuff[0]) || buffList.Contains(_burningPiece.burningGroundBuff[1]))
            //{
            //    var _buff = (ArenaManager.Instance.fieldManagers[0].mythActiveCount[PieceData.Myth.BurningGround] >= 4) ? _burningPiece.burningGroundBuff[1] : _burningPiece.burningGroundBuff[0];
            //    _buff.DirectEffect(this, true);
            //    _buff.BattleStartEffect(true);

            //    int _r = UnityEngine.Random.Range(0, 9);
            //    if (_r == 0 && _buff == _burningPiece.burningGroundBuff[1])
            //    {
            //        target.SetCharm();
            //    }
            //}
            //#endregion
            #region ������ �⹰ �ó��� Ȯ��
            if (isCatSynergeActiveCheck)
            {
                int _r = (ArenaManager.Instance.fieldManagers[0].animalActiveCount[PieceData.Animal.Cat] >= 4) ? UnityEngine.Random.Range(0, 3) : UnityEngine.Random.Range(0, 2);
                if (_r == 0)
                {
                    int _gold = UnityEngine.Random.Range(2, 6);
                    ArenaManager.Instance.fieldManagers[0].DualPlayers[0].gold += _gold;
                    Debug.Log(_gold + " ��ŭ ��带 ȹ���մϴ�.");
                }
            }
            #endregion

            target.DeadState();
            target = null;
        }
    }

    public void IdleState()
    {
        PieceState = State.IDLE;
        StartNextBehavior();
    }

    public void DeadState()
    {
        StopAllCoroutines();
        PieceState = State.DEAD;
        dead = true;
        //SpawnRandomBox();

        currentTile.piece = null;
        currentTile.IsFull = false;
        currentTile.walkable = true;

        ArenaManager.Instance.BattleEndCheck(myPieceList);
    }

    void SpawnRandomBox()
    {
        GameObject box = Instantiate(randomBoxObject, transform.position, Quaternion.identity);
        RandomBox randomBox = box.GetComponent<RandomBox>();
        ArenaManager.Instance.fieldManagers[0].chest.CurveMove(randomBox.transform, fieldManager.targetPositions);
        ArenaManager.Instance.fieldManagers[0].chest.SetBoxContents(randomBox, 0);
    }

    //�̵�
    public IEnumerator Move()
    {
        PieceState = State.MOVE;

        //if (path.Count > 0 && canMove)
        if (nextTile != null && canMove)
        {
            canMove = false;
            if (nextTile.IsFull)
            {
                yield return new WaitForSeconds(moveSpeed);
                //StartNextBehavior();
                IdleState();
            }

            Vector3 targetTilePos = new Vector3(nextTile.transform.position.x, transform.position.y, nextTile.transform.position.z);
            transform.DOMove(targetTilePos, moveSpeed).SetEase(ease);

            transform.DOLookAt(nextTile.transform.position, 0.1f);

            currentTile.piece = null;
            currentTile.IsFull = false;
            currentTile.walkable = true;
            currentTile = nextTile;

            currentTile.piece = this;
            currentTile.IsFull = true;
            currentTile.walkable = false;

            nextTile = null;
            canMove = true;

            yield return new WaitForSeconds(moveSpeed);
            StartNextBehavior();
        }
        else
            StartNextBehavior();
    }
    #region �䳢
    public bool isRabbitSynergeActiveCheck;
    public void RabbitJump()
    {
        List<Tile> _neighbor = new List<Tile>();
        int _distance = ArenaManager.Instance.fieldManagers[0].pathFinding.GetDistance(currentTile, target.currentTile);
        Debug.Log(_distance);
        if (_distance <= attackRange)
        {
            _neighbor = ArenaManager.Instance.fieldManagers[0].pathFinding.GetNeighbor(currentTile);
            Vector3[] Jumppath ={ new Vector3(transform.position.x,transform.position.y,transform.position.z),
                                     new Vector3(transform.position.x,transform.position.y + 3f, transform.position.z),
                                     new Vector3(transform.position.x, transform.position.y, transform.position.z) };
            GetComponent<Rigidbody>().DOPath(Jumppath, 2, PathType.CatmullRom, PathMode.Full3D); //��������
        }
        else
        {
            if (ArenaManager.Instance.fieldManagers[0].pathFinding.GetDistance(currentTile, target.currentTile) <= 4)
            {
                _neighbor = ArenaManager.Instance.fieldManagers[0].pathFinding.GetNeighbor(target.currentTile);
                for (int i = 0; i < _neighbor.Count; i++)
                {
                    if (_neighbor[i].IsFull == false)
                    {
                        Vector3 targetTilePos = new Vector3(_neighbor[i].transform.position.x, 0, _neighbor[i].transform.position.z);
                        Vector3 hpos = transform.position + ((targetTilePos - transform.position) / 2);
                        Vector3[] Jumppath = { new Vector3(transform.position.x, transform.position.y, transform.position.z),
                                             new Vector3(hpos.x, hpos.y + 5f, hpos.z),
                                             new Vector3(targetTilePos.x, targetTilePos.y, targetTilePos.z) };
                        GetComponent<Rigidbody>().DOPath(Jumppath, 2, PathType.CatmullRom, PathMode.Full3D).SetEase(rabbitEase); //��������


                        if(currentTile != _neighbor[i])
                        {
                            currentTile.piece = null;
                            currentTile.IsFull = false;
                            currentTile.walkable = true;
                            currentTile = _neighbor[i];

                            currentTile.piece = this;
                            currentTile.IsFull = true;
                            currentTile.walkable = false;
                        }
                        break;
                    }
                }
            }
            else
            {
                NextBehavior();
            }
        }

        isRabbitSynergeActiveCheck = false;



        //var _distance = ArenaManager.Instance.fieldManagers[0].pathFinding.GetDistance(currentTile, target.currentTile);
        //Debug.Log(_distance);
        //List<Tile> _neighbor = new List<Tile>();
        //if (attackRange >= _distance) //���ڸ� ����
        //{
        //    _neighbor = ArenaManager.Instance.fieldManagers[0].pathFinding.GetNeighbor(currentTile);
        //    Vector3[] Jumppath ={ new Vector3(transform.position.x,transform.position.y,transform.position.z),
        //                             new Vector3(transform.position.x,transform.position.y + 3f, transform.position.z),
        //                             new Vector3(transform.position.x, transform.position.y, transform.position.z) };
        //    GetComponent<Rigidbody>().DOPath(Jumppath, 2, PathType.CatmullRom, PathMode.Full3D); //��������
        //}
        //else
        //{
        //    if (_distance <= 4)
        //    {
        //        _neighbor = ArenaManager.Instance.fieldManagers[0].pathFinding.GetNeighbor(target.currentTile);
        //        foreach(var _neighborTile in _neighbor)
        //        {
        //            if (_neighborTile.IsFull == false)
        //            {
        //                Vector3 targetTilePos = new Vector3(path[0].transform.position.x, 1, path[0].transform.position.z);
        //                Vector3 hpos = transform.position + ((targetTilePos - transform.position) / 2);
        //                Vector3[] Jumppath = { new Vector3(transform.position.x, transform.position.y, transform.position.z),
        //                                         new Vector3(hpos.x, hpos.y + 3f, hpos.z),
        //                                         new Vector3(targetTilePos.x, targetTilePos.y, targetTilePos.z) };
        //                GetComponent<Rigidbody>().DOPath(Jumppath, 2, PathType.CatmullRom, PathMode.Full3D); //��������

        //                currentTile.piece = null;
        //                currentTile.IsFull = false;
        //                currentTile = _neighborTile;

        //                currentTile.piece = this;
        //                currentTile.IsFull = true;
        //            }
        //        }
        //    }
        //    else { Move(); return; } 
        //}
        //isRabbitSynergeActiveCheck = false;
        //StartCoroutine(RabbitSplashDamage(_neighbor, 2));
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
    }

    void RsetRabbitStatus()
    {
        if (ArenaManager.Instance.roundType != ArenaManager.RoundType.Battle) return;

        var _rabbitCount = ArenaManager.Instance.fieldManagers[0].animalActiveCount[PieceData.Animal.Rabbit];
        int _activeCount = _rabbitCount / 3;
        this.attackSpeed -= pieceData.attackSpeed[star] * 0.1f * _activeCount;
        this.moveSpeed -= pieceData.moveSpeed[star] * 0.1f * _activeCount;
    }
    #endregion
    #region ������
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

    public IEnumerator NextBehavior()
    {
        yield return new WaitForSeconds(0);
        EnemyCheck();
        if (CheckEnemySurvival(enemyPieceList) && !dead && ArenaManager.Instance.roundType == ArenaManager.RoundType.Battle)
        {
            //ArenaManager.Instance.fieldManagers[0].pathFinding.SetCandidatePath(this, enemyPieceList);
            ArenaManager.Instance.fieldManagers[0].pathFinding.SetTarget(this, enemyPieceList);

            if (target != null)
            {
                if (isRabbitSynergeActiveCheck) { RabbitJump(); }
                if (RangeCheck())
                {
                    if (RangeCheck())
                        AttackState();
                    else
                        StartMove();
                }
                else
                {
                    ArenaManager.Instance.fieldManagers[0].pathFinding.SetCandidatePath(this);
                    StartMove();
                }
            }
            else
            {
                StartNextBehavior();
            }
        }
        else
        {
            PieceState = State.IDLE;
        }
    }
    public void VictoryDacnce()
    {
        StopAllCoroutines();
        //pieceData.ResetPiece(this);
        print(name + "(��)�� �¸��� �� �ߴ� ��.");
        PieceState = State.DANCE;

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
            currentPiece.pieceData.InitialzePiece(currentPiece);
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
            currentPiece.pieceData.InitialzePiece(targetPiece);
            fieldManager.RemoveDPList(currentPiece);
            fieldManager.myFilePieceList.Remove(targetPiece);
            targetPiece.buffList.Clear();
            var _duplicationTargetCheck = fieldManager.myFilePieceList.FirstOrDefault(listPiece => listPiece.pieceName == targetPiece.pieceName);
            if (_duplicationTargetCheck == null) fieldManager.SynergeDecrease(targetPiece); //Minus


            var _duplicationCurrentCheck = fieldManager.myFilePieceList.FirstOrDefault(listPiece => listPiece.pieceName == currentPiece.pieceName);
            if (_duplicationCurrentCheck == null) fieldManager.SynergeIncrease(currentPiece); //Plus
            fieldManager.myFilePieceList.Add(currentPiece);
            fieldManager.AddDPList(currentPiece);
            fieldManager.CalSynerge(currentPiece, targetPiece);
        }  // Change Ready -> Battle
        else if (currentPiece.currentTile.isReadyTile == false && targetPiece.currentTile.isReadyTile == true)
        {
            currentPiece.pieceData.InitialzePiece(currentPiece);
            fieldManager.RemoveDPList(currentPiece);
            fieldManager.myFilePieceList.Remove(currentPiece);
            currentPiece.buffList.Clear();
            var _duplicationCurrentCheck = fieldManager.myFilePieceList.FirstOrDefault(listPiece => listPiece.pieceName == currentPiece.pieceName);
            if (_duplicationCurrentCheck == null) fieldManager.SynergeDecrease(currentPiece); //Minus

            var _duplicationTargetCheck = fieldManager.myFilePieceList.FirstOrDefault(listPiece => listPiece.pieceName == targetPiece.pieceName);
            if (_duplicationTargetCheck == null) fieldManager.SynergeIncrease(targetPiece); //Plus
            fieldManager.myFilePieceList.Add(targetPiece);
            fieldManager.AddDPList(currentPiece);
            fieldManager.CalSynerge(targetPiece, currentPiece);
        }  // Change Battle -> Ready
    }

    #region �����̻�
    public void SetDebuff(string debuff, float time, Piece target = null)
    {
        target = (target == null) ? this.target : target;
        if (target.immune) { Debug.Log("��밡 �����̻� �鿪�Դϴ�."); return; }
        switch (debuff)
        {
            case "Freeze":
                target.freeze = true;
                StartCoroutine(DebuffTimer(target, "Freeze", time));
                break;
            case "Slow":
                target.slow = true;
                StartCoroutine(DebuffTimer(target, "Slow", time));
                break;
            case "Airbone":
                target.airborne = true;
                StartCoroutine(DebuffTimer(target, "Airbone", time));
                break;
            case "Faint":
                target.faint = true;
                StartCoroutine(DebuffTimer(target, "Faint", time));
                break;
            case "Fear":
                target.fear = true;
                StartCoroutine(DebuffTimer(target, "Fear", time));
                break;
            case "Invincible":
                target.invincible = true;
                StartCoroutine(DebuffTimer(target, "Invincible", time));
                break;
            case "Charm":
                target.charm = true;
                StartCoroutine(DebuffTimer(target, "Charm", time));
                break;
            case "Blind":
                target.blind = true;
                StartCoroutine(DebuffTimer(target, "Blind", time));
                break;
            case "Stun":
                target.stun = true;
                StartCoroutine(DebuffTimer(target, "Stun", time));
                break;
        }
    }

    IEnumerator DebuffTimer(Piece target, string debuff, float time)
    {
        yield return new WaitForSeconds(time);
        switch (debuff)
        {
            case "Freeze":
                target.freeze = false;
                break;
            case "Slow":
                target.slow = false;
                break;
            case "Airbone":
                target.airborne = false;
                break;
            case "Faint":
                target.faint = false;
                break;
            case "Fear":
                target.fear = false;
                break;
            case "Invincible":
                target.invincible = false;
                break;
            case "Charm":
                target.charm = false;
                break;
            case "Blind":
                target.blind = false;
                break;
            case "Stun":
                target.stun = false;
                break;
        }
    }
    public void SetFreeze()
    {
        freeze = true;
        //������ ��ġ�� �ٷ� �̵��� ����¡ �ɸ���
    }
    public void SetFreeze(float time)
    {
        freeze = true;
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

    public void SetBlind(float time)
    {
        blind = true;
        Invoke("BlindClear", time);
    }

    void BlindClear()
    {
        blind = false;
    }

    public void SetStun(float time)
    {
        stun = true;
        Invoke("StunClear", time);
    }

    void StunClear()
    {
        stun = false;
    }

    //void 
    #endregion
    public void StartNextBehavior()
    {
        StopAllCoroutines();
        StartCoroutine(NextBehavior());
    }

    public void StartMove()
    {
        StopAllCoroutines();
        StartCoroutine(Move());
    }

    public void StartAttack()
    {
        StopAllCoroutines();
        StartCoroutine(Attack());
    }

    public void StartSkill()
    {
        StopAllCoroutines();
        StartCoroutine(Skill());
    }

    public virtual IEnumerator Attack()
    {
        yield return 0;
        DoAttack();
    }

    public virtual void DoAttack()
    {
        if (target != null)
        {
            print(name + "(��)��" + target.name + "���� �Ϲ� ������ �մϴ�.");
            Damage(attackDamage);
            //mana += 100;
            //currentMana += manaRecovery;
            StartNextBehavior();
        }
        else
        {
            IdleState();
        }
    }

    public void Dead()
    {
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }

    public virtual IEnumerator Skill()
    {
        print(name + "(��)��" + target.name + "���� ��ų�� ����մϴ�.");

        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    #region �ִϸ��̼� �̺�Ʈ (����)
    public void AttackEffect()
    {
        int _count = ArenaManager.Instance.fieldManagers[0].mythActiveCount[this.pieceData.myth];
        int _thresholds = 0;
        switch (pieceData.myth)
        {
            case PieceData.Myth.GreatMountain: _thresholds = 3; break;
            case PieceData.Myth.FrostyWind: _thresholds = 3; break;
            case PieceData.Myth.SandKingdom: _thresholds = 3; break;
            case PieceData.Myth.HeavenGround: _thresholds = 2; break;
            case PieceData.Myth.BurningGround: _thresholds = 2; break;
        }
        if (_count >= _thresholds && pieceState == State.ATTACK)
        {
            GameObject _effect = (isEquip == true) ? weaponAttackEffects : handAttackEffects;
            GameObject _attackEffect = Instantiate(_effect, handAttackEffects.transform.position, Quaternion.identity);
            _attackEffect.SetActive(true); _attackEffect.transform.SetParent(null);
        }
    }
    #endregion
}