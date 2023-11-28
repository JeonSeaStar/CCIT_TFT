using UnityEngine;
using DG.Tweening;
using TMPro;

public class DamageTextEffect : MonoBehaviour
{
    [SerializeField] private float destroyTime;
    [SerializeField] private Animator animator;
    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 offset2;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private float t;

    void Start()
    {
        Destroy(gameObject, destroyTime);

        transform.localPosition = new Vector3(Random.Range(offset.x - offset2.x, offset.x + offset2.x), Random.Range(offset.y - offset2.y, offset.y + offset2.y), Random.Range(offset.z, offset.z));
        Effect();
    }

    void Effect()
    {
        GetComponent<RectTransform>().DOMoveX(GetComponent<RectTransform>().position.x - t, 2).SetEase(Ease.InQuad);
        GetComponent<RectTransform>().DOMoveY(GetComponent<RectTransform>().position.y + t * 2f, 2).SetEase(Ease.OutQuad);
        text.DOColor(new Vector4(text.color.r, text.color.g, text.color.b, 0), 1.5f);
    }
}
