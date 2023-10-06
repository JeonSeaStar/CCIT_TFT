using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff Data", menuName = "Scriptable Object/Buff Data", order = int.MaxValue)]
public class Buff : ScriptableObject
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

    void Effect()
    {
        Debug.Log("부가 효과는 이 곳에");
    }
}
