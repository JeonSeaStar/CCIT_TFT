using UnityEngine;
using DG.Tweening;

public class CatCoin : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [Header("¿Ãµø DoTween")] public Ease ease;
    
    private void Start()
    {
        FieldManager.Instance.catcoin.Add(this);
    }

    public void MoveCoin()
    {
        Vector3 targetPosition = FieldManager.Instance.readyTileList[2].transform.position;
        targetPosition.y += 0.5f;
        transform.DOMove(targetPosition, moveSpeed).SetEase(ease);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ReadyTile")
        {
            //gameObject.AddComponent<RectTransform>();
            //RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            //rectTransform.anchorMin = Vector2.one;
            //rectTransform.anchorMax = Vector2.one;
            //gameObject.transform.parent = FieldManager.Instance.pieceShop.gameObject.transform.parent;

            //transform.position = Vector3.MoveTowards(transform.position, FieldManager.Instance.pieceShop.text.transform.position, Time.deltaTime * 7.5f);
            Vector3 goldPosition = Camera.main.WorldToScreenPoint(FieldManager.Instance.playerState.currentMoneyText.transform.position);
            Vector3 goldPosition2 = Camera.main.ScreenToWorldPoint(goldPosition);
            //goldPosition.z = 0;
            //Debug.Log(goldPosition2);
            //Vector3 translatePosition = Camera.main.ScreenToWorldPoint(goldPosition);
            //Vector3 translatePosit2ion = Camera.main.ScreenToViewportPoint(goldPosition); Debug.Log(translatePosit2ion);
            //transform.DOMove(translatePosition, 10f);
            //transform.DOScale(Vector3.zero, 1f);
        }
    }
}
