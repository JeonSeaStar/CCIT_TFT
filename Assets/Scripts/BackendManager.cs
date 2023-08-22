using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

//뒤끝 SDK namespace 추가
using BackEnd;


public class BackendManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //뒤끝 초기화에 대한 응답값
        var bro = Backend.Initialize(true);

        if(bro.IsSuccess())
        {
            Debug.Log("초기화 성공 : " + bro); //성공일 경우 ststusCode 204 Success
        }
        else
        {
            Debug.LogError("초기화 실패 : " + bro); //실패일 경우 statusCode 400대 에러 발생
        }

        Test();
    }

    //동기 함수를 비동기에서 호출하게 해주는 함수(유니티 UI 접근 불가)
    async void Test()
    {
        await Task.Run(() =>
        {
            //BackendLogin.Instance.CustomSignUp("user1", "1234"); // [추가] 뒤끝 회원가입 함수
            BackendLogin.Instance.CustomLogin("user1", "1234");// [추가] 뒤끝 로그인

            //게임 정보 기는 구현 로직 추가

            Debug.Log("테스트를 종료합니다.");
        });
    }
}
