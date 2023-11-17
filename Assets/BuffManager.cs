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

    [Header("개구리 시너지 환경 요소")]
    public GameObject frogRain;
    public GameObject frogThunder;

    [Header("서리바람 시너지 환경 요소")]
    public GameObject frostyWind;

    [Header("모래바람 시너지 환경 요소")]
    public GameObject sandKingdomWind;

    [Header("전쟁 병기 아이템")] 
    public List<Equipment> warMachineEquipments;


    public void FrogRain(bool isRain)
    {
        if (isRain) frogRain.SetActive(true);
        else if (!isRain) frogRain.SetActive(false);
    }

    public void FrogThunder(bool isThunder)
    {
        if (isThunder) frogThunder.SetActive(true);
        else if (!isThunder) frogThunder.SetActive(false);
    }
}
