using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

//�ڳ� SDK namespace �߰�
using BackEnd;


public class BackendManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //�ڳ� �ʱ�ȭ�� ���� ���䰪
        var bro = Backend.Initialize(true);

        if(bro.IsSuccess())
        {
            Debug.Log("�ʱ�ȭ ���� : " + bro); //������ ��� ststusCode 204 Success
        }
        else
        {
            Debug.LogError("�ʱ�ȭ ���� : " + bro); //������ ��� statusCode 400�� ���� �߻�
        }

        Test();
    }

    //���� �Լ��� �񵿱⿡�� ȣ���ϰ� ���ִ� �Լ�(����Ƽ UI ���� �Ұ�)
    async void Test()
    {
        await Task.Run(() =>
        {
            //BackendLogin.Instance.CustomSignUp("user1", "1234"); // [�߰�] �ڳ� ȸ������ �Լ�
            BackendLogin.Instance.CustomLogin("user1", "1234");// [�߰�] �ڳ� �α���
            #region ������ ����, �ҷ�����, ������Ʈ ����
            /*
            //BackendGameData.Instance.GameDataInsert();//������ ���� �Լ�

            //BackendGameData.Instance.GameDataGet(); //������ �ҷ����� �Լ�

            BackendGameData.Instance.GameDataGet(); //������ ���� �Լ�

            //�������� �ҷ��� �����Ͱ� �������� ���� ���, �����͸� ���� �����Ͽ� ����
            if(BackendGameData.userData == null)
            {
                BackendGameData.Instance.GameDataInsert();
            }

            BackendGameData.Instance.LevelUp(); //���ÿ� ����� �����͸� ����

            BackendGameData.Instance.GameDataUpdate(); //������ ����� �����͸� �����(����� �κи�)
            */
            #endregion

            #region ��ŷ ���, �ҷ����� ����
            /*
            //BackendRank.Instance.RankInsert(100); //��ŷ ����ϱ� �Լ�

            BackendRank.Instance.RankGet(); //��ŷ �ҷ����� �Լ�
            */
            #endregion

            #region ���ӷα� ���� ����
            /*
            BackendGameLog.Instance.GameLogInsert(); //���ӷα� ���� ���
            */
            #endregion

            #region ģ�� ��û, �ҷ����� �� ����, ģ�� ����Ʈ �ҷ�����



            #endregion
            Debug.Log("�׽�Ʈ�� �����մϴ�.");
        });
    }
}
