using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Synerge
{
    public static FieldManager fieldManager;

    /// <summary>
    /// Myth Synerge Effect
    /// </summary>
    /// <param name="isPlus"></param>
    
    public static GameObject setOneGreatMoutainPiece = null; //1 Star GreatMoutain
    public static GameObject setTwoGreatMoutainPiece = null; //2 Star GreatMoutain
    public static void MythGreatMoutain(bool isPlus)
    {
        int _count = fieldManager.mythActiveCount[PieceData.Myth.GreatMountain];
        if (isPlus)
        {
            switch(_count)
            {
                case 2:
                    break;
                case 4:
                    break;
                case 6:
                    break;
                case 8:
                    break;
            }
        }
        else
        {

        }
    }

    public static void MythFrostyWind(bool isPlus)
    {

    }

    public static void MythSandKingdom(bool isPlus)
    {
        
    }

    public static void MythHeavenGround(bool isPlus)
    {
        //Heaven Piece Abnormal Condition
    }

    public static void MythBurningGround(bool isPlus)
    {

    }

    /// <summary>
    /// Animal Synerge Effect
    /// </summary>
    /// <param name="isPlus"></param>
    public static void AnimalHamster(bool isPlus)
    {

    }
    public static void AnimalCat(bool isPlus)
    {

    }
    public static void AnimalDog(bool isPlus)
    {

    }
    public static void AnimalFrog(bool isPlus)
    {
        if(isPlus)
        {
            switch(fieldManager.animalActiveCount[PieceData.Animal.Frog])
            {
                case 2:
                    foreach(var frog in fieldManager.myFilePieceList)
                    {
                        if(frog.pieceData.animal == PieceData.Animal.Frog)
                        {
                            frog.damageRise += frog.attackPower * 0.1f;
                            frog.armor += frog.armor * 0.1f;
                        }
                    }
                    break;
                case 4:
                    foreach (var frog in fieldManager.myFilePieceList)
                    {
                        if (frog.pieceData.animal == PieceData.Animal.Frog)
                        {
                            frog.damageRise += frog.attackPower * 0.1f;
                            frog.armor += frog.armor * 0.1f;
                        }
                    }
                    break;
                case 6:
                    break;
            }
        }
    }
    public static void AnimalRabbit(bool isPlus)
    {

    }

    /// <summary>
    /// United Synerge Effect
    /// </summary>
    /// <param name="isPlus"></param>
    public static void UnitedUnderWorld(bool isPlus)
    {

    }
    public static void UnitedFaddist(bool isPlus)
    {

    }
    public static void UnitedWarMachine(bool isPlus)
    {

    }
    public static void UnitedCreature(bool isPlus)
    {

    }
}
