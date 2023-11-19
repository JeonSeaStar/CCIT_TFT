using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultPopup : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public string[] result;

    public void ActiveResultPopup(bool b)
    {
        if (b)
            resultText.text = result[0];
        else
            resultText.text = result[1];

        Invoke("ActivePopup", 3f);
    }

    private void ActivePopup()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}