using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [System.Serializable]
    public class EquipmentSpace
    {
        public Transform equipmentSpace;
        public bool full;
    }

    public EquipmentSpace chest;
}
