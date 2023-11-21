using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OdinPiece : Piece
{
    public override IEnumerator Attack()
    {
        if (mana <= 99)
        {
            StartSkill();
            mana = 0;
            yield return new WaitForSeconds(attackSpeed);
            StartNextBehavior();
        }
        else
        {
            DoAttack();
        }
    }

    public override IEnumerator Skill()
    {
        if (star == 0)
        {
            AllPieceBuffSkill(1.3f);
        }
        else if (star == 1)
        {
            AllPieceBuffSkill(1.6f);
        }
        else if (star == 2)
        {
            AllPieceBuffSkill(1.99f);
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void AllPieceBuffSkill(float buff)
    {
        List<Piece> _allPiece = fieldManager.myFilePieceList;
        foreach (var _Neigbor in _allPiece)
        {
            Piece _targets = _Neigbor.GetComponent<Piece>();
            if (_targets == null)
            {
                Debug.Log("대상없음");
            }
            else if (_targets.isOwned)
            {
                Instantiate(skillEffects, _targets.transform.position, Quaternion.identity);
                //ArenaManager.Instance.fieldManagers[0].AddBattleStartEffect(Test);
                _targets.attackDamage = attackDamage * buff;
            }
        }
    }

    //void Test(bool isAdd)
    //{
    //    if (!isAdd)
    //    {
    //        List<Piece> _allPiece = fieldManager.myFilePieceList;
    //        foreach (var _Neigbor in _allPiece)
    //        {
    //            Debug.Log(_Neigbor + "이새끼 스텟 다시 내려주기");
    //        }
    //        ArenaManager.Instance.fieldManagers[0].RemoveBattleStartEffect(Test); // 전투 끝나고 값들 초기화 실행해 줄때
    //        ArenaManager.Instance.fieldManagers[0].AddCoroutine(Test2); // 실행중이던 몇초동안 일어나고 있던 비동기 함수들 멈춰줄때
    //    }
    //}
    //void Test2()
    //{
    //    ArenaManager.Instance.fieldManagers[0].StartCoroutine(Test3());
    //}

    //IEnumerator Test3()
    //{
    //    yield return new WaitForSeconds(12f);
    //}
}
