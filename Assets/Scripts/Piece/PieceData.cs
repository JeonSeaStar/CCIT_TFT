using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PieceData", menuName = "Scriptable Object/Piece Data", order = int.MaxValue)]
public class PieceData : ScriptableObject
{
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

    public enum Mythology { NONE = -1, A, B, C, D, E, MAX }
    public enum Species { NONE = -1, HAMSTER, CAT, DOG, FROG, RABBIT, MAX }
    public enum PlusSynerge { NONE, A, B, C, D, E, MAX }
    public Mythology mythology = Mythology.NONE;
    public Species species = Species.NONE;
    public PlusSynerge plusSynerge = PlusSynerge.NONE;

    public void InitialzePiece(Piece piece)
    {
        piece.pieceName = pieceName;
        piece.piecePortrait = piecePortrait;
        piece.health = health;
        piece.mana = mana;
        piece.attackDamage = attackDamage;
        piece.abilityPower = abilityPower;
        piece.armor = armor;
        piece.magicResist = magicResist;
        piece.attackSpeed = attackSpeed;
        piece.criticalChance = criticalChance;
        piece.criticalDamage = criticalDamage;
        piece.attackRange = attackRange;
        piece.mythology = mythology;
        piece.species = species;
        piece.plusSynerge = plusSynerge;
    }

    void CalculateEquipments(Piece piece)
    {
        foreach (var item in piece.Equipments)
        {

        }
    }
}
