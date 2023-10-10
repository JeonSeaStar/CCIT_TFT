using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff Data", menuName = "Scriptable Object/Buff Data", order = int.MaxValue)]
public class BuffData : ScriptableObject
{
    [Header("Buff Name")]
    public string buffName;
    [Space(10)]

    [Header("Buff Status")]
    public bool percentHealth;
    public bool percentMana;
    public bool percentShield; //추가
    public bool percentAttackDamage;
    public bool percentAbilityPower;
    public bool percentArmor;
    public bool percentMagicResist;
    public bool percentAttackSpeed;
    public bool percentMoveSpeed; //추가
    public bool percentCriticalChance;
    public bool percentCriticalDamage;
    public bool percentAttackRange;

    public float health;
    public float mana;
    public float shield;
    public float attackDamage;
    public float abilityPower;
    public float armor;
    public float magicResist;
    public float attackSpeed;
    public float moveSpeed;
    public float criticalChance;
    public float criticalDamage;
    public float attackRange;
}
