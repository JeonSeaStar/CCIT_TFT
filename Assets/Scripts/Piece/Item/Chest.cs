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
        [Header("ÀÏ¹Ý µî±Þ È®·ü")] public float commonProbability;
        [Header("Èñ±Í µî±Þ È®·ü")] public float uncommonProbability;
    }

    [System.Serializable]
    class ItemTypeProbability
    {
        [Header("µ· È®·ü")] public float moneyProbability;
        [Header("Àåºñ È®·ü")] public float equipmentProbability;
    }

    [System.Serializable]
    class MoneyRange
    {
        [Header("ÃÖ¼Ò µ·")] public int minMoney;
        [Header("ÃÖ´ë µ·")] public int maxMoney;
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

    #region ¹Ú½º È®·ü
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
        //¹Ú½º µî±Þ
        if (Drawing(boxGradeProbability[stage].commonProbability))
            randomBox.grade = Grade.COMMON;
        else
            randomBox.grade = Grade.UNCOMMON;

        //¹Ú½º Á¾·ù
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
    
    public void EquipmentCheck(List<EquipmentData> equipmentDataList)
    {

    }

    #region ÀÌµ¿
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
