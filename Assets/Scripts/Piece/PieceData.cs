using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PieceData", menuName = "Scriptable Object/Piece Data", order = int.MaxValue)]
public class PieceData : ScriptableObject
{
    public string pieceName;
    public Sprite piecePortrait;
    public GameObject piecePrefab;

    public float defaultHealth;          //ü��
    public float defaultMana;            //����
    public float defaultManaRecovery;    //���� ȸ����

    public float defaultAttackPower;     //�⺻ ���ݷ�
    public float defaultAttackDamage;    //���� ���ݷ�
    public float defaultAbilityPower;    //���� ��ų ���ݷ�

    public float defaultArmor;           //����att
    public float defaultMagicResist;     //���� ���׷�

    public float defaultAttackSpeed;     //���ݼӵ�
    public float defaultCriticalChance;  //ũ��Ƽ�� Ȯ��
    public float defaultCriticalDamage;  //ũ��Ƽ�� ����
    public int defaultAttackRange;       //���ݹ���

    //�䳢 ���� ���� ������ ��ġ �Ķ�Ƽ�� �߰� �ʿ� Ex) JumpDemage
    //��ų ���� �ð� �Ķ���� �߰� �ʿ�

    public enum Myth //��Ͱ� ȯ�� ��� ��ȭ
    {
        None = -1, 
        GreatMountain,      //������ �� (�׸���)
        FrostyWind,         //�����ٶ�  (������)
        SandKingdom,        //�𷡿ձ�  (����Ʈ)
        HeavenGround,       //õ���    (õ��)
        BurningGround,      //��Ÿ�� �� (�Ǹ�)
        Max
    }
    public enum Animal //�ɷ�ġ ���
    {
        None = -1,
        Hamster,            //�ܽ��� (�뷮�߻�)
        Cat,                //����� (������� ����)
        Dog,                //������ (��������)
        Frog,               //������ (�Ų��Ų�)
        Rabbit,             //�䳢   (���ѱ���)
        Max 
    }
    public enum United //Ư���� �ɷ� �߰�
    {
        None,           
        UnderWorld,         //�Ʒ� ���� => (�ϵ���, ��, �ƴ���, ���ø���)
        Faddist,            //��������  => (�츣�޽�, ���콺, ��Ű, �����)
        WarMachine,         //������  => (�Ʒ���, ���׳�, ��Ű��, ����Ʈ)
        Creature,           //����      => (�̳�Ÿ��ν�, �޵λ�, ��Ÿ��ν�, �׸���, �����, ����Ʈ, �丣������, �渮��, �̶�, ����ũ��)
        MAX 
    }
    public Myth myth = Myth.None;
    public Animal animal = Animal.None;
    public United united = United.None;

    public void InitialzePiece(Piece piece)
    {
        piece.pieceName = pieceName;
        piece.piecePortrait = piecePortrait;
        piece.defaultHealth = defaultHealth;
        piece.defaultMana = defaultMana;
        piece.manaRecovery = defaultManaRecovery;
        piece.defaultAttackPower = defaultAttackDamage;
        piece.defaultAbilityPower = defaultAbilityPower;
        piece.defaultArmor = defaultArmor;
        piece.defaultMagicResist = defaultMagicResist;
        piece.defaultAttackSpeed = defaultAttackSpeed;
        piece.defaultCriticalChance = defaultCriticalChance;
        piece.defaultCriticalDamage = defaultCriticalDamage;
        piece.defaultAttackRange = defaultAttackRange;
        //piece.mythology = mythology;
        //piece.species = species;
        //piece.plusSynerge = plusSynerge;
    }

    void CalculateEquipments(Piece piece)
    {
        foreach (var item in piece.Equipments)
        {

        }
    }
}
