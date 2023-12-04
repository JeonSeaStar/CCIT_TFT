using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAnimation : MonoBehaviour
{
    public Animator sceneSwitchAnimator;

    private void Awake()
    {
        if (SceneManagement.instance.changeScene)
            SceneSwitching(true, null);
        else
            sceneSwitchAnimator.SetTrigger("Init");
    }

    private void Start()
    {
        SceneManagement.instance.switchAnimation = this;
    }

    public void SceneSwitching(bool b, string sceneName)
    {
        if (b)
        {
            sceneSwitchAnimator.SetTrigger("Open");
            SceneManagement.instance.changeScene = false;
        }
        else
        {
            sceneSwitchAnimator.SetTrigger("Close");
            SceneManagement.instance.changeScene = true;
            LoadingScene.nextScene = sceneName;
        }
    }

    public void LoadScene()
    {
        SceneManagement.instance.LoadScene();
    }
}
