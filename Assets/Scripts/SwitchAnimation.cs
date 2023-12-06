using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchAnimation : MonoBehaviour
{
    public Animator sceneSwitchAnimator;

    private void Awake()
    {
        SceneManagement.instance.switchAnimation = this;

        if (!SceneManagement.instance.first)
        {
            gameObject.SetActive(false);
            SceneManagement.instance.first = true;
        }

        if (SceneManagement.instance.changeScene)
            SceneSwitching(true, null);
    }

    private void Start()
    {

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
            gameObject.SetActive(true);
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
