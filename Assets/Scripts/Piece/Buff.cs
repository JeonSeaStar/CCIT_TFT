using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Buff : MonoBehaviour
{
    public bool percentHealth;
    public bool percentMana;
    public bool percentAttackDamage;
    public bool percentAbilityPower;
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
    public float armor;
    public float magicResist;
    public float attackSpeed;
    public float criticalChance;
    public float criticalDamage;
    public float attackRange;
}
