using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SynergeBoard : MonoBehaviour
{
    public static SynergeBoard instance;
    private void Awake() => instance = this;
    public List<SynergeItem> synergeList;
    public Sprite[] synergeGradeSprite;

    private void SetActiveSynergeItem(SynergeItem target, bool active)
    {
        target.gameObject.SetActive(active);
    }

    public void SynergeCountUpdate(PieceData.Myth myth, PieceData.Animal animal, PieceData.United united, bool b)
    {
        SynergeCountChange(myth.ToString(), b);
        SynergeCountChange(animal.ToString(), b);
        SynergeCountChange(united.ToString(), b);
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

        target.currentValueText.text = target.currentValue.ToString();
        target.maxValueText.text = target.maxValue[GetSynergeGrade(target) + 1].ToString();

        if (target.currentValue > 0)
            SetActiveSynergeItem(target, true);
        else
            SetActiveSynergeItem(target, false);

        SynergeGradeChange(target);
        SortingSynergeItem();
    }

    private void SortingSynergeItem()
    {
        int highestGrade = GetHighestGrade();

        List<SynergeItem> sortingSynergeItem = new List<SynergeItem>();

        for(int i = highestGrade; i >= 0; i--)
        {
            List<SynergeItem> currentGradeSynergeItems = new List<SynergeItem>();

            foreach(SynergeItem item in synergeList)
            {
                if(item.grade == i)
                    currentGradeSynergeItems.Add(item);
            }

            IEnumerable<SynergeItem> Temp = currentGradeSynergeItems.OrderBy(item => item.synergeNameText.text);
            currentGradeSynergeItems = Temp.ToList();

            foreach (SynergeItem item in currentGradeSynergeItems)
                sortingSynergeItem.Add(item);
        }

        synergeList = sortingSynergeItem;

        foreach (SynergeItem item in synergeList)
            item.transform.SetAsLastSibling();
    }

    private int GetHighestGrade()
    {
        int highestGrade = 0;

        foreach(SynergeItem item in synergeList)
        {
            int grade = GetSynergeGrade(item);

            if (highestGrade < grade)
                highestGrade = grade;
        }

        return highestGrade;
    }

    private void SynergeGradeChange(SynergeItem target)
    {
        if(target.grade >= 0)
        {
            int grade = GetSynergeGrade(target);
            target.synergeGradeImage.sprite = synergeGradeSprite[GetSynergeGrade(target)];
        }
    }

    private int GetSynergeGrade(SynergeItem target)
    {
        if (target.currentValue >= target.maxValue[target.grade + 1])
            target.grade++;

        return target.grade;
    }

    private SynergeItem GetSynergeItem(string synergeName)
    {
        foreach (SynergeItem synergeItem in synergeList)
        {
            if (synergeItem.synergeName == synergeName)
                return synergeItem;
        }

        return null;
    }
}
