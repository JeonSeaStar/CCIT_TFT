using UnityEngine;
using BackEnd;

/// <summary>
/// �ڳ� ������ �����ϴ� ��ũ��Ʈ
/// </summary>
public class BackendManager : MonoBehaviour
{
    private void Awake()
    {
        // Update() �޼ҵ��� Backend.AsyncPoll(); ȣ���� ���� ������Ʈ�� �ı����� �ʴ´�
        DontDestroyOnLoad(gameObject);

        // �ڳ� ���� �ʱ�ȭ
        BackendSetup();
    }

    private void Update()
    {
        // ������ �񵿱� �޼ҵ� ȣ��(�ݹ� �Լ� Ǯ��)�� ���� �ۼ�
        // ���� : https://developer.thebackend.io/unity3d/guide/Async/AsyncFuncPoll/
        if (Backend.IsInitialized)
        {
            Backend.AsyncPoll();
        }
    }

    void BackendSetup()
    {
        var bro = Backend.Initialize(true);

        if (bro.IsSuccess())
        {
            //�ʱ�ȭ ���� �� statusCode 204 Success
            Debug.Log($"�ʱ�ȭ ���� : {bro}");
        }
        else
        {
            // �ʱ�ȭ ���� �� statusCode 400�� ���� ����
            Debug.LogError($"�ʱ�ȭ ���� : {bro}");
        }
    }
}

