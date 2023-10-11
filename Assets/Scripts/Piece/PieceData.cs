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

    public float defaultArmor;           //����
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
        //piece.pieceName = pieceName;
        //piece.piecePortrait = piecePortrait;
        //piece.defaultHealth = defaultHealth;
        //piece.defaultMana = defaultMana;
        //piece.manaRecovery = defaultManaRecovery;
        //piece.defaultAttackPower = defaultAttackDamage;
        //piece.defaultAbilityPower = defaultAbilityPower;
        //piece.defaultArmor = defaultArmor;
        //piece.defaultMagicResist = defaultMagicResist;
        //piece.defaultAttackSpeed = defaultAttackSpeed;
        //piece.defaultCriticalChance = defaultCriticalChance;
        //piece.defaultCriticalDamage = defaultCriticalDamage;
        //piece.defaultAttackRange = defaultAttackRange;


        //piece.mythology = mythology;
        //piece.species = species;
        //piece.plusSynerge = plusSynerge;
    }

    void CalculateEquipments(Piece piece)
    {
        float health = 0;
        float mana = 0;
        float attackDamage = 0;
        float abilityPower = 0;
        float armor = 0;
        float magicResist = 0;
        float attackSpeed = 0;
        float criticalChance = 0;
        float criticalDamage = 0;
        float attackRange = 0;

        foreach (var item in piece.equipmentDatas)
        {
            health += CalculateStatus(piece.health, item.health, item.percentHealth);
            mana += CalculateStatus(piece.mana, item.mana, item.percentMana);
            attackDamage += CalculateStatus(piece.attackDamage, item.attackDamage, item.percentAttackDamage);
            abilityPower += CalculateStatus(piece.abilityPower, item.abilityPower, item.percentAbilityPower);
            armor += CalculateStatus(piece.armor, item.armor, item.percentArmor);
            magicResist += CalculateStatus(piece.magicResist, item.magicResist, item.percentMagicResist);
            attackSpeed += CalculateStatus(piece.attackSpeed, item.attackSpeed, item.percentAttackSpeed);
            criticalChance += CalculateStatus(piece.criticalChance, item.criticalChance, item.percentCriticalChance);
            criticalDamage += CalculateStatus(piece.criticalDamage, item.criticalDamage, item.percentCriticalDamage);
            attackRange += CalculateStatus(piece.attackRange, item.attackRange, item.percentAttackRange);
        }
    }

    void CalculateBuff(Piece piece)
    {
        float health = 0;
        float mana = 0;
        float attackDamage = 0;
        float abilityPower = 0;
        float armor = 0;
        float magicResist = 0;
        float attackSpeed = 0;
        float criticalChance = 0;
        float criticalDamage = 0;
        float attackRange = 0;

        foreach (var item in piece.buffList)
        {
            health += CalculateStatus(piece.health, item.health, item.percentHealth);
            mana += CalculateStatus(piece.mana, item.mana, item.percentMana);
            attackDamage += CalculateStatus(piece.attackDamage, item.attackDamage, item.percentAttackDamage);
            abilityPower += CalculateStatus(piece.abilityPower, item.abilityPower, item.percentAbilityPower);
            armor += CalculateStatus(piece.armor, item.armor, item.percentArmor);
            magicResist += CalculateStatus(piece.magicResist, item.magicResist, item.percentMagicResist);
            attackSpeed += CalculateStatus(piece.attackSpeed, item.attackSpeed, item.percentAttackSpeed);
            criticalChance += CalculateStatus(piece.criticalChance, item.criticalChance, item.percentCriticalChance);
            criticalDamage += CalculateStatus(piece.criticalDamage, item.criticalDamage, item.percentCriticalDamage);
            attackRange += CalculateStatus(piece.attackRange, item.attackRange, item.percentAttackRange);
        }
    }

    float CalculateStatus(float target, float value, bool percent)
    {
        float result = 0;

        if (percent)
        {
            result = target / 100 * value;
        }
        else
            result += value;

        return result;
    }
}
