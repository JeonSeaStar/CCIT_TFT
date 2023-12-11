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
    [SerializeField] Image extraShieldbarSprite;

    [SerializeField] float lerpSpeed = 0.05f;

    public Animator animator;

    public void InitHealthbar(float maxHealth, float currentHealth, float shield)
    {
        float _shield = shield / maxHealth;
        float _health = currentHealth / maxHealth;

        healthbarSprite.fillAmount = _health;

        if (_shield + _health <= 1)
        {
            extraShieldbarSprite.fillAmount = 0;
            shieldbarSprite.fillAmount = _shield + _health;
        }
        else if (_shield > 0 && _shield + _health > 1)
        {
            extraShieldbarSprite.fillAmount = (_shield + _health) - 1;
        }
    }

    public void FusionStarAnim(int star)
    {
        animator.SetTrigger(string.Format("{0}to{1}",star + 1,star + 2));
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
