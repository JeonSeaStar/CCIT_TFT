using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff Data", menuName = "Scriptable Object/Buff Data", order = int.MaxValue)]
public abstract class BuffData : ScriptableObject
{
    [Header("Buff Name")]
    public string buffName;
    [Space(10)]

    [Header("Buff Status")]
    public bool percentHealth;
    public bool percentMana;
    public bool percentShield; //�߰�
    public bool percentAttackDamage;
    public bool percentAbilityPower;
    public bool percentAbilityPowerCoefficient;
    public bool percentArmor;
    public bool percentMagicResist;
    public bool percentAttackSpeed;
    public bool percentMoveSpeed; //�߰�
    public bool percentCriticalChance;
    public bool percentCriticalDamage;
    public bool percentAttackRange;

    public float health;
    public float mana;
    public float shield;
    public float attackDamage;
    public float abilityPower;
    public float abilityPowerCoefficient;
    public float armor;
    public float magicResist;
    public float attackSpeed;
    public float moveSpeed;
    public float criticalChance;
    public float criticalDamage;
    public float attackRange;

    public bool haveDirectEffect;
    public virtual void DirectEffect(Piece piece, bool isAdd) { }

    public virtual void BattleStartEffect(bool isAdd) { }

    protected Coroutine coroutine;
    public virtual void CoroutineEffect() { }

    //public virtual void sCoroutineEffect(bool isRoundEnd) { }
}
