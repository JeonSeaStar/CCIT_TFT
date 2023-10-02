using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Synerge
{
    public static FieldManager fieldManager;
    public static PathFinding pathFinding;
    /// <summary>
    /// Myth Synerge Effect
    /// </summary>
    /// <param name="isPlus"></param>
    
    public static GameObject setOneGreatMoutainPiece = null; //1 Star GreatMoutain
    public static GameObject setTwoGreatMoutainPiece = null; //2 Star GreatMoutain
    public static void MythGreatMoutain(bool isPlus)
    {
        int _greatMoutainCount = fieldManager.mythActiveCount[PieceData.Myth.GreatMountain];
        //Tile _tile = null;
        //for(int i = pathFinding.grid.Count; i > 0; i--)
        //{
        //    for(int j = 0; j < pathFinding.grid[i].tile.Count; j++)
        //    {
        //        if (pathFinding.grid[i].tile[j].IsFull == false)
        //        {
        //            _tile = pathFinding.grid[i].tile[j];
        //            break;
        //        }
        //    }
        //}
        if(isPlus == true)
        {
            if (_greatMoutainCount == 2) Debug.Log(25);
            if (_greatMoutainCount == 4) Debug.Log(25);
            if (_greatMoutainCount == 8) Debug.Log(25);
        }   
        else if(isPlus == false)
        {
            if (_greatMoutainCount < 8) Debug.Log(25);
            if (_greatMoutainCount < 6) Debug.Log(25);
            if (_greatMoutainCount < 4) Debug.Log(25);
            if (_greatMoutainCount < 2) Debug.Log(25);
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

    #region Frog
    public static void AnimalFrog(bool isPlus)
    {
        int _frogCount = fieldManager.animalActiveCount[PieceData.Animal.Frog];
        if (isPlus == true)
        {
            if (_frogCount >= 2) { AddFrogSynergeBuff("Frog_1"); }
            if (_frogCount >= 4) { AddFrogSynergeBuff("Frog_2"); }
            if (_frogCount >= 6) { AddFrogSynergeBuff("Frog_3"); }
        }
        else if(isPlus == false)
        {
            if (_frogCount < 2) { RemoveFrogSynergeBuff("Frog_1"); }
            if (_frogCount < 4) { RemoveFrogSynergeBuff("Frog_2"); }
            if (_frogCount < 6) { RemoveFrogSynergeBuff("Frog_3"); }
        }
    }
    private static void AddFrogSynergeBuff(string synergeName)
    {
        foreach (var frog in fieldManager.myFilePieceList)
        {
            if (!frog.sReceivedBuff.Contains(synergeName))
            {
                frog.sReceivedBuff.Add(synergeName);
                frog.DamageRise += frog.defaultAttackPower * 0.1f;
                frog.armor += frog.defaultArmor * 0.1f;
            }
        }
        
    }
    private static void RemoveFrogSynergeBuff(string synergeName)
    {
        foreach (var frog in fieldManager.myFilePieceList)
        {
            if (frog.sReceivedBuff.Contains(synergeName))
            {
                frog.sReceivedBuff.Remove(synergeName);
                frog.DamageRise -= frog.defaultAttackPower * 0.1f;
                frog.armor -= frog.defaultArmor * 0.1f;
            }
        }
    }
    #endregion
    #region Rabbit
    public static void AnimalRabbit(bool isPlus)
    {
        int _rabbitCount = fieldManager.animalActiveCount[PieceData.Animal.Rabbit];
        if (isPlus == true)
        {
            if (_rabbitCount >= 3) { AddRabbitSynergeBuff("Rabbit_1"); }
            if (_rabbitCount >= 6) { AddRabbitSynergeBuff("Rabbit_2"); }
            if (_rabbitCount >= 9) { AddRabbitSynergeBuff("Rabbit_3"); }
        }
        else if (isPlus == false)
        {
            if (_rabbitCount < 3) { RemoveRabbitSynergeBuff("Rabbit_1"); }
            if (_rabbitCount < 6) { RemoveRabbitSynergeBuff("Rabbit_2"); }
            if (_rabbitCount < 9) { RemoveRabbitSynergeBuff("Rabbit_3"); }
        }
    }
    private static void AddRabbitSynergeBuff(string synergeName)
    {
        foreach (var rabbit in fieldManager.myFilePieceList)
        {
            if (!rabbit.sReceivedBuff.Contains(synergeName))
            {
                rabbit.sReceivedBuff.Add(synergeName);
                if (synergeName == "Rabbit_1")
                {
                    rabbit.AttackSpeedRise += rabbit.defaultAttackSpeed * 0.1f;
                    rabbit.moveSpeed += 0.1f;
                }
                else
                {
                    rabbit.AttackSpeedRise += rabbit.defaultAttackSpeed * 0.2f;
                    rabbit.moveSpeed += 0.2f;
                }
            }
        }
    }
    private static void RemoveRabbitSynergeBuff(string synergeName)
    {
        foreach (var rabbit in fieldManager.myFilePieceList)
        {
            if (rabbit.sReceivedBuff.Contains(synergeName))
            {
                rabbit.sReceivedBuff.Remove(synergeName);
                if (synergeName == "Rabbit_1")
                {
                    rabbit.AttackSpeedRise -= rabbit.defaultAttackSpeed * 0.1f;
                    rabbit.moveSpeed -= 0.1f;
                }
                else
                {
                    rabbit.AttackSpeedRise -= rabbit.defaultAttackSpeed * 0.2f;
                    rabbit.moveSpeed -= 0.2f;
                }
            }
        }
    }
    #endregion
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

    private static void RainEffect()
    {
        //
    }
}
