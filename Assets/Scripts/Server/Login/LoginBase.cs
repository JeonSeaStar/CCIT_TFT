using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// ���� ���� �������� ����ϴ� UI���� �����ϴ� ��ũ��Ʈ
/// </summary>
public class LoginBase : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI testMessage;

    /// <summary>
    /// �޼��� ����, InputField ���� �ʱ�ȭ
    /// </summary>
    protected void ResetUI(params Image[] images)
    {
        testMessage.text = string.Empty;

        for (int i = 0; i < images.Length; ++i)
        {
            images[i].color = Color.white;
        }
    }

    //�Ű������� �ִ� ������ ���
    protected void SetMessage(string msg)
    {
        testMessage.text = msg;
    }

    //�Է� ������ �ִ� InputField ���� ����
    //������ ���� �޼��� ���
    protected void GuideForIncorrectlyEnteredData(Image image, string msg)
    {
        testMessage.text = msg;
        image.color = Color.red;
    }

    //�ʵ� ���� ����ִ��� Ȯ�� (image: �ʵ� ����,  field: ����, result: ��µ� ����)
    protected bool IsFieldDataEmpty(Image image, string field, string result)
    {
        if (field.Trim().Equals(""))
        {
            GuideForIncorrectlyEnteredData(image, $"\"{result}\" �ʵ带 ä���ּ���.");

            return true;
        }

        return false;
    }
}
