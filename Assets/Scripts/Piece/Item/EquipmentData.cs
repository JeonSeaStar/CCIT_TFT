using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentData", menuName = "Scriptable Object/EquipmentData", order = int.MaxValue)]
public class EquipmentData : ScriptableObject
{
    public Chest.EquipmentType equipmentType;

    public string pieceName;
    public Sprite piecePortrait;

    public bool percentHealth;
    public bool percentMana;
    public bool percentAttackDamage;
    public bool percentAbilityPower;
    public bool percentAbilityPowerCoefficient;
    public bool percentArmor;
    public bool percentMagicResist;
    public bool percentAttackSpeed;
    public bool percentCriticalChance;
    public bool percentCriticalDamage;
    public bool percentAttackRange;

    public float health;
    public float mana;
    public float attackDamage;
    public float abilityPower;
    public float abilityPowerCoefficient;
    public float armor;
    public float magicResist;
    public float attackSpeed;
    public float criticalChance;
    public float criticalDamage;
    public float attackRange;

    [TextArea] public string Explanation;

    public GameObject equipmentPrefab;

    public void InitialzePiece(Equipment equipment)
    {
        equipment.equipmentName = pieceName;
        equipment.equipmentPortrait = piecePortrait;
        equipment.health = health;
        equipment.mana = mana;
        equipment.attackDamage = attackDamage;
        equipment.abilityPower = abilityPower;
        equipment.abilityPowerCoefficient = abilityPowerCoefficient;
        equipment.armor = armor;
        equipment.magicResist = magicResist;
        equipment.attackSpeed = attackSpeed;
        equipment.criticalChance = criticalChance;
        equipment.criticalDamage = criticalDamage;
        equipment.attackRange = attackRange;
    }
}
