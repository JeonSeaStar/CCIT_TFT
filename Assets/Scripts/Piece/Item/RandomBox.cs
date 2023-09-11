using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RandomBox : MonoBehaviour
{
    public enum Grade { NONE }
    public Grade equipmentGrade;
    [SerializeField] EquipmentData equipmentData;
    public float height = 1;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            OpenBox();
        }
    }

    public void CurveMove(List<Transform> targetPositions)
    {
        Vector3 startPosition = transform.localPosition;
        int randomPosition = Random.Range(0, targetPositions.Count);
        Vector3 targetPosition = targetPositions[randomPosition].position;
        Vector3 highPointPosition = new Vector3(startPosition.x + (targetPosition.x - startPosition.x) / 2, startPosition.y + height, startPosition.z + (targetPosition.z - startPosition.z) / 2);

        transform.DOPath(new[] { highPointPosition, startPosition, highPointPosition, targetPosition, highPointPosition, targetPosition }, 1, PathType.CubicBezier).SetEase(Ease.Linear);
    }

    void OpenBox()
    {
        print("아이템 박스를 엶");
        Destroy(gameObject);
    }
}
