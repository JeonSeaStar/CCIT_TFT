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
        SceneManagement.instance.SceneSwitching(false, "Battle");
    }
}
