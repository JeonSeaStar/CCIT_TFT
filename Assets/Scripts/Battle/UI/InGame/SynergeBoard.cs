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

    public static SynergeBoard instance;
    private void Awake()
    {
        instance = this;

        SortingSynergeItem();
        ChangeItemValue();

        for(int i = 0; i < synergeItemGameObjects.Count; i++)
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
        if (target.currentValue >= target.maxValue[target.grade + 1])
            target.grade++;

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

            foreach (SynergeItem item in synergeItems)
            {
                if (!sortingSynergeItem.Contains(item))
                {
                    sortingSynergeItem.Add(item);
                }
            }
        }

        synergeItems = sortingSynergeItem;
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

    private void SynergeCountChange(string synergeName, bool b)
    {
        if (synergeName == "None")
            return;

        SynergeItem target = GetSynergeItem(synergeName);

        if (b)
            target.currentValue++;
        else
            target.currentValue--;

        SynergeGradeChange(target);
        SortingSynergeItem();

        ChangeItemValue();
    }

    public void SynergeCountUpdate(PieceData.Myth myth, PieceData.Animal animal, PieceData.United united, bool b)
    {
        SynergeCountChange(myth.ToString(), b);
        SynergeCountChange(animal.ToString(), b);
        //SynergeCountChange(united.ToString(), b);
    }

    private void ChangeItemValue()
    {
        for (int i = 0; i < synergeItemGameObjects.Count; i++)
        {
            if (synergeItems[i].currentValue > 0)
            {
                if (GetSynergeGrade(synergeItems[i]) == 0)
                {
                    synergeItemGameObjects[i].synergeIconImage.sprite = synergeItems[i].synergeIcon[0];

                    print(synergeItemGameObjects[i].name + ", " + synergeItems[i].synergeOutputName);
                }
                else
                    synergeItemGameObjects[i].synergeIconImage.sprite = synergeItems[i].synergeIcon[1];
                synergeItemGameObjects[i].synergeNameText.text = synergeItems[i].synergeOutputName;
                synergeItemGameObjects[i].currentValueText.text = synergeItems[i].currentValue.ToString();
                synergeItemGameObjects[i].maxValueText.text = synergeItems[i].maxValue[GetSynergeGrade(synergeItems[i]) + 1].ToString();
                synergeItemGameObjects[i].synergeGradeImage.sprite = synergeGrade[GetSynergeGrade(synergeItems[i])];
            }
            else
            {
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