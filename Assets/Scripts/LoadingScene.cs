using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    public static string nextScene;

    private void Start()
    {
        StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;

            //timer += Time.deltaTime;
            //if (op.progress < 0.9f)
            //{
            //    progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
            //    if (progressBar.fillAmount >= op.progress)
            //    { timer = 0f; }
            //}
            //else
            //{
            //    progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
            //    if (progressBar.fillAmount == 1.0f)
            //    { op.allowSceneActivation = true; yield break; }
            //}
            if(op.progress == 1)
                op.allowSceneActivation = true;
        }
    }
}
