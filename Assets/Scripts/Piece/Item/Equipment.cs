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

    public Piece targetPiece;

    //Test
    public Vector3 originalPosition = new Vector3(0,0,0);

    [TextArea] public string explanation;

    public virtual void EquipmentEffect(bool isAdd)
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Piece"))
        {
            targetPiece = other.GetComponent<Piece>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Piece"))
        {
            targetPiece = null;
        }
    }
}
