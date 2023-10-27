using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using BackEnd;

/// <summary>
///  �ڳ� ������ ������ �α����� �����ϴ� ��ũ��Ʈ
/// </summary>
public class Login : LoginBase
{
    [SerializeField] private Image imageID;                //ID �ʵ� ���� ����
    [SerializeField] private TMP_InputField inputFieldID;  //ID �ʵ� �ؽ�Ʈ ���� ����
    [SerializeField] private Image imagePW;                //PW �ʵ� ���� ����
    [SerializeField] private TMP_InputField inputFieldPW;  //PW �ʵ� �ؽ�Ʈ ���� ����

    [SerializeField] private Button btnLogin;              //�α��� ��ư(��ȣ�ۿ� ����/�Ұ���)

    //�α��� ��ư�� ������ �� ȣ��
    public void OnClickLogin()
    {
        //�Ű������� �Է��� InputField UI ����� Message ���� �ʱ�ȭ
        ResetUI(imageID, imagePW);

        //�ʵ� ���� ����ִ��� üũ
        if (IsFieldDataEmpty(imageID, inputFieldID.text, "���̵�")) return;
        if (IsFieldDataEmpty(imagePW, inputFieldPW.text, "��й�ȣ")) return;

        //�α��� ��ư�� ��Ÿ���� ���ϵ��� ��ȣ�ۿ� ��Ȱ��ȭ
        btnLogin.interactable = false;

        // ������ �α����� ��û�ϴ� ���� ȭ�鿡 ����ϴ� ���� ������Ʈ
        // EX) �α��� ���� �ؽ�Ʈ ���, ��Ϲ��� ������ ȸ�� ��
        StartCoroutine(nameof(LoginProcess));

        //�ڳ� ���� �α��� �õ�
        ResponseToLogin(inputFieldID.text, inputFieldPW.text);
    }

    //�α��� �õ� �� �����κ��� ���޹��� Message�� ������� ���� ó��
    private void ResponseToLogin(string ID, string PW)
    {
        //������ �α��� ��û
        Backend.BMember.CustomLogin(ID, PW, callback =>
        {
            StopCoroutine(nameof(LoginProcess));

            //�α��� ����
            if (callback.IsSuccess())
            {
                SetMessage($"{inputFieldID.text}�� ȯ���մϴ�.");

                //��� ��Ʈ ������ �ҷ�����     %%��Ʈ �ҷ������ Ŭ���̾�Ʈ �α��� ���޿� ����� �� �ְ� ���� ���� ���߿� �����Ͱ� �ٲ� ���� ���⿡ 1ȸ�� �ҷ��´�
                BackendChartData.LoadAllChart();

                //�κ� ������ �̵�
                Utils.LoadScene(SceneNames.Lobby);
            }
            //�α��� ����
            else
            {
                //�α��ο� �������� ���� �ٽ� �α����� �ؾ��ϱ� ������ "�α���" ��ư ��ȣ�ۿ� Ȱ��ȭ
                btnLogin.interactable = true;

                string message = string.Empty;

                switch (int.Parse(callback.GetStatusCode()))
                {
                    case 401: //�������� �ʴ� ���̵�, �߸��� ��й�ȣ
                        message = callback.GetMessage().Contains("customId") ? "�������� �ʴ� ���̵��Դϴ�." : "�߸��� ��й�ȣ �Դϴ�.";
                        break;
                    case 403:
                        message = callback.GetMessage().Contains("user") ? "���� ���� �����Դϴ�." : "���ܴ��� �����Դϴ�.";
                        break;
                    case 410:
                        message = "Ż�� �������� �����Դϴ�.";
                        break;
                    default:
                        message = callback.GetMessage();
                        break;
                }

                //Status 401���� "�߸��� ��й�ȣ �Դϴ�." �� ��
                if (message.Contains("��й�ȣ"))
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

            SetMessage($"�α��� ���Դϴ�...{time:F1}");

            yield return null;
        }
    }
}