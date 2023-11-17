using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThorPiece : Piece
{
    protected override void Attack()
    {
        if (mana <= 80 && target != null)
        {
            Skill();
            mana = 0;
            Invoke("NextBehavior", attackSpeed);
        }
        else
        {
            base.Attack();
        }
    }

    protected override void Skill()
    {
        if (star == 0)
        {
            AllPieceDamageSkill(attackDamage * 2.5f);
            StartCoroutine(AllPieceDamageTimeSkill(attackDamage * 0.3f, 5f));
        }
        else if (star == 1)
        {
            AllPieceDamageSkill(attackDamage * 3.5f);
            StartCoroutine(AllPieceDamageTimeSkill(attackDamage * 0.6f, 10f));
        }
        else if (star == 2)
        {
            AllPieceDamageSkill(attackDamage * 6f);
            StartCoroutine(AllPieceDamageTimeSkill(attackDamage, 30f));
        }
    }

    IEnumerator AllPieceDamageTimeSkill(float damage, float time)
    {
        for (int i = 0; i < time; i++)
        {
            List<Piece> _allPiece = fieldManager.enemyFilePieceList;
            foreach (var _Neigbor in _allPiece)
            {
                Piece _targets = _Neigbor.GetComponent<Piece>();
                if(_targets == null)
                {
                    Debug.Log("대상없음");
                }
                else
                {
                    _targets.SkillDamage(damage);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void AllPieceDamageSkill(float damage)
    {
        List<Piece> _allPiece = fieldManager.enemyFilePieceList;
        foreach (var _Neigbor in _allPiece)
        {
            Piece _targets = _Neigbor.GetComponent<Piece>();
            if(_targets == null)
            {
                Debug.Log("대상없음");
            }
            else
            {
                _targets.SkillDamage(damage);
            }
        }
    }
}
