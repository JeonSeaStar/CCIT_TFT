using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundState : MonoBehaviour
{
    [Header("라운드 정보 바")]
    public FieldManager fieldManager;

    public TextMeshProUGUI stageText;
    public TextMeshProUGUI roundText;

    public List<Image> roundIconImages;
    public List<RoundSprites> roundSprites;

    [Header("라운드 정보 팝업")]
    public GameObject roundInfoPopup;
    public TextMeshProUGUI stagePopupText;
    public TextMeshProUGUI roundPopupText;
    public TextMeshProUGUI victoryReward;
    public TextMeshProUGUI defeatReward;
    public Animator animator;

    public void NextRound(int round)
    {
        roundText.text = round.ToString();
    }

    public void SetStage(int stage)
    {
        stageText.text = stage.ToString();
    }

    public void InitRoundIcon()
    {
        for(int i = 0; i < roundIconImages.Count; i++)
            roundIconImages[i].sprite = GetRoundIcon(fieldManager.stageInformation.enemy[i].roundType, 0);
    }

    public void UpdateStageIcon(int currentRound, int updateSprite, string roundType)
    {
        roundIconImages[currentRound].sprite = GetRoundIcon(roundType, updateSprite);
    }

    private Sprite GetRoundIcon(string roundType, int updateSprite)
    {
        foreach(var roundSprite in roundSprites)
        {
            if (roundSprite.roundType == roundType)
                return roundSprite.roundSprites[updateSprite];
        }

        return null;
    }

    public void OnRoundPopup(int currentStage, int currentRound)
    {
        UpdateRoundPopup(currentStage, currentRound);

        animator.SetTrigger("OpenPopup");
    }

    public void OffRoundPopup()
    {
        animator.SetTrigger("ClosePopup");
    }

    private void UpdateRoundPopup(int currentStage, int currentRound)
    {
        stagePopupText.text = currentStage.ToString();
        roundPopupText.text = (currentRound + 1).ToString();
        victoryReward.text = fieldManager.stageInformation.enemy[currentRound].gold.ToString();
        defeatReward.text = fieldManager.stageInformation.enemy[currentRound].defeatGold.ToString();
    }
}

[System.Serializable]
public class RoundSprites
{
    public string roundType;
    public List<Sprite> roundSprites;
}
