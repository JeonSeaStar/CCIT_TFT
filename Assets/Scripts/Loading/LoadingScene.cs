using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] private Image progressBar;
    [SerializeField] private string[] textArray;
    [SerializeField] private GameObject textPrefab;

    public static LoadingScene instant;
    public static string nextScene;

    private void Start()
    {
        StartTextAnime();

        StartLoadScene();
    }

    public void StartLoadScene()
    {
        StartCoroutine(LoadScene());
    }

    public IEnumerator LoadScene()
    {
        yield return null;

        AsyncOperation op = SceneManager.LoadSceneAsync(nextScene);
        op.allowSceneActivation = false;

        float timer = 0.0f;
        while (!op.isDone)
        {
            yield return null;

            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, op.progress, timer);
                if (progressBar.fillAmount >= op.progress)
                { timer = 0f; }
            }
            else
            {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, 1f, timer);
                if (progressBar.fillAmount == 1.0f)
                {
                    yield return new WaitForSeconds(5f);
                    op.allowSceneActivation = true; yield break;
                }
            }
        }
    }

    public void StartTextAnime()
    {
        GameObject text = Instantiate(textPrefab, new Vector3(1300, -225, 0), Quaternion.identity, transform);
        text.transform.localPosition = new Vector3(1300, -225, 0);
        text.GetComponent<TextMeshProUGUI>().text = GetText();
        text.GetComponent<TextEvent>().loadingScene = this;
    }

    public void NextText()
    {
        StartTextAnime();
    }

    public string GetText()
    {
        int i = Random.Range(0, textArray.Length);

        return textArray[i];
    }
}
