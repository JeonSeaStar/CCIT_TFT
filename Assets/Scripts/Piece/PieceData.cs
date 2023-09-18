using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PieceData", menuName = "Scriptable Object/Piece Data", order = int.MaxValue)]
public class PieceData : ScriptableObject
{
    [SerializeField] string pieceName;
    [SerializeField] Sprite piecePortrait;

    [SerializeField] float health;
    [SerializeField] float mana;
    [SerializeField] float manaRecovery;
    [SerializeField] float attackDamage;
    [SerializeField] float abilityPower;
    [SerializeField] float armor;
    [SerializeField] float magicResist;
    [SerializeField] float attackSpeed;
    [SerializeField] float criticalChance;
    [SerializeField] float criticalDamage;
    [SerializeField] int attackRange;

    //��� ������ ���� ���� �ð� �Ķ���� �߰� �ʿ�
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
        piece.health = health;
        piece.mana = mana;
        piece.manaRecovery = manaRecovery;
        piece.attackDamage = attackDamage;
        piece.abilityPower = abilityPower;
        piece.armor = armor;
        piece.magicResist = magicResist;
        piece.attackSpeed = attackSpeed;
        piece.criticalChance = criticalChance;
        piece.criticalDamage = criticalDamage;
        piece.attackRange = attackRange;
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
