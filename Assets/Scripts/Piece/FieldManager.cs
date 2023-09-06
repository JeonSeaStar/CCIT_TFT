//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public static FieldManager instance;

    public GameObject testPiece;

    public List<Piece> myFilePieceList;
    public List<Piece> enemyFilePieceList;

    public PathFinding pathFinding;

    [Space(10)]
    //Jun
    public bool isBattle = false;
    public enum RoundType 
    {
        NONE = -1,
        READY,
        BATTLE ,
        EVENT ,
        OVERTIME,
        DUEL,
        DEAD ,
        MAX
    };
    public RoundType roundType = RoundType.NONE;

    public GameObject[] readyZoneHexaIndicators;
    public GameObject[] battleFieldHexaIndicators;

    public GameObject[] allPieces;// Tile 타입으로 재선언 할지도?
    // 여기다가 구매한 기물 전부 넣어주세용~

    public int getPieceCount = 0; // 구매해서 가지고 있는 기물 갯수
    public int setPieceCount = 0; // 구매해서 배치한 기물 갯수

    // 신화 시너지 인덱스
    public int aMythology = 0;
    public int bMythology = 0;
    public int cMythology = 0;
    public int dMythology = 0;
    public int eMythology = 0;
    // 종족 시너지 인덱스
    public int hamsterSpecies = 0;
    public int catSpecies = 0;
    public int dogSpecies = 0;
    public int frogSpecies = 0;
    public int rabbitSpecies = 0;
    // 추가 시너지 인덱스
    public int aPlusSynerge = 0;
    public int bPlusSynerge = 0;
    public int cPlusSynerge = 0;
    public int dPlusSynerge = 0;
    public int ePlusSynerge = 0;

    public Dictionary<PieceData.Mythology, int> SynergeMythology = new Dictionary<PieceData.Mythology, int>()
    {
        { PieceData.Mythology.NONE      ,0 },
        { PieceData.Mythology.A         ,0 },
        { PieceData.Mythology.B         ,0 },
        { PieceData.Mythology.C         ,0 },
        { PieceData.Mythology.D         ,0 },
        { PieceData.Mythology.E         ,0 }
    };
    public Dictionary<PieceData.Species, int> SynergeSpecies = new Dictionary<PieceData.Species, int>()
    {
        { PieceData.Species.NONE        ,0 },
        { PieceData.Species.HAMSTER     ,0 },
        { PieceData.Species.CAT         ,0 },
        { PieceData.Species.DOG         ,0 },
        { PieceData.Species.FROG        ,0 },
        { PieceData.Species.RABBIT      ,0 },
    };
    public Dictionary<PieceData.PlusSynerge, int> SynergePlusSynerge = new Dictionary<PieceData.PlusSynerge, int>()
    {
        { PieceData.PlusSynerge.NONE    ,0 },
        { PieceData.PlusSynerge.A       ,0 },
        { PieceData.PlusSynerge.B       ,0 },
        { PieceData.PlusSynerge.C       ,0 },
        { PieceData.PlusSynerge.D       ,0 },
        { PieceData.PlusSynerge.E       ,0 }
    };

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Piece piece = SpawnPiece(testPiece, 0);
            piece.Owned();
        }
    }
    int d = 0;
    public Piece SpawnPiece(GameObject p, int star)
    {
        GameObject pieceGameObject = Instantiate(p, new Vector3(0, 0, 0), Quaternion.identity);
        pieceGameObject.name = "testPiece[" + d + "]";
        pieceGameObject.TryGetComponent(out Piece piece);
        piece.star = star;
        d++;
        return piece;
    }

    /// <summary>
    /// Hexa Icon ON / OFF
    /// </summary>
    /// <param name="isactive"></param>
    public void ActiveHexaIndicators(bool isactive)
    {
        if(roundType == RoundType.READY)
        {
            for(int i = 0; i < readyZoneHexaIndicators.Length; i++)
            {
                readyZoneHexaIndicators[i].SetActive(isactive);
            }
            for (int i = 0; i < battleFieldHexaIndicators.Length; i++)
            {
                battleFieldHexaIndicators[i].SetActive(isactive);
            }
        }
        if(roundType == RoundType.BATTLE)
        {
            for(int i = 0; i < readyZoneHexaIndicators.Length; i++)
            {
                readyZoneHexaIndicators[i].SetActive(isactive);
            }
        }
    }
}