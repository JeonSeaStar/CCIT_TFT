using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInformationData", menuName = "Scriptable Object/Enemy Information Data", order = int.MaxValue)]
public class EnemyInformationData : ScriptableObject
{
    [System.Serializable]
    public class EnemyInformation
    {
        public GameObject piece;
        public Vector2 spawnTile;
    }

    [System.Serializable]
    public class StageInformation
    {
        public List<EnemyInformation> enemyInformation;
    }

    public List<StageInformation> enemy;
}
