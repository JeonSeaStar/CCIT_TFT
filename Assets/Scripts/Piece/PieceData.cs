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

    //즉사 구현을 위한 생존 시간 파라미터 추가 필요
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
