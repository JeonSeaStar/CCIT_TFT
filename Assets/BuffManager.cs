using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MythBuffs
{
    public List<Buff> greatMoutainBuff;
    public List<Buff> frostyWindBuff;
    public List<Buff> sandKingdomBuff;
    public List<Buff> heavenGroundBuff;
    public List<Buff> burningGroundBuff;
}

[System.Serializable]
public class AnimalBuffs
{
    public List<Buff> hamsterBuff;
    public List<Buff> catBuff;
    public List<Buff> dogBuff;
    public List<Buff> frogBuff;
    public List<Buff> rabbitBuff;
}

[System.Serializable]
public class UnitedBuffs
{
    public List<Buff> underWorldBuff;
    public List<Buff> FaddistBuff;
    public List<Buff> warMachineBuff;
    public List<Buff> creatureBuff;
}


public class BuffManager : MonoBehaviour
{
    private void Awake() => Synerge.buffManager = this;

    public List<MythBuffs> mythBuff = new List<MythBuffs>();
    public List<AnimalBuffs> animalBuff = new List<AnimalBuffs>();
    public List<UnitedBuffs> unitedBuff = new List<UnitedBuffs>();
}
