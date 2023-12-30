using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultPopup : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public string[] result;
    public Color[] resultColor;
    public Image frameImage;
    public Sprite[] frameSprites;

    public void ActiveResultPopup(bool b)
    {
        if (b)
        {
            SoundManager.instance.Clear();
            SoundManager.instance.Play("UI/Eff_Stage_Win", SoundManager.Sound.Bgm);
            //resultText.color = resultColor[0];
            resultText.text = result[0];
            frameImage.sprite = frameSprites[0];
        }
        else
        {
            SoundManager.instance.Clear();
            SoundManager.instance.Play("UI/Eff_Stage_Lose", SoundManager.Sound.Bgm);
            //resultText.color = resultColor[1];
            resultText.text = result[1];
            frameImage.sprite = frameSprites[1];
        }

        Invoke("ActivePopup", 3f);
    }

    private void ActivePopup()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void RestartStage()
    {
        SoundManager.instance.Clear();
        SceneManagement.instance.SceneSwitching(false, "Battle");
    }

    public void MainScene()
    {
        SoundManager.instance.Clear();
        SceneManagement.instance.SceneSwitching(false, "Main");
    }
}
