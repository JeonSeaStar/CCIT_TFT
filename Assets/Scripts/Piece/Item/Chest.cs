using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Chest : MonoBehaviour
{
    [System.Serializable]
    public class EquipmentSpace
    {
        public Transform equipmentSpace;
        public bool full;
    }

    public List<EquipmentSpace> itemChest;

    public enum Grade { NONE, COMMON, UNCOMMON }
    public enum BoxType { NONE, MONEY, EQUIPMENT, BOTH }
    public enum EquipmentType { NONE, SUB, HIGH, ENCHANT }

    [SerializeField] List<EquipmentData> equipmentList;

    [System.Serializable]
    class BoxGradeProbability
    {
        [Header("일반 등급 확률")] public float commonProbability;
        [Header("희귀 등급 확률")] public float uncommonProbability;
    }

    [System.Serializable]
    class ItemTypeProbability
    {
        [Header("돈 확률")] public float moneyProbability;
        [Header("장비 확률")] public float equipmentProbability;
    }

    [System.Serializable]
    class MoneyRange
    {
        [Header("최소 돈")] public int minMoney;
        [Header("최대 돈")] public int maxMoney;
    }

    [SerializeField] List<BoxGradeProbability> boxGradeProbability;
    [SerializeField] List<ItemTypeProbability> itemTypeProbability;
    [SerializeField] List<MoneyRange> moneyRange;
    public float height = 1;

    [System.Serializable]
    public class EquipmentCombination
    {
        public EquipmentData subEquipmentData;
        public EquipmentData resultEquipmentData;
    }

    [System.Serializable]
    public class EquipmentCombinationTable
    {
        public EquipmentData mainEquipmentData;
        public List<EquipmentCombination> combinationTable;
    }

    public List<EquipmentCombinationTable> equipmentCombinationTable;

    #region 박스 확률
    bool Drawing(float x)
    {
        float drawing = Random.Range(0, 100);

        if (drawing <= x)
            return true;
        else
            return false;
    }

    int GetGradeValue(Grade grade)
    {
        switch (grade)
        {
            case Grade.COMMON:
                return 0;
            case Grade.UNCOMMON:
                return 1;
            default:
                return -1;
        }
    }

    EquipmentData GetEquipmentData()
    {
        EquipmentData equipment = equipmentList[Random.Range(0, equipmentList.Count)];

        return equipment;
    }

    int GetMoney(int grade)
    {
        int money = -1;

        money = Random.Range(moneyRange[grade].minMoney, moneyRange[grade].maxMoney);

        return money;
    }

    public void SetBoxContents(RandomBox randomBox, int stage)
    {
        //박스 등급
        if (Drawing(boxGradeProbability[stage].commonProbability))
            randomBox.grade = Grade.COMMON;
        else
            randomBox.grade = Grade.UNCOMMON;

        //박스 종류
        if (Drawing(itemTypeProbability[stage].moneyProbability))
            randomBox.boxType = BoxType.MONEY;
        else
            randomBox.boxType = BoxType.EQUIPMENT;

        if (randomBox.boxType == BoxType.MONEY || randomBox.boxType == BoxType.BOTH)
            randomBox.money = GetMoney(GetGradeValue(randomBox.grade));
        else if (randomBox.boxType == BoxType.EQUIPMENT || randomBox.boxType == BoxType.BOTH)
            randomBox.equipmentData = GetEquipmentData();
    }
    #endregion

    #region 장비 조합
    public void AddEquipment(Piece target, Equipment equipment)
    {
        if(target.equipmentDatas.Count < 3)
        {
            target.equipmentDatas.Add(equipment.equipmentData);

            Destroy(equipment.gameObject);

            EquipmentCheck(target);
        }
    }

    public void RemoveEquipment(Piece target, Equipment equipment)
    {
        target.equipmentDatas.Remove(equipment.equipmentData);

        Instantiate(equipment.equipmentData.equipmentPrefab);
        //창고로 이동하는거 추가 해야함
    }

    public void EquipmentCheck(Piece targetPiece)
    {
        List<EquipmentData> equipmentDataList = targetPiece.equipmentDatas;

        EquipmentData mainEquipmentData = null;
        EquipmentData subEquipmentData = null;

        foreach (EquipmentData equipmentData in equipmentDataList)
        {
            if(mainEquipmentData == null)
            {
                mainEquipmentData = equipmentData;
                continue;
            }

            if (subEquipmentData == null)
            {
                subEquipmentData = equipmentData;
                continue;
            }
        }

        if (mainEquipmentData != null && subEquipmentData != null)
        {
            CombinationEquipment(targetPiece, mainEquipmentData, subEquipmentData);
        }
        else
            return;
    }

    private void CombinationEquipment(Piece targetPiece, EquipmentData main, EquipmentData sub)
    {
        int mainIndex = -1;
        int subIndex = -1;

        for(int i = 0; i < equipmentCombinationTable.Count; i++)
        {
            if(equipmentCombinationTable[i].mainEquipmentData == main)
            {
                mainIndex = i;
                break;
            }
            else if(i == equipmentCombinationTable.Count - 1 && mainIndex == -1) { return; }
        }

        for(int i = 0; i < equipmentCombinationTable[mainIndex].combinationTable.Count; i++)
        {
            if(equipmentCombinationTable[mainIndex].combinationTable[i].subEquipmentData == sub)
            {
                subIndex = i;
                break;
            }
            else if(i == equipmentCombinationTable[mainIndex].combinationTable.Count - 1 && subIndex == -1) { return; }
        }

        targetPiece.equipmentDatas.Remove(main);
        targetPiece.equipmentDatas.Remove(sub);
        targetPiece.equipmentDatas.Add(equipmentCombinationTable[mainIndex].combinationTable[subIndex].resultEquipmentData);
    }
    #endregion

    #region 이동
    public void CurveMove(Transform boxTransform, List<Transform> targetPositions)
    {
        Vector3 startPosition = boxTransform.localPosition;
        int randomPosition = Random.Range(0, targetPositions.Count);
        Vector3 targetPosition = targetPositions[randomPosition].position;
        Vector3 highPointPosition = new Vector3(startPosition.x + (targetPosition.x - startPosition.x) / 2, startPosition.y + height, startPosition.z + (targetPosition.z - startPosition.z) / 2);

        boxTransform.DOPath(new[] { highPointPosition, startPosition, highPointPosition, targetPosition, highPointPosition, targetPosition }, 1, PathType.CubicBezier).SetEase(Ease.Linear);
    }

    public void CurveMove(Transform boxTransform, Transform targetTransform)
    {
        Vector3 startPosition = boxTransform.localPosition;
        Vector3 targetPosition = targetTransform.position;
        Vector3 highPointPosition = new Vector3(startPosition.x + (targetPosition.x - startPosition.x) / 2, startPosition.y + height, startPosition.z + (targetPosition.z - startPosition.z) / 2);

        boxTransform.DOPath(new[] { highPointPosition, startPosition, highPointPosition, targetPosition, highPointPosition, targetPosition }, 1, PathType.CubicBezier).SetEase(Ease.Linear);
    }
    #endregion
}
