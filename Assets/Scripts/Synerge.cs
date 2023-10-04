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

    #region GreatMoutain
    public static Piece setOneCostGreatMoutainPiece = null; //1 Star GreatMoutain
    public static Piece setTwoCostGreatMoutainPiece = null; //2 Star GreatMoutain
    public static void MythGreatMoutain(bool isPlus)
    {
        int _greatMoutainCount = fieldManager.mythActiveCount[PieceData.Myth.GreatMountain];
        
        if(isPlus == true)
        {
            if (_greatMoutainCount >= 2) AddGreatMoutainSynergeBuff("GreatMoutain_1");
            if (_greatMoutainCount >= 4) AddGreatMoutainSynergeBuff("GreatMoutain_2");
            if (_greatMoutainCount >= 6) AddGreatMoutainSynergeBuff("GreatMoutain_3");
            if (_greatMoutainCount >= 8) AddGreatMoutainSynergeBuff("GreatMoutain_4");
        }   
        else if(isPlus == false)
        {
            if (_greatMoutainCount < 2) RemoveGreatMoutainSynergeBuff("GreatMoutain_1");
            if (_greatMoutainCount < 4) RemoveGreatMoutainSynergeBuff("GreatMoutain_2");
            if (_greatMoutainCount < 6) RemoveGreatMoutainSynergeBuff("GreatMoutain_3");
            if (_greatMoutainCount < 8) RemoveGreatMoutainSynergeBuff("GreatMoutain_4");
        }
    }
    private static void AddGreatMoutainSynergeBuff(string synergeName)
    {
        foreach (var greatMountain in fieldManager.myFilePieceList)
        {
            if (greatMountain.pieceData.myth == PieceData.Myth.GreatMountain && !greatMountain.sReceivedBuff.Contains(synergeName))
            {
                greatMountain.sReceivedBuff.Add(synergeName);
                if (!fieldManager.BattleEffect.Contains("GreatMoutainEffect")) fieldManager.BattleEffect.Add("GreatMoutainEffect");
            }
        }
    }
    private static void RemoveGreatMoutainSynergeBuff(string synergeName)
    {
        foreach (var greatMountain in fieldManager.myFilePieceList)
        {
            if (greatMountain.pieceData.myth == PieceData.Myth.GreatMountain && greatMountain.sReceivedBuff.Contains(synergeName))
            {
                greatMountain.sReceivedBuff.Remove(synergeName);
                if(synergeName == "GreatMoutain_1") fieldManager.BattleEffect.Remove("GreatMoutainEffect");
            }
        }
    }
    #endregion
    #region FrostyWind
    public static void MythFrostyWind(bool isPlus)
    {
        int _frostyWindCount = fieldManager.mythActiveCount[PieceData.Myth.FrostyWind];

        if (isPlus == true)
        {
            if (_frostyWindCount >= 3) AddFrostyWindSynergeBuff("FrostyWind_1");
            if (_frostyWindCount >= 6) AddFrostyWindSynergeBuff("FrostyWind_2");
            if (_frostyWindCount >= 9) AddFrostyWindSynergeBuff("FrostyWind_3");
        }
        else if (isPlus == false)
        {
            if (_frostyWindCount < 3) RemoveFrostyWindSynergeBuff("FrostyWind_1");
            if (_frostyWindCount < 6) RemoveFrostyWindSynergeBuff("FrostyWind_2");
            if (_frostyWindCount < 9) RemoveFrostyWindSynergeBuff("FrostyWind_3");
        }
    }
    private static void AddFrostyWindSynergeBuff(string synergeName)
    {
        foreach (var frostyWind in fieldManager.myFilePieceList)
        {
            if (frostyWind.pieceData.myth == PieceData.Myth.FrostyWind && !frostyWind.sReceivedBuff.Contains(synergeName))
            {
                frostyWind.sReceivedBuff.Add(synergeName);
                if (!fieldManager.BattleEffect.Contains("FrostyWindEffect")) fieldManager.BattleEffect.Add("FrostyWindEffect");
            }
        }
    }
    private static void RemoveFrostyWindSynergeBuff(string synergeName)
    {
        foreach (var frostyWind in fieldManager.myFilePieceList)
        {
            if (frostyWind.pieceData.myth == PieceData.Myth.FrostyWind && frostyWind.sReceivedBuff.Contains(synergeName))
            {
                frostyWind.sReceivedBuff.Remove(synergeName);
                if (synergeName == "FrostyWindEffect") fieldManager.BattleEffect.Remove("FrostyWindEffect");
            }
        }
    }
    #endregion
    #region SandKingdom
    public static void MythSandKingdom(bool isPlus)
    {
        int _sandKingdomCount = fieldManager.mythActiveCount[PieceData.Myth.SandKingdom];

        if (isPlus == true)
        {
            if (_sandKingdomCount >= 3) AddSandKingdomSynergeBuff("SandKingdom_1");
            if (_sandKingdomCount >= 5) AddSandKingdomSynergeBuff("SandKingdom_2");
            if (_sandKingdomCount >= 7) AddSandKingdomSynergeBuff("SandKingdom_3");
        }
        else if (isPlus == false)
        {
            if (_sandKingdomCount < 3) RemoveSandKingdomSynergeBuff("SandKingdom_1");
            if (_sandKingdomCount < 5) RemoveSandKingdomSynergeBuff("SandKingdom_2");
            if (_sandKingdomCount < 7) RemoveSandKingdomSynergeBuff("SandKingdom_3");
        }
    }
    private static void AddSandKingdomSynergeBuff(string synergeName)
    {
        foreach (var sandKingdom in fieldManager.myFilePieceList)
        {
            if (sandKingdom.pieceData.myth == PieceData.Myth.SandKingdom && !sandKingdom.sReceivedBuff.Contains(synergeName))
            {
                sandKingdom.sReceivedBuff.Add(synergeName);
                if (!fieldManager.BattleEffect.Contains("SandKindomEffect")) fieldManager.BattleEffect.Add("SandKindomEffect");

                if (synergeName == "SandKingdom_1") sandKingdom.shield = sandKingdom.defaultHealth * 0.1f;
                else if (synergeName == "SandKingdom_2") sandKingdom.shield = sandKingdom.defaultHealth * 0.25f;
                else if (synergeName == "SandKingdom_3") sandKingdom.shield = sandKingdom.defaultHealth * 0.4f;
            }
        }
    }
    private static void RemoveSandKingdomSynergeBuff(string synergeName)
    {
        foreach (var sandKingdom in fieldManager.myFilePieceList)
        {
            if (sandKingdom.pieceData.myth == PieceData.Myth.SandKingdom && sandKingdom.sReceivedBuff.Contains(synergeName))
            {
                sandKingdom.sReceivedBuff.Remove(synergeName);
                if (synergeName == "SandKindomEffect") fieldManager.BattleEffect.Remove("SandKindomEffect");

                if (synergeName == "SandKingdom_3") sandKingdom.shield = sandKingdom.defaultHealth * 0.25f;
                else if (synergeName == "SandKingdom_2") sandKingdom.shield = sandKingdom.defaultHealth * 0.1f;
                else if (synergeName == "SandKingdom_1") sandKingdom.shield = 0;
            }
        }
    }
    #endregion
    #region HeavenGround
    public static void MythHeavenGround(bool isPlus)
    {
        int _heavenGroundCount = fieldManager.mythActiveCount[PieceData.Myth.HeavenGround];

        if (isPlus == true)
        {
            if (_heavenGroundCount >= 2) AddHeavenGroundSynergeBuff("HeavenGround_1");
            if (_heavenGroundCount >= 4) AddHeavenGroundSynergeBuff("HeavenGround_2");
        }
        else if (isPlus == false)
        {
            if (_heavenGroundCount < 2) RemoveHeavenGroundSynergeBuff("HeavenGround_1");
            if (_heavenGroundCount < 4) RemoveHeavenGroundSynergeBuff("HeavenGround_2");
        }
    }
    private static void AddHeavenGroundSynergeBuff(string synergeName)
    {
        foreach (var heavenGround in fieldManager.myFilePieceList)
        {
            if (heavenGround.pieceData.myth == PieceData.Myth.HeavenGround && !heavenGround.sReceivedBuff.Contains(synergeName))
            {
                heavenGround.sReceivedBuff.Add(synergeName);
                if (!fieldManager.BattleEffect.Contains("HeavenGroundEffect")) fieldManager.BattleEffect.Add("HeavenGroundEffect");
            }
        }
    }
    private static void RemoveHeavenGroundSynergeBuff(string synergeName)
    {
        foreach (var heavenGround in fieldManager.myFilePieceList)
        {
            if (heavenGround.pieceData.myth == PieceData.Myth.HeavenGround && heavenGround.sReceivedBuff.Contains(synergeName))
            {
                heavenGround.sReceivedBuff.Remove(synergeName);
                if (synergeName == "HeavenGroundEffect") fieldManager.BattleEffect.Remove("HeavenGroundEffect");
            }
        }
    }

    #endregion
    #region BurningGround
    public static void MythBurningGround(bool isPlus)
    {

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
        if (isPlus == true)
        {
            if (_hamsterCount >= 2) { AddHamsterSynergeBuff("Hamster_1"); }
            if (_hamsterCount >= 4) { AddHamsterSynergeBuff("Hamster_2"); }
            if (_hamsterCount >= 6) { AddHamsterSynergeBuff("Hamster_3"); }
        }
        else if (isPlus == false)
        {
            if (_hamsterCount < 2) { RemoveHamsterSynergeBuff("Hamster_1"); }
            if (_hamsterCount < 4) { RemoveHamsterSynergeBuff("Hamster_2"); }
            if (_hamsterCount < 6) { RemoveHamsterSynergeBuff("Hamster_3"); }
        }
    }
    private static void AddHamsterSynergeBuff(string synergeName)
    {
        foreach (var hamster in fieldManager.myFilePieceList)
        {
            if (hamster.pieceData.animal == PieceData.Animal.Hamster && !hamster.sReceivedBuff.Contains(synergeName))
            {
                hamster.sReceivedBuff.Add(synergeName);
                if (!fieldManager.BattleEffect.Contains("HamsterEffect")) fieldManager.BattleEffect.Add("HamsterEffect");
            }
        }
    }
    private static void RemoveHamsterSynergeBuff(string synergeName)
    {
        foreach (var hamster in fieldManager.myFilePieceList)
        {
            if (hamster.pieceData.animal == PieceData.Animal.Hamster && hamster.sReceivedBuff.Contains(synergeName))
            {
                hamster.sReceivedBuff.Remove(synergeName);
                if (synergeName == "Hamster_1") fieldManager.BattleEffect.Remove("HamsterEffect");
            }
        }
    }
    #endregion
    public static void AnimalCat(bool isPlus)
    {

    } 
    #region Dog
    public static void AnimalDog(bool isPlus)
    {
        int _dogCount = fieldManager.animalActiveCount[PieceData.Animal.Dog];
        if (isPlus == true)
        {
            if (_dogCount >= 2) { AddDogSynergeBuff("Dog_1"); }
            if (_dogCount >= 4) { AddDogSynergeBuff("Dog_2"); }
            if (_dogCount >= 6) { AddDogSynergeBuff("Dog_3"); }
        }
        else if (isPlus == false)
        {
            if (_dogCount < 2) { RemoveDogSynergeBuff("Dog_1"); }
            if (_dogCount < 4) { RemoveDogSynergeBuff("Dog_2"); }
            if (_dogCount < 6) { RemoveDogSynergeBuff("Dog_3"); }
        }
    }
    private static void AddDogSynergeBuff(string synergeName)
    {
        foreach (var dog in fieldManager.myFilePieceList)
        {
            if (dog.pieceData.animal == PieceData.Animal.Hamster && !dog.sReceivedBuff.Contains(synergeName))
            {
                dog.sReceivedBuff.Add(synergeName);
                if (!fieldManager.BattleEffect.Contains("DogEffect")) fieldManager.BattleEffect.Add("DogEffect");
            }
        }
    }
    private static void RemoveDogSynergeBuff(string synergeName)
    {
        foreach (var dog in fieldManager.myFilePieceList)
        {
            if (dog.pieceData.animal == PieceData.Animal.Hamster && dog.sReceivedBuff.Contains(synergeName))
            {
                dog.sReceivedBuff.Remove(synergeName);
                if(synergeName == "Dog_1") fieldManager.BattleEffect.Remove("DogEffect");
            }
        }
    }
    #endregion
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
            if (frog.pieceData.animal == PieceData.Animal.Frog && !frog.sReceivedBuff.Contains(synergeName))
            {
                frog.sReceivedBuff.Add(synergeName);
                if (!fieldManager.BattleEffect.Contains(synergeName)) { fieldManager.BattleEffect.Add("FrogEffect"); } 
                frog.DamageRise += frog.defaultAttackPower * 0.1f;
                frog.armor += frog.defaultArmor * 0.1f;
            }
        }
    }
    private static void RemoveFrogSynergeBuff(string synergeName)
    {
        foreach (var frog in fieldManager.myFilePieceList)
        {
            if (frog.pieceData.animal == PieceData.Animal.Frog && frog.sReceivedBuff.Contains(synergeName))
            {
                frog.sReceivedBuff.Remove(synergeName);
                if (synergeName == "Frog_1") { fieldManager.BattleEffect.Remove("FrogEffect"); }
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
            if (rabbit.pieceData.animal == PieceData.Animal.Rabbit && !rabbit.sReceivedBuff.Contains(synergeName))
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
            if (rabbit.pieceData.animal == PieceData.Animal.Rabbit && rabbit.sReceivedBuff.Contains(synergeName))
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
}
