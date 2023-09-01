//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    public static FieldManager instance;

    public GameObject testPiece;
    public List<PrivatePieceCount> privatePieceCount;

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

    public Dictionary<Piece.Mythology, int> SynergeMythology = new Dictionary<Piece.Mythology, int>()
    {
        { Piece.Mythology.NONE      ,0 },
        { Piece.Mythology.A         ,0 },
        { Piece.Mythology.B         ,0 },
        { Piece.Mythology.C         ,0 },
        { Piece.Mythology.D         ,0 },
        { Piece.Mythology.E         ,0 }
    };
    public Dictionary<Piece.Species, int> SynergeSpecies = new Dictionary<Piece.Species, int>()
    {
        { Piece.Species.NONE        ,0 },
        { Piece.Species.HAMSTER     ,0 },
        { Piece.Species.CAT         ,0 },
        { Piece.Species.DOG         ,0 },
        { Piece.Species.FROG        ,0 },
        { Piece.Species.RABBIT      ,0 },
    };
    public Dictionary<Piece.PlusSynerge, int> SynergePlusSynerge = new Dictionary<Piece.PlusSynerge, int>()
    {
        { Piece.PlusSynerge.NONE    ,0 },
        { Piece.PlusSynerge.A       ,0 },
        { Piece.PlusSynerge.B       ,0 },
        { Piece.PlusSynerge.C       ,0 },
        { Piece.PlusSynerge.D       ,0 },
        { Piece.PlusSynerge.E       ,0 }
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

    public int FindPieceList(Piece piece)
    {
        int listIndex = -1;
        for (int i = 0; i < privatePieceCount.Count; i++)
        {
            if (privatePieceCount[i].pieceName == piece.pieceName)
                listIndex = i;
        }

        return listIndex;
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

[System.Serializable]
public class PrivatePieceCount
{
    public string pieceName;
    public List<Piece> piecesList;
    public int[] star = new int[3];

    public void PieceCountUp(Piece piece)
    {
        if (FieldManager.instance.privatePieceCount[FieldManager.instance.FindPieceList(piece)].pieceName == pieceName)
        {
            star[piece.star]++;
            piecesList.Add(piece);
            FusionPiece(piece);
        }
    }

    public void PieceCountDown(Piece piece)
    {
        if (FieldManager.instance.privatePieceCount[FieldManager.instance.FindPieceList(piece)].pieceName == pieceName)
        {
            star[piece.star]--;
            piecesList.Remove(piece);
        }
    }

    void FusionPiece(Piece piece)
    {
        int fusionCondition = 1;
        Piece firstFusionTarget = null;
        Piece secondFusionTarget = null;
        bool canFusion = false;

        for (int i = 0; i < piecesList.Count; i++)
        {
            if (piecesList[i] != piece && piecesList[i].star == piece.star)
            {
                if (fusionCondition == 0)
                {
                    secondFusionTarget = piecesList[i];
                    canFusion = true;
                    break;
                }
                else if (fusionCondition == 1)
                {
                    firstFusionTarget = piecesList[i];
                    fusionCondition--;
                }
            }
        }

        if (canFusion)
        {
            piece.DestroyPiece();
            firstFusionTarget.DestroyPiece();
            secondFusionTarget.DestroyPiece();
            Fusion(piece);
        }
    }

    void Fusion(Piece piece)
    {
        Piece p = FieldManager.instance.SpawnPiece(FieldManager.instance.testPiece, ++piece.star);
        p.Owned();
    }
}