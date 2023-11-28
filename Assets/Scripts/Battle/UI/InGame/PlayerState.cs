using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerState : MonoBehaviour
{
    public TextMeshProUGUI level;
    public TextMeshProUGUI currentHPText;
    public TextMeshProUGUI currentMoneyText;

    public void UpdateLevel(int value)
    {
        level.text = (value + 1).ToString();
    }

    public void UpdateCurrentHP(int value)
    {
        currentHPText.text = value.ToString();
    }

    public void UpdateMoney(int value)
    {
        currentMoneyText.text = value.ToString();
    }
}
