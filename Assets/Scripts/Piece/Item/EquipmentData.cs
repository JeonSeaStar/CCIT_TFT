using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentData", menuName = "Scriptable Object/EquipmentData", order = int.MaxValue)]
public class EquipmentData : ScriptableObject
{
    public FieldManager fieldManager;

    public string pieceName;
    public Sprite piecePortrait;

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
        equipment.armor = armor;
        equipment.magicResist = magicResist;
        equipment.attackSpeed = attackSpeed;
        equipment.criticalChance = criticalChance;
        equipment.criticalDamage = criticalDamage;
        equipment.attackRange = attackRange;
    }


    void Awake() => fieldManager = ArenaManager.Instance.fieldManagers[0];

    public void InputChest(Transform spaceTransform)
    {
        //transform.DoMove(spaceTransform, 2f);
    }
}
