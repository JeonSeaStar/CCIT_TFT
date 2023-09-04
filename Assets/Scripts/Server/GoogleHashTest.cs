using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


using BackEnd;


public class GoogleHashTest : MonoBehaviour
{
    public TMP_InputField inputField;
    private void Awake()
    {
        // Update() 메소드의 Backend.AsyncPoll(); 호출을 위해 오브젝트를 파괴하지 않는다
        DontDestroyOnLoad(gameObject);

        // 뒤끝 서버 초기화
        BackendSetup();
    }

    private void Update()
    {
        // 서버의 비동기 메소드 호출(콜백 함수 풀링)을 위해 작성
        // 참고 : https://developer.thebackend.io/unity3d/guide/Async/AsyncFuncPoll/
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
            //초기화 성공 시 statusCode 204 Success
            Debug.Log($"초기화 성공 : {bro}");
        }
        else
        {
            // 초기화 실패 시 statusCode 400대 에러 생성
            Debug.LogError($"초기화 실패 : {bro}");
        }
    }

    public void GetGoogleHash()
    {
        string googleHashKey = Backend.Utils.GetGoogleHash();

        if(!string.IsNullOrEmpty(googleHashKey))
        {
            Debug.Log(googleHashKey);
            if(inputField != null)
            {
                inputField.text = googleHashKey;
            }
        }
    }
}  
