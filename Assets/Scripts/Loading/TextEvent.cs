using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEvent : MonoBehaviour
{
    public LoadingScene loadingScene;

    public void NextText()
    {
        loadingScene.NextText();
        Destroy(gameObject);
    }
}
