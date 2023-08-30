using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;


/// <summary>
/// Text UI ��ȣ�ۿ��� �����ϴ� ��ũ��Ʈ
/// </summary>
public class UITextInteraction : MonoBehaviour, IPointerClickHandler, IPointerExitHandler, IPointerEnterHandler
{
    [System.Serializable]
    private class OnClickEvent : UnityEvent { }

    //Text UI�� Ŭ������ �� ȣ���ϰ� ���� �޼ҵ� ���
    [SerializeField]
    private OnClickEvent onClickEvent;

    //������ �ٲ��, ��ġ�� �Ǵ� TextMeshProGUI
    private TextMeshProUGUI text;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        text.fontStyle = FontStyles.Bold;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        text.fontStyle = FontStyles.Normal;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClickEvent?.Invoke();
    }
}
