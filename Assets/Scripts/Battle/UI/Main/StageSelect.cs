using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageSelect : MonoBehaviour
{
    public float radius = 2;
    public List<GameObject> stageList;
    public GameObject parent;

    public void StageSelectSwitch()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }

    public void SceneSwitching()
    {
        SoundManager.instance.Play("UI/Eff_Main_to_Ingame", SoundManager.Sound.Effect);
        SceneManagement.instance.SceneSwitching(false, "Battle");
    }

    private void CirclePlace()
    {
        int numOfChild = stageList.Count;

        for (int i = 0; i < numOfChild; i++)
        {
            float angle = i * (Mathf.PI * 2.0f) / numOfChild;

            GameObject child = stageList[i];

            child.transform.position = parent.transform.position + (new Vector3(0, Mathf.Sin(angle), Mathf.Cos(angle))) * radius;
        }
    }
}
