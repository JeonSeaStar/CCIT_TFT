using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StaticManager : MonoBehaviour
{
    public static StaticManager Instance { get; private set; }
    public static BackendManager Backend { get; private set; }
    public static UIManager UI { get; private set; }

    //모든 씬에서 사용되는 기능들을 모아놓은 클래스
    //각씬매니저가 현재 씬에 존재하는지 확인 후 생성
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
