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

    public void ActiveResultPopup(bool b)
    {
        if (b)
        {
            resultText.color = resultColor[0];
            resultText.text = result[0];
        }
        else
        {
            resultText.color = resultColor[1];
            resultText.text = result[1];
        }

        Invoke("ActivePopup", 3f);
    }

    private void ActivePopup()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void RestartStage()
    {
        SoundManager.instance.Play("BGM/Bgm_Battle_Default", SoundManager.Sound.Effect);
        SceneManagement.instance.SceneSwitching(false, "Battle");
    }

    public void MainScene()
    {
        //SoundManager.instance.Play("BGM/Bgm_Battle_Default", SoundManager.Sound.Effect); 메인씬 사운드로 경로 놓으면 됨
        SceneManagement.instance.SceneSwitching(false, "Main");
    }
}
