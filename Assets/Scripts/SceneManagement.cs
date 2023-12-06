using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement instance;
    public Animator sceneSwitchAnimator;
    private bool first;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;

        SceneManager.sceneLoaded += LoadedSceneEvent;
    }

    private void LoadedSceneEvent(Scene scene, LoadSceneMode mode)
    {
        if (SceneManager.GetActiveScene().name == "Loading")
        {
            sceneSwitchAnimator.SetTrigger("Common");
        }
        else
        {
            if (first)
                SceneSwitching(true, null);
            first = true;
        }
    }

    public void SceneSwitching(bool b, string sceneName)
    {
        if (b)
            sceneSwitchAnimator.SetTrigger("Open");
        else
        {
            sceneSwitchAnimator.SetTrigger("Close");
            LoadingScene.nextScene = sceneName;
        }
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("Loading");
    }
}
