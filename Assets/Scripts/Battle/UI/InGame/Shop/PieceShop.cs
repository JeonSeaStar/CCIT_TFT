using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PieceShop : MonoBehaviour
{
    public FieldManager fieldManager;
    public List<Sprite> cardSprites;
    public PieceBuySlot[] slots = new PieceBuySlot[5];
    public TextMeshProUGUI text;

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
    public TextMeshProUGUI levelUpCost;

    private void Awake()
    {
        InitSlot();
        levelUpCost.text = fieldManager.owerPlayer.levelUpCost[fieldManager.owerPlayer.level].ToString();
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
        SoundManager.instance.Play("UI/Eff_Gold_Pos", SoundManager.Sound.Effect);
        foreach (var slot in slots)
        {
            //여기 줄에 기물 데이터 넣어주기
            GetPieceTier(fieldManager.owerPlayer.level, slot);
        }
    }

    public void ShopSwitch()
    {
        bool active = !gameObject.activeSelf;
        gameObject.SetActive(active);

        if (active)
        {
            text.text = "상점\n닫기";
            Camera.main.transform.DOMove(new Vector3(4.5f, 20, -30), 0.5f);
            SoundManager.instance.Play("UI/Eff_Button_Positive", SoundManager.Sound.Effect);
        }
        else
        {
            text.text = "상점\n열기";
            Camera.main.transform.DOMove(new Vector3(4.5f, 20, -22), 0.5f);
            SoundManager.instance.Play("UI/Eff_Button_Nagative", SoundManager.Sound.Effect);
        }
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

        if (fieldManager.owerPlayer.gold >= fieldManager.owerPlayer.levelUpCost[fieldManager.owerPlayer.level])
        {
            fieldManager.owerPlayer.gold -= fieldManager.owerPlayer.levelUpCost[fieldManager.owerPlayer.level];
            fieldManager.owerPlayer.level++;

            fieldManager.playerState.UpdateLevel(fieldManager.owerPlayer.level);
            fieldManager.playerState.UpdateMoney(fieldManager.owerPlayer.gold);

            fieldManager.fieldPieceStatus.UpdateFieldStatus(fieldManager.myFilePieceList.Count, fieldManager.owerPlayer.maxPieceCount[fieldManager.owerPlayer.level]);
        }

        levelUpCost.text = fieldManager.owerPlayer.levelUpCost[fieldManager.owerPlayer.level].ToString();
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

    public void DeactiveSlots()
    {
        foreach (var slot in slots)
            if (!slot.Bought)
                slot.DeactiveSlot(slot.pieceData);
    }
}
