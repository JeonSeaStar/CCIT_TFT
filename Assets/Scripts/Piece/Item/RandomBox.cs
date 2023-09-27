using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBox : MonoBehaviour
{
    public Chest.Grade grade;
    public Chest.BoxType boxType = Chest.BoxType.NONE;

    public EquipmentData equipmentData;
    public int money;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OpenBox();
        }
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

                //equipment.equipmentData.InputChest(spaceTransform);
                ArenaManager.Instance.fm[0].chest.CurveMove(equipmentGameObject.transform, spaceTransform);

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
