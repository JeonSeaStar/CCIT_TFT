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
            #region 데이터 삽입, 불러오기, 업데이트 관련
            /*
            //BackendGameData.Instance.GameDataInsert();//데이터 삽입 함수

            //BackendGameData.Instance.GameDataGet(); //데이터 불러오기 함수

            BackendGameData.Instance.GameDataGet(); //데이터 삽입 함수

            //서버에서 불러온 데이터가 존재하지 않을 경우, 데이터를 새로 생성하여 삽입
            if(BackendGameData.userData == null)
            {
                BackendGameData.Instance.GameDataInsert();
            }

            BackendGameData.Instance.LevelUp(); //로컬에 저장된 데이터를 변경

            BackendGameData.Instance.GameDataUpdate(); //서버에 저장된 데이터를 덮어쓰기(변경된 부분만)
            */
            #endregion

            #region 랭킹 등록, 불러오기 관련
            /*
            //BackendRank.Instance.RankInsert(100); //랭킹 등록하기 함수

            BackendRank.Instance.RankGet(); //랭킹 불러오기 함수
            */
            #endregion

            #region 게임로그 저장 관련
            /*
            BackendGameLog.Instance.GameLogInsert(); //게임로그 저장 기능
            */
            #endregion

            #region 친구 요청, 불러오기 및 수락, 친구 리스트 불러오기



            #endregion
            Debug.Log("테스트를 종료합니다.");
        });
    }
}
