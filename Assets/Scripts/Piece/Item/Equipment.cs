using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Equipment : MonoBehaviour
{
    public EquipmentData equipmentData;

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

    [TextArea] public string explanation;

    public virtual void EquipmentEffect()
    {

    }
}
