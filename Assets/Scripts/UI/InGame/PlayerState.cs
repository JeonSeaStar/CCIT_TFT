using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerState : MonoBehaviour
{
    public TextMeshProUGUI curentXPText;
    public TextMeshProUGUI maxXPText;
    public TextMeshProUGUI currentHPText;
    public TextMeshProUGUI currentMoneyText;

    public void UpdateCurrentXP(int value)
    {
        curentXPText.text = value.ToString();
    }

    public void UpdateMaxXP(int value)
    {
        maxXPText.text = value.ToString();
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
