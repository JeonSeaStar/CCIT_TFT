//using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static ArenaManager;

public class FieldManager : MonoBehaviour
{
    public static FieldManager instance;

    public GameObject testPiece;
    public List<PrivatePieceCount> privatePieceCount;

    public List<Piece> myFilePieceList;
    public List<Piece> enemyFilePieceList;

    public GameObject[] readyZoneHexaIndicators;
    public GameObject[] battleFieldHexaIndicators;

    //public int getPieceCount = 0; // 구매해서 가지고 있는 기물 갯수
    //public int setPieceCount = 0; // 구매해서 배치한 기물 갯수

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

    public TMP_Text testTimer;

    void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        //testTimer.text = ArenaManager.instance.roundTime.ToString();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Piece piece = SpawnPiece(testPiece, 0);
            piece.Owned();
        }

        if(Input.GetKeyDown(KeyCode.S))
        {
            Debug.Log("SynergeMythology  A 의 활성화 수 = " + SynergeMythology[PieceData.Mythology.A] +
                      System.Environment.NewLine + 
                      "SynergeMythology  B 의 활성화 수 = " + SynergeMythology[PieceData.Mythology.B] +
                      System.Environment.NewLine +
                      "SynergeMythology  C 의 활성화 수 = " + SynergeMythology[PieceData.Mythology.C] +
                      System.Environment.NewLine +
                      "SynergeMythology  D 의 활성화 수 = " + SynergeMythology[PieceData.Mythology.D] +
                      System.Environment.NewLine +
                      "SynergeMythology  E 의 활성화 수 = " + SynergeMythology[PieceData.Mythology.E]);
        }

        if(Input.GetKeyDown(KeyCode.B))
        {
            ArenaManager.instance.roundType = RoundType.BATTLE;
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
        if(ArenaManager.instance.roundType == ArenaManager.RoundType.READY)
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
        if(ArenaManager.instance.roundType == ArenaManager.RoundType.BATTLE)
        {
            for(int i = 0; i < readyZoneHexaIndicators.Length; i++)
            {
                readyZoneHexaIndicators[i].SetActive(isactive);
            }
        }
    }

    public void ChangeRoundType(RoundType roundType)
    {
        if(roundType == RoundType.READY)
        {

        }
        else if(roundType == RoundType.BATTLE)
        {

        }
        else if(roundType == RoundType.EVENT)
        {

        }
        else if(roundType == RoundType.OVERTIME)
        {

        }
        else if(roundType == RoundType.DUEL)
        {

        }
        else if(roundType == RoundType.DEAD)
        {

        }
    }

    //IEnumerator RoundTimer(RoundType type)
    //{
    //    yield return new WaitForSeconds(1f);
    //    if(time > 0) time = time - 1;
    //    else if(time < 0)
    //    {
    //        time = 0;
    //        Debug.Log("Round Change");
    //    }
    //}
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