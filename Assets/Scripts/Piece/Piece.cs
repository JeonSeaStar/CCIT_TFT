using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Piece;
using static PieceData;

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

    [SerializeField] GameObject randomBoxObject;

    void Awake()
    {
        pieceData.InitialzePiece(this);
    }

    private void Start()
    {
        // 초기화 순서 상 Start 에서 넣어주세요.
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
        for(int i = 0; i < fieldManager.myFilePieceList.Count;i++)
        {
            if (fieldManager.myFilePieceList[i].name == currentPiece.name) return;
        }

        var _currentPiece = currentPiece.GetComponent<PieceControl>();

        if (_currentPiece.currentTile.isReadyTile == true && _currentPiece.targetTile.isReadyTile == false)
        {
            // Plus
            fieldManager.SynergeMythology[currentPiece.pieceData.mythology]++;
            fieldManager.SynergeSpecies[currentPiece.pieceData.species]++;
            fieldManager.SynergePlusSynerge[currentPiece.pieceData.plusSynerge]++;

            fieldManager.myFilePieceList.Add(currentPiece);
            return;
        }
        if (_currentPiece.currentTile.isReadyTile == false && _currentPiece.targetTile.isReadyTile == true)
        {
            // Minus
            fieldManager.SynergeMythology[currentPiece.pieceData.mythology]--;
            fieldManager.SynergeSpecies[currentPiece.pieceData.species]--;
            fieldManager.SynergePlusSynerge[currentPiece.pieceData.plusSynerge]--;
            
            fieldManager.myFilePieceList.Remove(currentPiece);
        }
    }

    public void SetPiece(Piece currentPiece, Piece targetPiece)
    {
        var _currentPiece = currentPiece.GetComponent<PieceControl>();
        var _targetPiece = targetPiece.GetComponent<PieceControl>();

        if (_currentPiece.currentTile.isReadyTile == true && _targetPiece.currentTile.isReadyTile == true) return;
        if (_currentPiece.currentTile.isReadyTile == false && _targetPiece.currentTile.isReadyTile == false) return;
        if (_currentPiece.currentTile.isReadyTile == true && _targetPiece.currentTile.isReadyTile == false)
        {
            //for(int i =0; i < fieldManager.)

            fieldManager.SynergeMythology[currentPiece.pieceData.mythology]++;
            fieldManager.SynergeSpecies[currentPiece.pieceData.species]++;
            fieldManager.SynergePlusSynerge[currentPiece.pieceData.plusSynerge]++;

            fieldManager.SynergeMythology[targetPiece.pieceData.mythology]--;
            fieldManager.SynergeSpecies[targetPiece.pieceData.species]--;
            fieldManager.SynergePlusSynerge[targetPiece.pieceData.plusSynerge]--;
        }
        if (_currentPiece.currentTile.isReadyTile == false && _targetPiece.currentTile.isReadyTile == true)
        {
            fieldManager.SynergeMythology[currentPiece.pieceData.mythology]--;
            fieldManager.SynergeSpecies[currentPiece.pieceData.species]--;
            fieldManager.SynergePlusSynerge[currentPiece.pieceData.plusSynerge]--;

            fieldManager.SynergeMythology[targetPiece.pieceData.mythology]++;
            fieldManager.SynergeSpecies[targetPiece.pieceData.species]++;
            fieldManager.SynergePlusSynerge[targetPiece.pieceData.plusSynerge]++;
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

    void CalculateSynerge()
    {

    }
}
