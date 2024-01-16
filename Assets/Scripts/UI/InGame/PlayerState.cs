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
    public Image levelFill;
    public Image hpFill;
    public PieceShop pieceShop;
    private int maxLevel = 7;

    public void UpdateLevel(int value)
    {
        level.text = (value + 1).ToString();
        levelFill.fillAmount = (float)(value + 1) / (float)maxLevel;
    }

    public void UpdateCurrentHP(int value)
    {
        currentHPText.text = value.ToString();
        hpFill.fillAmount = (float)value / (float)FieldManager.Instance.owerPlayer.maxLifePoint;
    }

    public void UpdateMoney(int value)
    {
        currentMoneyText.text = value.ToString();
        pieceShop.DeactiveLevelUp();
        pieceShop.DeactiveRefresh();
    }
}
