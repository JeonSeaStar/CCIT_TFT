using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MythBuffs
{
    public List<BuffData> greatMoutainBuff;
    public List<BuffData> frostyWindBuff;
    public List<BuffData> sandKingdomBuff;
    public List<BuffData> heavenGroundBuff;
    public List<BuffData> burningGroundBuff;
}

[System.Serializable]
public class AnimalBuffs
{
    public List<BuffData> hamsterBuff;
    public List<BuffData> catBuff;
    public List<BuffData> dogBuff;
    public List<BuffData> frogBuff;
    public List<BuffData> rabbitBuff;
}

[System.Serializable]
public class UnitedBuffs
{
    public List<BuffData> underWorldBuff;
    public List<BuffData> faddistBuff;
    public List<BuffData> warMachineBuff;
    public List<BuffData> creatureBuff;
}


public class BuffManager : MonoBehaviour
{
    public List<MythBuffs> mythBuff = new List<MythBuffs>();
    public List<AnimalBuffs> animalBuff = new List<AnimalBuffs>();
    public List<UnitedBuffs> unitedBuff = new List<UnitedBuffs>();
}
