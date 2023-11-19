using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThorPiece : Piece
{
    public override IEnumerator Attack()
    {
        if (mana <= 80 && target != null)
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
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    IEnumerator AllPieceDamageTimeSkill(float damage, float time)
    {
        for (int i = 0; i < time; i++)
        {
            List<Piece> _allPiece = fieldManager.enemyFilePieceList;
            foreach (var _Neigbor in _allPiece)
            {
                Piece _targets = _Neigbor.GetComponent<Piece>();
                if (_targets == null)
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
            if (_targets == null)
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
