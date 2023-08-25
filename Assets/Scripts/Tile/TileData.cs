using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TileType { test1, test2, test3, test4 }

[CreateAssetMenu(fileName = "TileData", menuName = "Scriptable Object/Tile Data", order = int.MaxValue)]
public class TileData : ScriptableObject
{
    [SerializeField]
    private bool isFull;
    [SerializeField]
    private Vector2 targetIndex; // 원정할때 가게 될 타일 배열

}
