using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceShop : MonoBehaviour
{
    public FieldManager fieldManager;
    public List<Sprite> cardSprites;
    public PieceBuySlot[] slots = new PieceBuySlot[5];

    [System.Serializable]
    public class PiecePercents
    {
        public List<int> tier;
    }
    public List<PiecePercents> percentageByLevel;

    //테스트용
    [System.Serializable]
    public class TestPieceCount
    {
        public int count;
        public PieceData piecedata;
    }
    [System.Serializable]
    public class TestPieceCountList
    {
        public List<TestPieceCount> testCountList;
    }
    public List<TestPieceCountList> testList;
    public GameObject LevelUpButtonDeactive;
    public GameObject RefreshButtonDeactive;

    private void Awake()
    {
        InitSlot();
    }

    public void InitSlot()
    {
        foreach (var slot in slots)
        {
            //여기 줄에 기물 데이터 넣어주기
            GetPieceTier(fieldManager.owerPlayer.level, slot);
        }
    }

    public void RefreshSlots()
    {
        if (fieldManager.owerPlayer.gold <= 0)
            return;

        fieldManager.ChargeGold(-1);

        foreach (var slot in slots)
        {
            //여기 줄에 기물 데이터 넣어주기
            GetPieceTier(fieldManager.owerPlayer.level, slot);
        }
    }

    public void ShopSwitch()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    void SetSlot(PieceBuySlot slot, PieceData pieceData)
    {
        slot.InitSlot(pieceData);
    }

    void GetPieceTier(int level, PieceBuySlot slot)
    {
        int chooseTier = Random.Range(1, 100);
        int currentPercent = 0;
        for (int i = 0; i < percentageByLevel[level].tier.Count; i++)
        {
            if (currentPercent < chooseTier && currentPercent + percentageByLevel[level].tier[i] >= chooseTier)
            {
                SetSlot(slot, testList[i].testCountList[GetRandomIndex(i)].piecedata);
                return;
            }

            currentPercent += percentageByLevel[level].tier[i];
        }
    }

    int GetRandomIndex(int tier)
    {
        List<int> weights = new List<int>();

        for (int i = 0; i < testList[tier].testCountList.Count; i++)
        {
            weights.Add(testList[tier].testCountList[i].count);
        }

        int total = 0;
        for (int i = 0; i < weights.Count; i++)
            total += weights[i];

        int pivot = Mathf.RoundToInt(total * Random.Range(0.0f, 1.0f));
        int weight = 0;

        for (int i = 0; i < weights.Count; i++)
        {
            weight += weights[i];
            if (pivot <= weight)
            {
                return i;
            }
        }
        return 1;
    }

    public void LevelUpButton()
    {
        if (fieldManager.owerPlayer.level == fieldManager.owerPlayer.levelUpCost.Length - 1)
            return;

        if(fieldManager.owerPlayer.gold >= fieldManager.owerPlayer.levelUpCost[fieldManager.owerPlayer.level])
        {
            fieldManager.owerPlayer.gold -= fieldManager.owerPlayer.levelUpCost[fieldManager.owerPlayer.level];
            fieldManager.owerPlayer.level++;

            fieldManager.playerState.UpdateLevel(fieldManager.owerPlayer.level);
            fieldManager.playerState.UpdateMoney(fieldManager.owerPlayer.gold);

            fieldManager.fieldPieceStatus.UpdateFieldStatus(fieldManager.myFilePieceList.Count, fieldManager.owerPlayer.maxPieceCount[fieldManager.owerPlayer.level]);
        }
    }

    public void DeactiveRefresh()
    {
        if (fieldManager.owerPlayer.gold <= 0)
            RefreshButtonDeactive.SetActive(true);
        else
            RefreshButtonDeactive.SetActive(false);
    }

    public void DeactiveLevelUp()
    {
        if (fieldManager.owerPlayer.gold < fieldManager.owerPlayer.levelUpCost[fieldManager.owerPlayer.level])
            LevelUpButtonDeactive.SetActive(true);
        else
            LevelUpButtonDeactive.SetActive(false);
    }
}
