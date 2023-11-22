using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Synerge : MonoBehaviour
{
    [SerializeField] FieldManager fieldManager;
    public List<Piece> deletePieceList;

    //Methods Needs to be run once at the start of the battle round
    //public delegate void BattleEffect(bool isBattle);
    //BattleEffect sOnceBattleEffect;
    
    private void Start()
    {
        //BattleBuff(); 
        //sOnceBattleEffect(true);

        CoroutineBuff();
    }

    //void BattleBuff() // 2
    //{
    //    sOnceBattleEffect += GreatMountain;
    //    sOnceBattleEffect += SandKingdom;
    //    sOnceBattleEffect += Dog;
    //    sOnceBattleEffect += Frog;
    //    sOnceBattleEffect += UnderWorld;
    //    sOnceBattleEffect += Faddist;
    //}
    //public void GreatMountain(bool isBattle)
    //{
    //    var _greatMountainCount = fieldManager.mythActiveCount[PieceData.Myth.GreatMountain];
    //}
    //public void SandKingdom(bool isBattle)
    //{
    //    var _sandKingdomCount = fieldManager.mythActiveCount[PieceData.Myth.SandKingdom];
    //}
    //public void Dog(bool isBattle)
    //{
    //    var _dogCount = fieldManager.animalActiveCount[PieceData.Animal.Dog];
    //}
    //public void Frog(bool isBattle)
    //{
    //    var _FrogCount = fieldManager.animalActiveCount[PieceData.Animal.Frog];
    //}
    //public void UnderWorld(bool isBattle)
    //{
    //    var _underWorldCount = fieldManager.unitedActiveCount[PieceData.United.UnderWorld];
    //}
    //public void Faddist(bool isBattle)
    //{
    //    var _faddistCount = fieldManager.unitedActiveCount[PieceData.United.Faddist];
    //}

    //Methods Needs to be run multiple times during the battle round
    Coroutine[] sCoroutineEffect = new Coroutine[5];

    void CoroutineBuff()
    {
        sCoroutineEffect[0] = StartCoroutine(FrostyWind());
        sCoroutineEffect[1] = StartCoroutine(SandKingdom());
        sCoroutineEffect[2] = StartCoroutine(HeavenKingdom());
        sCoroutineEffect[3] = StartCoroutine(Hamster());
        sCoroutineEffect[4] = StartCoroutine(UnderWorld());
    }
    void StopCoroutineBuff()
    {
        foreach(var coroutineBuffCount in sCoroutineEffect) StopCoroutine(coroutineBuffCount);
    }

    IEnumerator FrostyWind()
    {
        var _frostyWindCount = fieldManager.mythActiveCount[PieceData.Myth.FrostyWind];
        float time = 0;
        //Debug.Log(3613);
        yield return new WaitForSeconds(time);
    }
    IEnumerator SandKingdom()
    {
        var _sandKingdomCount = fieldManager.mythActiveCount[PieceData.Myth.SandKingdom];
        float time = 0;

        yield return new WaitForSeconds(time);
    }
    IEnumerator HeavenKingdom()
    {
        var _heavenKingdomCount = fieldManager.mythActiveCount[PieceData.Myth.HeavenGround];
        float time = 0;

        yield return new WaitForSeconds(time);
    }
    IEnumerator Hamster()
    {
        var _hamsterCount = fieldManager.animalActiveCount[PieceData.Animal.Hamster];
        float time = 0;

        yield return new WaitForSeconds(time);
    }
    IEnumerator UnderWorld()
    {
        var _underWorldCount = fieldManager.unitedActiveCount[PieceData.United.UnderWorld];
        float time = 0;

        yield return new WaitForSeconds(time);
    }
}
