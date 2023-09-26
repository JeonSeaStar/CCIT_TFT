using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipmentData", menuName = "Scriptable Object/EquipmentData", order = int.MaxValue)]
[SerializeField] class EquipmentData : ScriptableObject
{
    [SerializeField] FieldManager fieldManager;

    [SerializeField] string pieceName;
    [SerializeField] Sprite piecePortrait;

    [SerializeField] float health;
    [SerializeField] float mana;
    [SerializeField] float attackDamage;
    [SerializeField] float abilityPower;
    [SerializeField] float armor;
    [SerializeField] float magicResist;
    [SerializeField] float attackSpeed;
    [SerializeField] float criticalChance;
    [SerializeField] float criticalDamage;
    [SerializeField] float attackRange;

    [TextArea] [SerializeField] string Explanation;

    public GameObject equipmentPrefab;

    public void InitialzePiece(Equipment equipment)
    {
        equipment.pieceName = pieceName;
        equipment.piecePortrait = piecePortrait;
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


    void Awake() => fieldManager = ArenaManager.instance.fm[0];

    public void InputChest(Transform spaceTransform)
    {
        //transform.DoMove(spaceTransform, 2f);
    }
}
