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
        public MapType mapType;
        public string roundType;
        public List<EnemyInformation> enemyInformation;
        public int gold;
        public int defeatGold;
        public int defeatDamage;
    }

    public List<StageInformation> enemy;

    public enum MapType { FORESTDAY, FORESTNIGHT, TEMPLEDAY, TEMPLEEVENING}
}
