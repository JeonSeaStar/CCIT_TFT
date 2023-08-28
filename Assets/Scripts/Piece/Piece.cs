using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    public PieceData pieceData;

    public string pieceName;
    public Sprite piecePortrait;

    public List<Synerge> synerges;
    public List<Equipment> Equipments;
    //public int pieceGrade = 1;
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
    public float attackRange;

    public string owner;
    public bool isOwned;

    void Awake()
    {
        pieceData.InitialzePiece(this);
    }

    public void Owned()
    {
        isOwned = true;
        FieldManager.instance.privatePieceCount[FieldManager.instance.FindPieceList(this)].PieceCountUp(this);
    }

    protected virtual void Attack()
    {
        print("�Ϲ� ������ �մϴ�.");
    }

    protected virtual void Skill()
    {
        print("��ų�� ����մϴ�.");
    }

    void Dead()
    {
        print("ü���� 0 ���ϰ� �Ǿ� ���.");
        DestroyPiece();
    }

    public void DestroyPiece()
    {
        FieldManager.instance.privatePieceCount[FieldManager.instance.FindPieceList(this)].PieceCountDown(this);
        Destroy(gameObject);
    }
}
