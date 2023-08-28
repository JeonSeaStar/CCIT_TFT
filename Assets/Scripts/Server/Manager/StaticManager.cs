using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StaticManager : MonoBehaviour
{
    public static StaticManager Instance { get; private set; }
    public static BackendManager Backend { get; private set; }
    public static UIManager UI { get; private set; }

    //��� ������ ���Ǵ� ��ɵ��� ��Ƴ��� Ŭ����
    //�����Ŵ����� ���� ���� �����ϴ��� Ȯ�� �� ����
    private void Awake()
    {
        Init();
    }

    void Init()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);

        Backend = GetComponentInChildren<BackendManager>();
        UI = GetComponentInChildren<UIManager>();

        
    }

}
