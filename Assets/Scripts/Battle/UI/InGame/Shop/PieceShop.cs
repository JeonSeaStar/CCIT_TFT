using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceShop : MonoBehaviour
{
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

    private void Awake()
    {
        RefreshSlots();
    }

    public void RefreshSlots()
    {
        foreach (var slot in slots)
        {
            //여기 줄에 기물 데이터 넣어주기
            GetPieceTier(0, slot);
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
}
