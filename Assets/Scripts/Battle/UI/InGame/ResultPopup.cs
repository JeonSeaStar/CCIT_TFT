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
        SceneManagement.instance.SceneSwitching(false, "Battle");
    }

    public void MainScene()
    {
        SceneManagement.instance.SceneSwitching(false, "Main");
    }
}
