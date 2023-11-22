using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceHealthBar : MonoBehaviour
{
    [SerializeField] Image healthbarSprite;

    public void UpdateHealthBar(float maxHealth, float currentHealth)
    {
        healthbarSprite.fillAmount = currentHealth / maxHealth;
    }
}
