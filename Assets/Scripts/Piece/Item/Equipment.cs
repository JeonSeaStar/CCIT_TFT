using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Equipment : MonoBehaviour
{
    public EquipmentData equipmentData;

    public string equipmentName;
    public Sprite equipmentPortrait;

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

    //Test
    public Vector3 originPos;

    [TextArea] public string explanation;

    public virtual void EquipmentEffect()
    {

    }
}
