using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

[System.Serializable]
public class AugmentInformation
{
    public string augmentName;
    [TextArea] public string augmentField;

    public enum AugmentType { None, Immediately, BattleStart, Coroutine };
    public AugmentType augmentType = AugmentType.None;

    [Space(10)]
    public UnityEvent func;
}

public class AugmentManager : MonoBehaviour
{
    private static AugmentManager instance;
    public static AugmentManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AugmentManager>();
                if (instance == null)
                {
                    GameObject _arena = new GameObject();
                    _arena.name = "AugmentManager";
                    instance = _arena.AddComponent<AugmentManager>();
                }
            }
            return instance;
        }
    }

    [SerializeField] List<AugmentInformation> augmentInformationList;

    [SerializeField] GameObject augmentPanel;
    [SerializeField] Button[] augmentBtns;
    [SerializeField] TextMeshProUGUI firstAugmentName;
    [SerializeField] TextMeshProUGUI firstAugmentField;
    [SerializeField] TextMeshProUGUI secondAugmentName;
    [SerializeField] TextMeshProUGUI secondAugmentField;
    [SerializeField] TextMeshProUGUI thirdAugmentName;
    [SerializeField] TextMeshProUGUI thirdAugmentField;

    [Header("���� üũ")]
    [SerializeField] bool health_augment;
    [SerializeField] bool attackPower_augment;
    [SerializeField] bool abilityPower_augment;
    [SerializeField] bool criticalChance_augment;
    [SerializeField] bool attackSpeed_augment;
    [SerializeField] bool healthTrain_augment;
    [SerializeField] bool doubleAttackPower_augment;
    [SerializeField] bool giantPower_augment;
    [SerializeField] bool rollerHamster_augment;
    [SerializeField] public bool lifeInsurance_augment;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void CheckAugmentRound(int currentStage)
    {
        if (currentStage == 1 || currentStage == 3)
        {
            //AugmentInfo(firstAugmentName, firstAugmentField, augmentInformationList[0]);
            //AugmentInfo(secondAugmentName, secondAugmentField, augmentInformationList[1]);
            //AugmentInfo(thirdAugmentName, thirdAugmentField, augmentInformationList[2]);

            foreach (var btn in augmentBtns)
            {
                //btn.onClick.AddListener(delegate { Test(ref health_augment); });
                //btn.onClick.AddListener(delegate { Test(ref health_augment); });
            }

            augmentPanel.SetActive(true);
        }
    }

    void AugmentInfo(TextMeshProUGUI augmentName, TextMeshProUGUI augmentField, AugmentInformation augmentInformation)
    {
        augmentName.text = augmentInformation.augmentName;
        augmentField.text = augmentInformation.augmentField;
    }

    public void AugmentCheck(Piece piece)
    {
        int star = piece.star;
        if (health_augment) piece.health += 300;
        if (attackPower_augment) piece.attackDamage += 20;
        if (abilityPower_augment) piece.abilityPower += 50;
        if (criticalChance_augment) piece.criticalChance += piece.pieceData.criticalChance[star] * 0.3f;
        if (attackSpeed_augment) piece.attackSpeed += piece.pieceData.attackSpeed[star] * 0.3f;
        if (healthTrain_augment) piece.health += 450; piece.attackDamage -= 20;
        if (doubleAttackPower_augment)
        {
            piece.attackDamage += piece.pieceData.attackDamage[star] * 2f;
            piece.attackSpeed -= piece.pieceData.attackSpeed[star] * 0.5f;
        }
        if (giantPower_augment) piece.attackDamage += piece.pieceData.health[star] * 0.02f;
        if (rollerHamster_augment) piece.attackSpeed += piece.pieceData.attackSpeed[star];
    }

    public void HamsterAugmentCheck(Piece piece) // For the miniHamster
    {
        if (rollerHamster_augment) piece.attackSpeed += piece.pieceData.attackSpeed[0];
    }

    #region Immediately
    public void AddHealth() => health_augment = true;
    public void AddAttack() => attackPower_augment = true;
    public void AddAbilityPower() => abilityPower_augment = true;
    public void AddCriticalDamage() => criticalChance_augment = true;
    public void AddAttackSpeed() => attackSpeed_augment = true;
    public void AddHealthTrain() => healthTrain_augment = true;
    public void AddDoublePower() => doubleAttackPower_augment = true;
    public void AddGiantPower() => giantPower_augment = true;
    public void AddRollerHamster() => rollerHamster_augment = true;
    public void AddLifeInsurance() => lifeInsurance_augment = true;
    #endregion

    #region BattleStart
    public void Protection()
    {
        void AddProtectionFunc(bool isAdd)
        {
            if (isAdd == true)
                foreach (var piecelist in FieldManager.Instance.pieceDpList) { piecelist.piece.shield += 500f; }
        }
        FieldManager.Instance.AddBattleStartEffect(AddProtectionFunc);
    }

    public void StunWind()
    {
        void AddStunWindFunc(bool isAdd)
        {
            if (isAdd == true)
                foreach (var piece in FieldManager.Instance.enemyFilePieceList) { piece.SetStun(3f); }
        }
        FieldManager.Instance.AddBattleStartEffect(AddStunWindFunc);
    }
    #endregion

    #region Coroutine
    
    #endregion
}