using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect : MonoBehaviour
{
    public void StageSelectSwitch()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void SceneSwitching()
    {
        SoundManager.instance.Play("UI/Eff_Main_to_Ingame", SoundManager.Sound.Effect);
        SceneManagement.instance.SceneSwitching(false, "Battle");
    }
}
