using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SynergeBoard : MonoBehaviour
{
    #region 옛날거
    //public List<SynergeItem> synergeList;
    //public Sprite[] synergeGradeSprite;

    //private void SetActiveSynergeItem(SynergeItem target, bool active)
    //{
    //    target.gameObject.SetActive(active);
    //}
    #endregion
    [SerializeField] GameObject canvas;
    [SerializeField] private FieldManager fieldManager;
    public static SynergeBoard instance;
    private void Awake()
    {
        instance = this;

        SortingSynergeItem();
        ChangeItemValue();

        for (int i = 0; i < synergeItemGameObjects.Count; i++)
        {
            foreach (var item in synergeItemGameObjects[i].csf)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)item.transform);
                item.enabled = false;
                item.enabled = true;
            }

            synergeItemGameObjects[i].hlg.Reverse();
            foreach (var item in synergeItemGameObjects[i].hlg)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)item.transform);
                item.enabled = false;
                item.enabled = true;
            }
            synergeItemGameObjects[i].hlg.Reverse();
        }
    }

    [SerializeField] private List<Sprite> synergeGrade;
    [SerializeField] private List<SynergeItem> synergeItems;
    [SerializeField] private List<SynergeItemGameObject> synergeItemGameObjects;

    private int GetSynergeGrade(SynergeItem target)
    {
        if (target.currentValue >= target.maxValue[target.grade])
        {
            if (target.grade < target.maxValue.Count())
            {
                ++target.grade;
            }
        }

        if(target.grade > 0 && target.currentValue < target.maxValue[target.grade - 1])
        {
            --target.grade;
        }

        return target.grade;
    }

    private int GetHighestGrade()
    {
        int highestGrade = 0;

        foreach (SynergeItem item in synergeItems)
        {
            int grade = GetSynergeGrade(item);

            if (highestGrade < grade)
                highestGrade = grade;
        }

        return highestGrade;
    }

    private void SortingSynergeItem()
    {
        int highestGrade = GetHighestGrade();

        List<SynergeItem> sortingSynergeItem = new List<SynergeItem>();

        for (int i = highestGrade; i >= 0; i--)
        {
            List<SynergeItem> currentGradeSynergeItems = new List<SynergeItem>();

            foreach (SynergeItem item in synergeItems)
            {
                if (item.grade == i && item.currentValue > 0)
                    currentGradeSynergeItems.Add(item);
            }

            IEnumerable<SynergeItem> Temp = currentGradeSynergeItems.OrderBy(item => item.synergeOutputName);
            currentGradeSynergeItems = Temp.ToList();

            foreach (SynergeItem item in currentGradeSynergeItems)
                sortingSynergeItem.Add(item);

            //foreach (SynergeItem item in synergeItems)
            //{
            //    if (!sortingSynergeItem.Contains(item) && item.currentValue > 0)
            //    {
            //        sortingSynergeItem.Add(item);
            //    }
            //}

            //foreach (SynergeItem item in synergeItems)
            //{
            //    if (!sortingSynergeItem.Contains(item))
            //    {
            //        sortingSynergeItem.Add(item);
            //    }
            //}
        }
        List<SynergeItem> otherSynergeItems = new List<SynergeItem>();

        foreach (SynergeItem item in synergeItems)
            if (!sortingSynergeItem.Contains(item))
                otherSynergeItems.Add(item);

        IEnumerable<SynergeItem> Temp2 = otherSynergeItems.OrderBy(item => item.synergeOutputName);
        otherSynergeItems = Temp2.ToList();

        foreach (SynergeItem item in otherSynergeItems)
            if (!sortingSynergeItem.Contains(item))
                sortingSynergeItem.Add(item);

        synergeItems = sortingSynergeItem;

        //canvas.gameObject.SetActive(false);
        //canvas.gameObject.SetActive(true);
    }

    private void SynergeGradeChange(SynergeItem target)
    {
        if (target.grade >= 0)
        {
            int grade = GetSynergeGrade(target);
        }
    }

    private SynergeItem GetSynergeItem(string synergeName)
    {
        foreach (SynergeItem synergeItem in synergeItems)
        {
            if (synergeItem.synergeName == synergeName)
            {
                return synergeItem;
            }
        }

        return null;
    }

    private void SynergeCountChange(PieceData.Myth myth, PieceData.Animal animal, PieceData.United united)
    {
        SynergeItem mythTarget = GetSynergeItem(myth.ToString());
        SynergeItem animalTarget = GetSynergeItem(animal.ToString());
        SynergeItem unitedTarget = GetSynergeItem(united.ToString());

        if (myth != PieceData.Myth.None)
        {
            mythTarget.currentValue = fieldManager.mythActiveCount[myth];
            SynergeGradeChange(mythTarget);
            SortingSynergeItem();
        }
        if (animal != PieceData.Animal.None)
        {
            animalTarget.currentValue = fieldManager.animalActiveCount[animal];
            SynergeGradeChange(animalTarget);
            SortingSynergeItem();
        }
        //if (united != PieceData.United.None)
        //{
        //    unitedTarget.currentValue = fieldManager.unitedActiveCount[united];
        //    SynergeGradeChange(unitedTarget);
        //}

        SortingSynergeItem();
        ChangeItemValue();
    }

    public void SynergeCountUpdate(PieceData.Myth myth, PieceData.Animal animal, PieceData.United united)
    {
        SynergeCountChange(myth, animal, united);
        SortingSynergeItem();
    }

    private void ChangeItemValue()
    {
        for (int i = 0; i < synergeItemGameObjects.Count; i++)
        {
            if (synergeItems[i].currentValue > 0)
            {
                if (GetSynergeGrade(synergeItems[i]) == 0)
                {
                    synergeItemGameObjects[i].synergeIconImage.color = new Vector4(255, 255, 255, 255);
                    synergeItemGameObjects[i].synergeIconImage.sprite = synergeItems[i].synergeIcon[0];
                }
                else
                {
                    synergeItemGameObjects[i].synergeIconImage.color = new Vector4(255, 255, 255, 255);
                    synergeItemGameObjects[i].synergeIconImage.sprite = synergeItems[i].synergeIcon[1];
                }
                synergeItemGameObjects[i].synergeNameText.text = synergeItems[i].synergeOutputName;
                synergeItemGameObjects[i].currentValueText.text = synergeItems[i].currentValue.ToString();
                synergeItemGameObjects[i].maxValueText.text = synergeItems[i].maxValue[GetSynergeGrade(synergeItems[i])].ToString();
                synergeItemGameObjects[i].synergeGradeImage.sprite = synergeGrade[GetSynergeGrade(synergeItems[i])];
            }
            else
            {
                synergeItemGameObjects[i].synergeIconImage.color = new Vector4(255, 255, 255, 0);
                synergeItemGameObjects[i].synergeIconImage.sprite = null;
                synergeItemGameObjects[i].synergeNameText.text = "미활성";
                synergeItemGameObjects[i].currentValueText.text = "0";
                synergeItemGameObjects[i].maxValueText.text = "0";
                synergeItemGameObjects[i].synergeGradeImage.sprite = synergeGrade[0];
            }

            foreach (var item in synergeItemGameObjects[i].csf)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)item.transform);
                item.enabled = false;
                item.enabled = true;
            }

            synergeItemGameObjects[i].hlg.Reverse();
            foreach (var item in synergeItemGameObjects[i].hlg)
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)item.transform);
                item.enabled = false;
                item.enabled = true;
            }
            synergeItemGameObjects[i].hlg.Reverse();
        }
    }
}

[System.Serializable]
public class SynergeItem
{
    public Sprite[] synergeIcon;
    public string synergeName;
    public string synergeOutputName;
    public int grade;
    public int[] maxValue;
    public int currentValue;
}