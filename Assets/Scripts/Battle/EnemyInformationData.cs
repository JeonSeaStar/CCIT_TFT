using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInformationData", menuName = "Scriptable Object/Enemy Information Data", order = int.MaxValue)]
public class EnemyInformationData : ScriptableObject
{
    public enum EnemyImage
    {
        BUD, BLOOM, BLOSSOM,
        BOBM, POISIONBOBM, SNOWBOMB,
        DRAGONSPARK, DRAGONFIRE, DRAGONINFERNO,
        SNAKE, SNAKELET, SNAKENAGA,
        SHELL, SPIKE, HERMITKING,
        SUNBLOSSOM, SUNFLOWER, SUNFLORAPIXIE,
        WOLFPUP, WOLF, WEREWOLF,
        TARGETDUMMY, PRACTICEDUMMY, TRAININGDUMMY
    }

    public EnemyImage[] enemyImage = new EnemyImage[3];

    [System.Serializable]
    public class EnemyInformation
    {
        public GameObject piece;
        [Range(0, 2)]
        public int star;
        public Vector2 spawnTile;
    }

    [System.Serializable]
    public class ObstacleInformation
    {
        public GameObject Obstacle;
        public Vector2 spawnTile;
    }

    [System.Serializable]
    public class StageInformation
    {
        public MapType mapType;
        public string roundType;
        public List<EnemyInformation> enemyInformation;
        public List<ObstacleInformation> obstacleInformation;
        public int gold;
        public int defeatGold;
        public int defeatDamage;
    }

    public List<StageInformation> enemy;

    public enum MapType { SNOW, DESERT, BEACH, FORESTDAY, FORESTNIGHT, GATE, TEMPLE}
}
