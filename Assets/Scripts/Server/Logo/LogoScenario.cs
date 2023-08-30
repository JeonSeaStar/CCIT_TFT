using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ε� ���� �����ϴ� ��ũ��Ʈ
/// </summary>
public class LogoScenario : MonoBehaviour
{
    [SerializeField] private Progress progress;

    [SerializeField] private SceneNames nextScene;

    private void Awake()
    {
        SystemSetup();
    }

    private void SystemSetup()
    {
        //Ȱ��ȭ���� ���� ���¿����� ������ ��� ����
        Application.runInBackground = true;

        ////�ػ� ����
        //int width = Screen.width;
        //int height = (int)(Screen.width * 18.5f / 9);
        //Screen.SetResolution(width, height, true);

        //ȭ���� ������ �ʵ��� ����
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        // �ε� �ִϸ��̼� ����, ��� �Ϸ�� OnAfterProgress() �޼ҵ� ����
        progress.Play(OnAfterProgress);
    }

    private void OnAfterProgress()
    {
        Utils.LoadScene(nextScene);
    }
}
