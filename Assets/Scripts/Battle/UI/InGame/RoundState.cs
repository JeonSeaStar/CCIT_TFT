using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundState : MonoBehaviour
{
    public FieldManager fieldManager;

    public TextMeshProUGUI stageText;
    public TextMeshProUGUI roundText;

    public List<Image> roundIconImages;
    public List<RoundSprites> roundSprites;

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
}

[System.Serializable]
public class RoundSprites
{
    public string roundType;
    public List<Sprite> roundSprites;
}
