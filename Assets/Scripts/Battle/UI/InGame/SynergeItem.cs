using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SynergeItem : MonoBehaviour
{
    public Image synergeGradeImage;
    public Image synergeIconImage;
    public TextMeshProUGUI synergeNameText;
    public TextMeshProUGUI maxValueText;
    public TextMeshProUGUI currentValueText;

    public string synergeName;
    public int[] maxValue;
    public int currentValue = 0;
    public int grade = 0;
}
