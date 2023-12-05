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
    }; //등급 //몇성

    public float[] health = new float[3];          //체력
    public float[] mana = new float[3];            //마나
    public float currentMana;                      //현재 마나
    public float[] manaRecovery = new float[3];    //마나 회복력

    public float[] attackDamage = new float[3];    //기본 공격력
    public float[] abilityPower = new float[3];   //최종 스킬 공격력
    public float[] abilityPowerCoefficient = new float[3];    //스킬 계수

    public float[] armor = new float[3];//방어력
    public float[] magicResist = new float[3];//마법 저항력

    public float[] attackSpeed = new float[3];     //공격속도
    public float[] criticalChance = new float[3];  //크리티컬 확률
    public float[] criticalDamage = new float[3];  //크리티컬 배율
    public int[] attackRange = new int[3];       //공격범위
    public float[] bloodBrain = new float[3];      //흡협률
    public float[] moveSpeed = new float[3];     //이동속도
    public Buff buff;

    public Sprite skilSprite;
    public string skillName;
    [TextArea] public string skillExplain;

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
        piece.immune = false; //상태면역
        piece.freeze = false;
        piece.slow = false;
        piece.airborne = false;
        piece.faint = false;
        piece.fear = false;
        piece.invincible = false;
        piece.charm = false; //매혹
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
