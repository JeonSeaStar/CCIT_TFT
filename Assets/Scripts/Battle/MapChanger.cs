using UnityEngine;

public class MapChanger : MonoBehaviour
{
    public Animator animator;
    public GameObject[] maps;
     
    public void ChangeMap(EnemyInformationData.MapType mapType)
    {
        for (int i = 0; i < maps.Length; i++)
            maps[i].SetActive(false);

        GetMap(mapType).SetActive(true);
    }

    private GameObject GetMap(EnemyInformationData.MapType mapType)
    {
        switch (mapType)
        {
            case EnemyInformationData.MapType.SNOW:
                return maps[0];
            case EnemyInformationData.MapType.DESERT:
                return maps[1];
            case EnemyInformationData.MapType.BEACH:
                return maps[2];
            case EnemyInformationData.MapType.FORESTDAY:
                return maps[3];
            case EnemyInformationData.MapType.FORESTNIGHT:
                return maps[4];
            case EnemyInformationData.MapType.GATE:
                return maps[5];
            case EnemyInformationData.MapType.TEMPLE:
                return maps[6];
            default:
                return null;
        }
    }

    public void MapChangeEvent()
    {
        FieldManager.Instance.NextRound();
    }
}
