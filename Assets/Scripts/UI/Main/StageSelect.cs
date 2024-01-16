using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageSelect : MonoBehaviour
{
    public Transform enemyImageParent;
    public TextMeshProUGUI stage;

    public void StageSelectSwitch(string s)
    {
        gameObject.SetActive(!gameObject.activeSelf);
        stage.text = s;
    }

    public void StageSeletectOff()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void SceneSwitching()
    {
        SoundManager.instance.Play("UI/Eff_Main_to_Ingame", SoundManager.Sound.Effect);
        SceneManagement.instance.SceneSwitching(false, "Battle");
    }
}
