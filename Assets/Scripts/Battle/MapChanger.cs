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
            case EnemyInformationData.MapType.FORESTDAY:
                return maps[0];
            case EnemyInformationData.MapType.FORESTNIGHT:
                return maps[1];
            case EnemyInformationData.MapType.TEMPLEDAY:
                return maps[2];
            case EnemyInformationData.MapType.TEMPLEEVENING:
                return maps[3];
            default:
                return null;
        }
    }

    public void MapChangeEvent()
    {
        FieldManager.Instance.NextRound();
    }
}
