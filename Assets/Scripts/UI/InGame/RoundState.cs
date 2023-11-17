using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RoundState : MonoBehaviour
{
    public TextMeshProUGUI stageText;
    public TextMeshProUGUI roundText;

    public List<Image> roundIconImages;
    public List<Sprite> roundIconSprites;

    public void NextRound(int round)
    {
        roundText.text = round.ToString();
    }

    public void SetStage(int stage)
    {
        stageText.text = stage.ToString();
    }

    public void UpdateStageIcon(int currentRound)
    {
        
    }
}
