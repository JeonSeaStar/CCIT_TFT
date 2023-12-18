using UnityEngine;
using DG.Tweening;

public class CatCoin : MonoBehaviour
{
    [SerializeField] float radius;
    [SerializeField] float moveSpeed;

    private void Start()
    {
        FieldManager.Instance.catcoin.Add(this);
    }

    public void MoveCoin()
    {
        Vector3 targetPosition = FieldManager.Instance.readyTileList[2].transform.position;
        targetPosition.y += 0.5f;
        transform.DOMove(targetPosition, moveSpeed);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "ReadyTile")
        {
            Debug.Log(other.name);
        }
    }
}
