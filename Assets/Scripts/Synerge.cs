using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Synerge
{
    public static FieldManager fieldManager;
    public static BuffManager buffManager;

    /// <summary>
    /// Myth Synerge Effect
    /// </summary>
    /// <param name="isPlus"></param>

    #region GreatMoutain
    public static Piece setOneCostGreatMoutainPiece = null; //1 Star GreatMoutain
    public static Piece setTwoCostGreatMoutainPiece = null; //2 Star GreatMoutain
    public static void MythGreatMoutain(bool isPlus)
    {
        int _greatMoutainCount = fieldManager.mythActiveCount[PieceData.Myth.GreatMountain];
    }
    #endregion
    #region FrostyWind
    public static void MythFrostyWind(bool isPlus)
    {
        int _frostyWindCount = fieldManager.mythActiveCount[PieceData.Myth.FrostyWind];
    }
    #endregion
    #region SandKingdom
    public static void MythSandKingdom(bool isPlus)
    {
        int _sandKingdomCount = fieldManager.mythActiveCount[PieceData.Myth.SandKingdom];
    }
    #endregion
    #region HeavenGround
    public static void MythHeavenGround(bool isPlus)
    {
        int _heavenGroundCount = fieldManager.mythActiveCount[PieceData.Myth.HeavenGround];
        
    }
    #endregion
    #region BurningGround
    public static void MythBurningGround(bool isPlus)
    {
        int _burningGroundCount = fieldManager.mythActiveCount[PieceData.Myth.BurningGround];
    }
    #endregion
    /// <summary>
    /// Animal Synerge Effect
    /// </summary>
    /// <param name="isPlus"></param>
    /// 
    #region Hamster
    public static Piece miniHamster;
    public static void AnimalHamster(bool isPlus)
    {
        int _hamsterCount = fieldManager.animalActiveCount[PieceData.Animal.Hamster];
    }
    #endregion
    public static void AnimalCat(bool isPlus)
    {
        int _catCount = fieldManager.animalActiveCount[PieceData.Animal.Cat];
    } 
    #region Dog
    public static void AnimalDog(bool isPlus)
    {
        int _dogCount = fieldManager.animalActiveCount[PieceData.Animal.Dog];
    }
    #endregion
    #region Frog
    public static void AnimalFrog(bool isPlus)
    {
        int _frogCount = fieldManager.animalActiveCount[PieceData.Animal.Frog];
        if (isPlus) AddSynergeBuffs(PieceData.Animal.Frog, _frogCount);
        else if (!isPlus) RemoveSynergeBuffs(PieceData.Animal.Frog, _frogCount);
    }
    #endregion
    #region Rabbit
    public static void AnimalRabbit(bool isPlus)
    {
        int _rabbitCount = fieldManager.animalActiveCount[PieceData.Animal.Rabbit];
    }
    #endregion

    /// <summary>
    /// United Synerge Effect
    /// </summary>
    /// <param name="isPlus"></param>
    public static void UnitedUnderWorld(bool isPlus)
    {
        int _underWorldCount = fieldManager.unitedActiveCount[PieceData.United.UnderWorld];
    }
    public static void UnitedFaddist(bool isPlus)
    {
        int _faddistCount = fieldManager.unitedActiveCount[PieceData.United.Faddist];
    }
    public static void UnitedWarMachine(bool isPlus)
    {
        int _warMachineCount = fieldManager.unitedActiveCount[PieceData.United.WarMachine];
    }
    public static void UnitedCreature(bool isPlus)
    {
        int _creatureCount = fieldManager.unitedActiveCount[PieceData.United.Creature];
    }
    static void AddSynergeBuffs(PieceData.Myth value, int count)
    {

    }

    static List<Buff> animalBuff;
    static void AddSynergeBuffs(PieceData.Animal value, int count)
    {
        if (value == PieceData.Animal.Hamster) animalBuff = buffManager.animalBuff[0].hamsterBuff;
        else if (value == PieceData.Animal.Cat) animalBuff = buffManager.animalBuff[0].catBuff;
        else if (value == PieceData.Animal.Dog) animalBuff = buffManager.animalBuff[0].dogBuff;
        else if (value == PieceData.Animal.Frog) animalBuff = buffManager.animalBuff[0].frogBuff;
        else if (value == PieceData.Animal.Rabbit) animalBuff = buffManager.animalBuff[0].rabbitBuff;

        if (count >= 2)
        {
            if (value == PieceData.Animal.Frog || value == PieceData.Animal.Rabbit) count = count / 2;
            else count = count / 3;
        }

        foreach(var piece in fieldManager.myFilePieceList)
        {
            if(piece.pieceData.animal == value)
            {
                for(int i = 0; i < count; i++)
                {
                    if(!piece.buffList.Contains(animalBuff[i])) piece.buffList.Add(animalBuff[i]);
                }
            }
        }
    }
    static void AddSynergeBuffs(PieceData.United value, int count)
    {
        
    }


    static void RemoveSynergeBuffs(PieceData.Myth value, int count)
    {

    }
    static void RemoveSynergeBuffs(PieceData.Animal value, int count)
    {

    }
    static void RemoveSynergeBuffs(PieceData.United value, int count)
    {

    }
}
