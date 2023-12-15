using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    private static TileManager instance;
    public static TileManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<TileManager>();
                if (instance == null)
                {
                    GameObject _arena = new GameObject();
                    _arena.name = "TileManager";
                    instance = _arena.AddComponent<TileManager>();
                    //DontDestroyOnLoad(_arena);
                }
            }
            return instance;
        }
    }

    [Header("�⹰ ��ġ ��ƼŬ")]public GameObject setPieceEffect;
    [Header("�⹰ �ռ� ��ƼŬ")]public GameObject fusionPieceEffect;
    [Header("�ܽ��� ��ġ ��ƼŬ")]public GameObject hamsterSpawnEffect;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void ActiveSetEffect(GameObject tilePosition)
    {
        Instantiate(setPieceEffect, tilePosition.transform.position, Quaternion.identity);
    }

    public void ActiveFusionEffect(GameObject tilePosition)
    {
        Instantiate(fusionPieceEffect, tilePosition.transform.position, Quaternion.identity);
    }

    public void ActiveHamsterSpawnEffect(GameObject tilePosition)
    {
        Instantiate(hamsterSpawnEffect, tilePosition.transform.position, Quaternion.identity);
    }
}
