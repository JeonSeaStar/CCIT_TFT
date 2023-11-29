using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PieceHealthBar : MonoBehaviour
{
    Camera cam;
    public Image backgroundSprite;
    [SerializeField] Image healthbarSprite;
    [SerializeField] Image healthEaseBarSprite;
    [SerializeField] Image manabarSprite;
    [SerializeField] Image shieldbarSprite;

    [SerializeField] float lerpSpeed = 0.05f;
    public float maxHealth = 0;
    public float maxMana = 0;

    public void InitHealthbar(float maxHealth, float currentHealth, float shield)
    {
        float _shield = shield / maxHealth;
        float _health = currentHealth / maxHealth;

        healthbarSprite.fillAmount = _health;
        Debug.Log(_shield + _health);
        if (_shield + _health <= 1)
        {
            shieldbarSprite.fillAmount = _shield + healthbarSprite.fillAmount;
        }
        else
        {
            shieldbarSprite.fillOrigin = 1;
        }
    }

    public void InitManabar(float maxMana, float currentMana)
    {
        manabarSprite.fillAmount = currentMana / maxMana;
    }

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(transform.position + cam.transform.rotation * Vector3.back, cam.transform .rotation * Vector3.up);
       
        if (healthbarSprite.fillAmount != healthEaseBarSprite.fillAmount)
        {
            healthEaseBarSprite.fillAmount = Mathf.Lerp(healthEaseBarSprite.fillAmount, healthbarSprite.fillAmount, lerpSpeed);
        }
    }
}
