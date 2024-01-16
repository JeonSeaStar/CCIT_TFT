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

    [SerializeField] Button refreshBtn;
    [SerializeField] TextMeshProUGUI refreshGold;

    [Header("증강 체크")]
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
        //Shuffle(augmentInformationList);
    }

    public void CheckAugmentRound(int currentStage)
    {
        if (currentStage == 1 || currentStage == 3)
        {
            AugmentInfo(firstAugmentName, firstAugmentField, augmentInformationList[0]);
            AugmentInfo(secondAugmentName, secondAugmentField, augmentInformationList[1]);
            AugmentInfo(thirdAugmentName, thirdAugmentField, augmentInformationList[2]);

            void RemoveAugmentList()
            {
                for (int i = 0; i < 3; i++)
                {
                    augmentInformationList.Remove(augmentInformationList[0]);
                }
            }

            void RemoveAllListeners()
            {
                for (int i = 0; i < 3; i++)
                    augmentBtns[i].onClick.RemoveAllListeners();
            }

            for(int i = 0; i < augmentBtns.Length;i++)
            {
                int index = i;
                augmentBtns[i].onClick.AddListener(() => augmentInformationList[index].func.Invoke());
                augmentBtns[i].onClick.AddListener(() => RemoveAugmentList());
                augmentBtns[i].onClick.AddListener(() => augmentPanel.SetActive(false));
                augmentBtns[i].onClick.AddListener(() => RemoveAllListeners());
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
    public void LevelUp() => FieldManager.Instance.pieceShop.LevelUpButton();

    public void TightropeWalk()
    {
        int _health = FieldManager.Instance.owerPlayer.lifePoint;
        int _point = (_health - 1) * 10;
        FieldManager.Instance.ChargeGold(-(_health - 1));
        FieldManager.Instance.ChargeGold(_point);
    }
    public void GoldPocket() => FieldManager.Instance.ChargeGold(30);
    public void AddSpace()
    {
        for(int i = 0; i < FieldManager.Instance.owerPlayer.maxPieceCount.Length;i++)
        {
            FieldManager.Instance.owerPlayer.maxPieceCount[i] += 1;
        }
        FieldManager.Instance.fieldPieceStatus.UpdateFieldStatus(FieldManager.Instance.myFilePieceList.Count, FieldManager.Instance.owerPlayer.maxPieceCount[FieldManager.Instance.owerPlayer.level]);
    }
    public void BonusRoll()
    {
        void SetBonusRollEvent()
        {
            if (refreshGold.text == "0") FieldManager.Instance.ChargeGold(1);

            int ran = UnityEngine.Random.Range(0, 99);
            if (ran < 35) refreshGold.text = "0";
            else refreshGold.text = "1";
        }
        refreshBtn.onClick.AddListener(SetBonusRollEvent);
    }

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
    public void AddManaCycleEffect() => FieldManager.Instance.AddCoroutine(ManaCycle);
    void ManaCycle()
    {
        IEnumerator ManaCycleAugment()
        {
            while (true)
            {
                yield return new WaitForSeconds(5f);
                foreach(var piecelist in FieldManager.Instance.pieceDpList)
                {
                    if (piecelist.piece.isOwned == true)
                    {
                        if (piecelist.piece.maxMana < piecelist.piece.mana + 30) piecelist.piece.mana = piecelist.piece.maxMana;
                        else piecelist.piece.mana += 30;
                    }
                }
            }
        }
        FieldManager.Instance.StartCoroutine(ManaCycleAugment());
    }
    public void AddHealthRecoveryEffect() => FieldManager.Instance.AddCoroutine(HealthRecovery);
    void HealthRecovery()
    {
        IEnumerator HealthRecoveryAugment()
        {
            while (true)
            {
                yield return new WaitForSeconds(1f);
                foreach (var piecelist in FieldManager.Instance.pieceDpList)
                {
                    if (piecelist.piece.isOwned == true)
                    {
                        if (piecelist.piece.health + 20 > piecelist.piece.maxHealth) piecelist.piece.health = piecelist.piece.maxHealth;
                        else piecelist.piece.health += 20;
                    }
                }
            }
        }
        FieldManager.Instance.StartCoroutine(HealthRecoveryAugment());
    }
    public void AddShortBattleEffect() => FieldManager.Instance.AddCoroutine(ShortBattle);
    void ShortBattle()
    {
        IEnumerator ShortBattleAugment()
        {
            foreach(var pieceList in FieldManager.Instance.pieceDpList)
            {
                int _star = pieceList.piece.star;
                pieceList.piece.maxHealth = pieceList.piece.health + pieceList.piece.pieceData.health[_star];
                pieceList.piece.health = pieceList.piece.maxHealth;
                pieceList.piece.attackDamage = pieceList.piece.attackDamage + pieceList.piece.pieceData.attackDamage[_star];
                pieceList.piece.attackSpeed = pieceList.piece.attackSpeed + pieceList.piece.pieceData.attackSpeed[_star];
            }

            yield return new WaitForSeconds(15f);

            if (FieldManager.Instance.BattleResult != FieldManager.Result.VICTORY || FieldManager.Instance.BattleResult != FieldManager.Result.DEFEAT)
            {
                foreach (var pieceList in FieldManager.Instance.pieceDpList)
                {
                    pieceList.piece.DeadState();
                }
            }
        }
        FieldManager.Instance.StartCoroutine(ShortBattleAugment());
    }
    #endregion

    void Shuffle<T>(List<T> list)
    {
        System.Random random = new System.Random();

        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = random.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}
