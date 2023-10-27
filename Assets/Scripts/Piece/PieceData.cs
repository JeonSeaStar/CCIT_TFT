using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PieceData", menuName = "Scriptable Object/Piece Data", order = int.MaxValue)]
public class PieceData : ScriptableObject
{
    public string pieceName;
    public Sprite piecePortrait;
    public GameObject piecePrefab;

    public float[] health           = new float[3];          //체력
    public float[] mana             = new float[3];            //마나
    public float[] manaRecovery     = new float[3];    //마나 회복력

    public float[] attackPower      = new float[3];    //기본 공격력
    public float[] abilityPower     = new float[3];   //최종 스킬 공격력

    public float[] armor            = new float[3];//방어력
    public float[] magicResist      = new float[3];//마법 저항력

    public float[] attackSpeed      = new float[3];     //공격속도
    public float[] criticalChance   = new float[3];  //크리티컬 확률
    public float[] criticalDamage   = new float[3];  //크리티컬 배율
    public int[] attackRange        = new int[3];       //공격범위
    public float[] bloodBrain       = new float[3];      //흡협률
    public Buff buff;

    //토끼 전용 고정 데미지 수치 파라티터 추가 필요 Ex) JumpDemage
    //스킬 지속 시간 파라미터 추가 필요

    public enum Myth //기믹과 환경 요소 변화
    {
        None = -1,
        GreatMountain,      //위대한 산 (그리스)
        FrostyWind,         //서리바람  (북유럽)
        SandKingdom,        //모래왕국  (이집트)
        HeavenGround,       //천상계    (천사)
        BurningGround,      //불타는 땅 (악마)
        Max
    }
    public enum Animal //능력치 상승
    {
        None = -1,
        Hamster,            //햄스터 (대량발생)
        Cat,                //고양이 (고양이의 보은)
        Dog,                //강아지 (서열정리)
        Frog,               //개구리 (매끈매끈)
        Rabbit,             //토끼   (깡총깡총)
        Max
    }
    public enum United //특별한 능력 추가
    {
        None,
        UnderWorld,         //아래 세계 => (하데스, 헬, 아누비스, 오시리스)
        Faddist,            //변덕쟁이  => (헤르메스, 제우스, 로키, 루시퍼)
        WarMachine,         //전쟁기계  => (아레스, 아테네, 발키리, 네이트)
        Creature,           //괴물      => (미노타우로스, 메두사, 켄타우로스, 그리핀, 드라우그, 수르트, 요르문간드, 펜리르, 미라, 스핑크스)
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

    public void CalculateBuff(Piece piece , BuffData buffData)
    {
        piece.health += CalculateStatus(piece.health, buffData.health, buffData.percentHealth);
        piece.mana += CalculateStatus(piece.mana, buffData.mana, buffData.percentMana);
        piece.attackDamage += CalculateStatus(piece.attackDamage, buffData.attackDamage, buffData.percentAttackDamage);
        piece.abilityPower += CalculateStatus(piece.abilityPower, buffData.abilityPower, buffData.percentAbilityPower);
        piece.armor += CalculateStatus(piece.armor, buffData.armor, buffData.percentArmor);
        piece.magicResist += CalculateStatus(piece.magicResist, buffData.magicResist, buffData.percentMagicResist);
        piece.attackSpeed += CalculateStatus(piece.attackSpeed, buffData.attackSpeed, buffData.percentAttackSpeed);
        piece.criticalChance += CalculateStatus(piece.criticalChance, buffData.criticalChance, buffData.percentCriticalChance);
        piece.criticalDamage += CalculateStatus(piece.criticalDamage, buffData.criticalDamage, buffData.percentCriticalDamage);
        piece.attackRange += CalculateStatus(piece.attackRange, buffData.attackRange, buffData.percentAttackRange);
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
