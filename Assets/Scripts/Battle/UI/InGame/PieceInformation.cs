using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PieceInformation : MonoBehaviour
{
    public Piece piece;
    public Image pieceImage;
    public Image skillImage;
    public TextMeshProUGUI pieceName;
    public TextMeshProUGUI currentHP;
    public TextMeshProUGUI maxHP;
    public TextMeshProUGUI currentMP;
    public TextMeshProUGUI maxMP;
    public List<SynergeDisplay> synerges;
    public TextMeshProUGUI price;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillExplain;
    public TextMeshProUGUI attackDamage;
    public TextMeshProUGUI abilityPower;
    public TextMeshProUGUI attackSpeed;
    public TextMeshProUGUI attackRange;
    public Image hpFill;
    public Image mpFill;

    public List<MythData> mythDatas;
    public List<AnimalData> animalDatas;
    public List<UnitedData> unitedDatas;

    [SerializeField] float lerpSpeed = 0.05f;

    public void Update()
    {
        if (currentHP.text != piece.health.ToString())
        {
            currentHP.text = piece.health.ToString();
            hpFill.fillAmount = piece.health / piece.pieceData.health[piece.star];
        }

        if(hpFill.fillAmount != piece.health / piece.pieceData.health[piece.star])
            hpFill.fillAmount = Mathf.Lerp(hpFill.fillAmount, piece.health / piece.pieceData.health[piece.star], lerpSpeed);

        if (currentMP.text != piece.mana.ToString())
        {
            currentMP.text = piece.mana.ToString();
            mpFill.fillAmount = piece.mana / piece.pieceData.mana[piece.star];
        }

        if (mpFill.fillAmount != piece.mana / piece.pieceData.mana[piece.star])
            mpFill.fillAmount = Mathf.Lerp(mpFill.fillAmount, piece.health / piece.pieceData.health[piece.star], lerpSpeed);
    }

    public void OpenPieceInformation(Piece target)
    {
        gameObject.SetActive(true);
        InitPieceInformation(target);
    }

    public void ClosePieceInformation()
    {
        gameObject.SetActive(false);
    }

    public void InitPieceInformation(Piece target)
    {
        piece = target;
        pieceImage.sprite = target.pieceData.piecePortrait;
        pieceImage.sprite = target.pieceData.skilSprite;
        pieceName.text = target.pieceName;
        currentHP.text = target.health.ToString();
        maxHP.text = target.maxHealth.ToString();
        currentMP.text = target.mana.ToString();
        maxMP.text = target.maxMana.ToString();
        price.text = target.pieceData.cost[target.pieceData.grade, target.star].ToString();
        skillName.text = target.pieceData.skillName;
        skillExplain.text = target.pieceData.skillExplain;
        attackDamage.text = target.attackDamage.ToString();
        abilityPower.text = target.abilityPower.ToString();
        attackSpeed.text = target.attackSpeed.ToString();
        attackRange.text = target.attackRange.ToString();

        float currentHealth = (float)target.health;
        float maxHealth = (float)target.maxHealth;
        float currentMana = (float)target.mana;
        float maxMana = (float)target.maxMana;
        hpFill.fillAmount = GetFillValue(currentHealth, maxHealth);
        mpFill.fillAmount = GetFillValue(currentMana, maxMana);

        if (target.pieceData.myth != PieceData.Myth.None)
        {
            foreach (var myth in mythDatas)
            {
                if (myth.myth == target.pieceData.myth)
                {
                    synerges[0].synergeIcon.sprite = myth.synergeIconSprite;
                    synerges[0].synergeName.text = myth.synergeName;
                    synerges[0].synergeInfoGameObject.SetActive(true);
                    break;
                }
            }
        }
        else
            synerges[0].synergeInfoGameObject.SetActive(false);

        if (target.pieceData.animal != PieceData.Animal.None)
        {
            foreach (var animal in animalDatas)
            {
                if (animal.animal == target.pieceData.animal)
                {
                    synerges[1].synergeIcon.sprite = animal.synergeIconSprite;
                    synerges[1].synergeName.text = animal.synergeName;
                    synerges[1].synergeInfoGameObject.SetActive(true);
                    break;
                }
            }
        }
        else
            synerges[1].synergeInfoGameObject.SetActive(false);

        if (target.pieceData.united != PieceData.United.None)
        {
            synerges[2].synergeInfoGameObject.SetActive(false);
            //foreach (var united in unitedDatas)
            //{
            //    if (united.united == target.pieceData.united)
            //    {
            //        synerges[2].synergeIcon.sprite = united.synergeIconSprite;
            //        synerges[2].synergeName.text = united.synergeName;
            //        synerges[2].synergeInfoGameObject.SetActive(true);
            //        break;
            //    }
            //}
        }
        else
            synerges[2].synergeInfoGameObject.SetActive(false);
    }

    public float GetFillValue(float currentValue, float maxValue)
    {
        return currentValue / maxValue;
    }
}

[System.Serializable]
public class SynergeDisplay
{
    public GameObject synergeInfoGameObject;
    public Image synergeIcon;
    public TextMeshProUGUI synergeName;
}

[System.Serializable]
public class MythData
{
    public PieceData.Myth myth;
    public Sprite synergeIconSprite;
    public string synergeName;
}

[System.Serializable]
public class AnimalData
{
    public PieceData.Animal animal;
    public Sprite synergeIconSprite;
    public string synergeName;
}

[System.Serializable]
public class UnitedData
{
    public PieceData.United united;
    public Sprite synergeIconSprite;
    public string synergeName;
}