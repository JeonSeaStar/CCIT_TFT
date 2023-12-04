using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement instance;
    public SwitchAnimation switchAnimation;
    private bool first;
    public bool changeScene;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;
    }

    public void SceneSwitching(bool b, string sceneName)
    {
        switchAnimation.SceneSwitching(b, sceneName);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("Loading");
    }
}
