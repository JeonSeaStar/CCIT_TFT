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
        if (other.gameObject.CompareTag("Player"))
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
        GameObject equipmentGameObject;
        Equipment equipment;

        Transform spaceTransform = null;

        foreach (var space in ArenaManager.Instance.fm[0].chest.itemChest)
        {
            if (!space.full)
            {
                equipmentGameObject = Instantiate(equipmentData.equipmentPrefab, transform.position, Quaternion.identity);
                equipment = equipmentGameObject.GetComponent<Equipment>();

                space.full = true;
                spaceTransform = space.equipmentSpace;

                equipment.equipmentData.InputChest(spaceTransform);

                Destroy(gameObject);
                break;
            }
            else
            {
                print("Ã¢°í °¡µæÂü");
            }
        }
    }
}
