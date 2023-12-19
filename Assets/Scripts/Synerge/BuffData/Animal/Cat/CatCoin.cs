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
            Vector3 goldPosition = FieldManager.Instance.uiCamera.WorldToScreenPoint(FieldManager.Instance.playerState.currentMoneyText.transform.position);

            Ray ray = Camera.main.ScreenPointToRay(goldPosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity))
            {
                int _gold = Random.Range(2, 6);
                FieldManager.Instance.owerPlayer.gold += _gold;
                FieldManager.Instance.playerState.UpdateMoney(FieldManager.Instance.owerPlayer.gold);

                transform.DOMove(hit.point, 0.5f);
                transform.DOScale(Vector3.zero, 0.5f);
            }
        }
    }
}
