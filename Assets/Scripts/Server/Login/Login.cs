using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

/// <summary>
///  뒤끝 서버와 연동해 로그인을 제어하는 스크립트
/// </summary>
public class Login : LoginBase
{
    [SerializeField] private Image imageID;                //ID 필드 색상 변경
    [SerializeField] private TMP_InputField inputFieldID;  //ID 필드 텍스트 정보 추출
    [SerializeField] private Image imagePW;                //PW 필드 색상 변경
    [SerializeField] private TMP_InputField inputFieldPW;  //PW 필드 텍스트 정보 추출

    [SerializeField] private Button btnLogin;              //로그인 버튼(상호작용 가능/불가능)

    //로그인 버튼을 눌렀을 때 호출
    public void OnClickLogin()
    {
        //매개변수로 입력한 InputField UI 색상과 Message 내용 초기화
        ResetUI(imageID, imagePW);

        //필드 값이 비어있는지 체크
        if (IsFieldDataEmpty(imageID, inputFieldID.text, "아이디")) return;
        if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "비밀번호")) return;

        //로그인 버튼을 연타하지 못하도록 상호작용 비활성화
        btnLogin.interactable = false;

        // 서버에 로그인을 요청하는 동안 화면에 출력하는 내용 업데이트
        // EX) 로그인 관련 텍스트 출력, 톱니바퀴 아이콘 회전 등
        StartCoroutine(nameof(LoginProcess));

        //뒤끝 서버 로그인 시도
        ResponseToLogin(inputFieldID.text, inputFieldPW.text);
    }

    //로그인 시도 후 서버로부터 전달받은 Message를 기반으로 로직 처리
    private void ResponseToLogin(string ID, string PW)
    {
        //서버에 로그인 요청
        Backend.BMember.CustomLogin(ID, PW, callback =>
        {
            StopCoroutine(nameof(LoginProcess));

            //로그인 성공
            if (callback.IsSuccess())
            {
                SetMessage($"{inputFieldID.text}님 환영합니다.");

                //모든 차트 데이터 불러오기     %%차트 불러오기는 클라이언트 로그인 이휴에 사용할 수 있고 게임 실행 도중에 데이터가 바뀔 일이 없기에 1회만 불러온다
                BackendChartData.LoadAllChart();

                //로비 씬으로 이동
                Utils.LoadScene(SceneNames.Lobby);
            }
            //로그인 실패
            else
            {
                //로그인에 실패했을 때는 다시 로그인을 해야하기 때문에 "로그인" 버튼 상호작용 활성화
                btnLogin.interactable = true;

                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 401: //존재하지 않는 아이디, 잘못된 비밀번호
                        message = callback.GetMessage().Contains("customId") ? "존재하지 않는 아이디입니다." : "잘못된 비밀번호 입니다.";
                        break;
                    case 403:
                        message = callback.GetMessage().Contains("user") ? "차단 당한 유저입니다." : "차단당한 유저입니다.";
                        break;
                    case 410:
                        message = "탈퇴가 진행중인 유저입니다.";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;
                }

                //Status 401에서 "잘못된 비밀번호 입니다." 일 때
                if (message.Contains("비밀번호"))
                {
                    GuideForIncorrectlyEnteredData(imagePW, message);
                }
                else
                {
                    GuideForIncorrectlyEnteredData(imageID, message);
                }
            }
        });
    }
    private IEnumerator LoginProcess()
    {
        float time = 0;

        while (true)
        {
            time += Time.deltaTime;

            SetMessage($"로그인 중입니다...{time:F1}");

            yield return null;
        }
    }
}
