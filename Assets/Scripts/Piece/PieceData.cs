using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PieceData", menuName = "Scriptable Object/Piece Data", order = int.MaxValue)]
public class PieceData : ScriptableObject
{
    public string pieceName;
    public Sprite piecePortrait;
    public GameObject piecePrefab;

    public float[] health           = new float[3];          //ü��
    public float[] mana             = new float[3];            //����
    public float[] manaRecovery     = new float[3];    //���� ȸ����

    public float[] attackDamage     = new float[3];    //�⺻ ���ݷ�
    public float[] abilityPower     = new float[3];   //���� ��ų ���ݷ�

    public float[] armor            = new float[3];//����
    public float[] magicResist      = new float[3];//���� ���׷�

    public float[] attackSpeed      = new float[3];     //���ݼӵ�
    public float[] criticalChance   = new float[3];  //ũ��Ƽ�� Ȯ��
    public float[] criticalDamage   = new float[3];  //ũ��Ƽ�� ����
    public int[] attackRange        = new int[3];       //���ݹ���
    public float[] bloodBrain       = new float[3];      //������
    public float[] moveSpeed        = new float[3];     //�̵��ӵ�
    public Buff buff;

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
        piece.health = health[piece.grade];
        piece.mana = mana[piece.grade];
        piece.attackDamage = attackDamage[piece.grade];
        piece.abilityPower = abilityPower[piece.grade];
        piece.armor = armor[piece.grade];
        piece.magicResist = magicResist[piece.grade];
        piece.attackSpeed = attackSpeed[piece.grade];
        piece.criticalChance = criticalChance[piece.grade];
        piece.criticalDamage = criticalDamage[piece.grade];
        piece.attackRange = attackRange[piece.grade];
        piece.bloodBrain = bloodBrain[piece.grade];

        piece.shield = 0;
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

    public void CalculateBuff(Piece piece , BuffData buffData, bool isPlus = true)
    {
        int _star = piece.star; //0, 1, 2

        if (isPlus)
        {
            piece.health += CalculateStatus(piece.pieceData.health[_star], buffData.health, buffData.percentHealth);
            piece.mana += CalculateStatus(piece.pieceData.mana[_star], buffData.mana, buffData.percentMana);
            piece.attackDamage += CalculateStatus(piece.pieceData.attackDamage[_star], buffData.attackDamage, buffData.percentAttackDamage);
            piece.abilityPower += CalculateStatus(piece.pieceData.abilityPower[_star], buffData.abilityPower, buffData.percentAbilityPower);
            piece.armor += CalculateStatus(piece.pieceData.armor[_star], buffData.armor, buffData.percentArmor);
            piece.magicResist += CalculateStatus(piece.pieceData.magicResist[_star], buffData.magicResist, buffData.percentMagicResist);
            piece.attackSpeed += CalculateStatus(piece.pieceData.attackSpeed[_star], buffData.attackSpeed, buffData.percentAttackSpeed);
            piece.criticalChance += CalculateStatus(piece.pieceData.criticalChance[_star], buffData.criticalChance, buffData.percentCriticalChance);
            piece.criticalDamage += CalculateStatus(piece.pieceData.criticalDamage[_star], buffData.criticalDamage, buffData.percentCriticalDamage);
            piece.attackRange += CalculateStatus(piece.pieceData.attackRange[_star], buffData.attackRange, buffData.percentAttackRange);

            piece.shield += CalculateStatus(piece.pieceData.health[_star], buffData.shield, buffData.percentShield);
        }
        else if(isPlus == false)
        {
            piece.health -= CalculateStatus(piece.pieceData.health[_star], buffData.health, buffData.percentHealth);
            piece.mana -= CalculateStatus(piece.pieceData.mana[_star], buffData.mana, buffData.percentMana);
            piece.attackDamage -= CalculateStatus(piece.pieceData.attackDamage[_star], buffData.attackDamage, buffData.percentAttackDamage);
            piece.abilityPower -= CalculateStatus(piece.pieceData.abilityPower[_star], buffData.abilityPower, buffData.percentAbilityPower);
            piece.armor -= CalculateStatus(piece.pieceData.armor[_star], buffData.armor, buffData.percentArmor);
            piece.magicResist -= CalculateStatus(piece.pieceData.magicResist[_star], buffData.magicResist, buffData.percentMagicResist);
            piece.attackSpeed -= CalculateStatus(piece.pieceData.attackSpeed[_star], buffData.attackSpeed, buffData.percentAttackSpeed);
            piece.criticalChance -= CalculateStatus(piece.pieceData.criticalChance[_star], buffData.criticalChance, buffData.percentCriticalChance);
            piece.criticalDamage -= CalculateStatus(piece.pieceData.criticalDamage[_star], buffData.criticalDamage, buffData.percentCriticalDamage);
            piece.attackRange -= CalculateStatus(piece.pieceData.attackRange[_star], buffData.attackRange, buffData.percentAttackRange);

            piece.shield -= CalculateStatus(piece.pieceData.health[_star], buffData.shield, buffData.percentShield);
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
