using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SynergeItemGameObject : MonoBehaviour
{
    public Image synergeGradeImage;
    public Image synergeIconImage;
    public TextMeshProUGUI synergeNameText;
    public TextMeshProUGUI maxValueText;
    public TextMeshProUGUI currentValueText;
    public List<ContentSizeFitter> csf;
    public List<HorizontalLayoutGroup> hlg;
}
