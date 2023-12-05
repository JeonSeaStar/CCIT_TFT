using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PieceData", menuName = "Scriptable Object/Piece Data", order = int.MaxValue)]
public class PieceData : ScriptableObject
{
    public string pieceName;
    public Sprite piecePortrait;
    public GameObject piecePrefab;

    public int grade;
    public int star;


    [HideInInspector]
    public int[,] cost =
    {
        { 1, 3, 9},
        { 2, 5, 17},
        { 3, 8, 26},
        { 4, 11, 35 }
    }; //��� //�

    public float[] health = new float[3];          //ü��
    public float[] mana = new float[3];            //����
    public float currentMana;                      //���� ����
    public float[] manaRecovery = new float[3];    //���� ȸ����

    public float[] attackDamage = new float[3];    //�⺻ ���ݷ�
    public float[] abilityPower = new float[3];   //���� ��ų ���ݷ�
    public float[] abilityPowerCoefficient = new float[3];    //��ų ���

    public float[] armor = new float[3];//����
    public float[] magicResist = new float[3];//���� ���׷�

    public float[] attackSpeed = new float[3];     //���ݼӵ�
    public float[] criticalChance = new float[3];  //ũ��Ƽ�� Ȯ��
    public float[] criticalDamage = new float[3];  //ũ��Ƽ�� ����
    public int[] attackRange = new int[3];       //���ݹ���
    public float[] bloodBrain = new float[3];      //������
    public float[] moveSpeed = new float[3];     //�̵��ӵ�
    public Buff buff;

    public Sprite skilSprite;
    public string skillName;
    [TextArea] public string skillExplain;

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
        Debug.Log(piece.star);
        Debug.Log(health[piece.star]);
        piece.pieceName = pieceName;
        piece.piecePortrait = piecePortrait;
        piece.health = health[piece.star];
        piece.mana = mana[piece.star];
        piece.manaRecovery = manaRecovery[piece.star];
        piece.attackDamage = attackDamage[piece.star];
        piece.abilityPower = abilityPower[piece.star];
        piece.abilityPowerCoefficient = abilityPowerCoefficient[piece.star];
        piece.armor = armor[piece.star];
        piece.magicResist = magicResist[piece.star];
        piece.attackSpeed = attackSpeed[piece.star];
        piece.criticalChance = criticalChance[piece.star];
        piece.criticalDamage = criticalDamage[piece.star];
        piece.attackRange = attackRange[piece.star];
        piece.bloodBrain = bloodBrain[piece.star];

        piece.shield = 0;
    }

    public void ResetPiece(Piece piece)
    {
        piece.pieceName = pieceName;
        piece.piecePortrait = piecePortrait;
        piece.health = health[piece.star];
        piece.mana = mana[piece.star];
        piece.attackDamage = attackDamage[piece.star];
        piece.abilityPower = abilityPower[piece.star];
        piece.attackSpeed = attackSpeed[piece.star];

        piece.shield = 0;
        piece.stun = false;
        piece.immune = false; //���¸鿪
        piece.freeze = false;
        piece.slow = false;
        piece.airborne = false;
        piece.faint = false;
        piece.fear = false;
        piece.invincible = false;
        piece.charm = false; //��Ȥ
        piece.blind = false;
        piece.stun = false;
    }

    void CalculateEquipments(Piece piece)
    {
        float health = 0;
        float mana = 0;
        float attackDamage = 0;
        float abilityPower = 0;
        float abilityPowerCoefficient = 0;
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
            abilityPowerCoefficient += CalculateStatus(piece.abilityPowerCoefficient, item.abilityPowerCoefficient, item.percentAbilityPowerCoefficient);
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
        float abilityPowerCoefficient = 0;
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
            abilityPowerCoefficient += CalculateStatus(piece.abilityPowerCoefficient, item.abilityPowerCoefficient, item.percentAbilityPowerCoefficient);
            armor += CalculateStatus(piece.armor, item.armor, item.percentArmor);
            magicResist += CalculateStatus(piece.magicResist, item.magicResist, item.percentMagicResist);
            attackSpeed += CalculateStatus(piece.attackSpeed, item.attackSpeed, item.percentAttackSpeed);
            criticalChance += CalculateStatus(piece.criticalChance, item.criticalChance, item.percentCriticalChance);
            criticalDamage += CalculateStatus(piece.criticalDamage, item.criticalDamage, item.percentCriticalDamage);
            attackRange += CalculateStatus(piece.attackRange, item.attackRange, item.percentAttackRange);
        }
    }

    public void CalculateBuff(Piece piece, BuffData buffData, bool isPlus = true)
    {
        int _star = piece.star; //0, 1, 2

        if (isPlus)
        {
            piece.health += CalculateStatus(piece.pieceData.health[_star], buffData.health, buffData.percentHealth);
            piece.mana += CalculateStatus(piece.pieceData.mana[_star], buffData.mana, buffData.percentMana);
            piece.attackDamage += CalculateStatus(piece.pieceData.attackDamage[_star], buffData.attackDamage, buffData.percentAttackDamage);
            piece.abilityPower += CalculateStatus(piece.pieceData.abilityPower[_star], buffData.abilityPower, buffData.percentAbilityPower);
            piece.abilityPowerCoefficient += CalculateStatus(piece.pieceData.abilityPowerCoefficient[_star], buffData.abilityPowerCoefficient, buffData.percentAbilityPowerCoefficient);
            piece.armor += CalculateStatus(piece.pieceData.armor[_star], buffData.armor, buffData.percentArmor);
            piece.magicResist += CalculateStatus(piece.pieceData.magicResist[_star], buffData.magicResist, buffData.percentMagicResist);
            piece.attackSpeed += CalculateStatus(piece.pieceData.attackSpeed[_star], buffData.attackSpeed, buffData.percentAttackSpeed);
            piece.criticalChance += CalculateStatus(piece.pieceData.criticalChance[_star], buffData.criticalChance, buffData.percentCriticalChance);
            piece.criticalDamage += CalculateStatus(piece.pieceData.criticalDamage[_star], buffData.criticalDamage, buffData.percentCriticalDamage);
            piece.attackRange += CalculateStatus(piece.pieceData.attackRange[_star], buffData.attackRange, buffData.percentAttackRange);

            piece.shield += CalculateStatus(piece.pieceData.health[_star], buffData.shield, buffData.percentShield);
        }
        else if (isPlus == false)
        {
            piece.health -= CalculateStatus(piece.pieceData.health[_star], buffData.health, buffData.percentHealth);
            piece.mana -= CalculateStatus(piece.pieceData.mana[_star], buffData.mana, buffData.percentMana);
            piece.attackDamage -= CalculateStatus(piece.pieceData.attackDamage[_star], buffData.attackDamage, buffData.percentAttackDamage);
            piece.abilityPower -= CalculateStatus(piece.pieceData.abilityPower[_star], buffData.abilityPower, buffData.percentAbilityPower);
            piece.abilityPowerCoefficient += CalculateStatus(piece.pieceData.abilityPowerCoefficient[_star], buffData.abilityPowerCoefficient, buffData.percentAbilityPowerCoefficient);
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
