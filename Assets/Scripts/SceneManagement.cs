using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    public static SceneManagement instance;
    public SwitchAnimation switchAnimation;
    public TapEffect tapEffect;
    public bool first;
    public bool changeScene;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        instance = this;

        SceneManager.sceneLoaded += LoadedsceneEvent;
    }

    private void LoadedsceneEvent(Scene scene, LoadSceneMode mode)
    {
        tapEffect.CameraStack();
    }

    public void SceneSwitching(bool b, string sceneName)
    {
        switchAnimation.SceneSwitching(b, sceneName);
    }

    public void LoadScene()
    {
        SceneManager.LoadScene("Loading");
        SoundManager.instance.Clear();
        //SoundManager.instance.Play("BGM/Bgm_Loading", SoundManager.Sound.Bgm);
    }
}
