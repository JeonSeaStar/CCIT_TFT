using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JormungandPiece : Piece
{
    public Tile skillCheckTile;
    public PathFinding pathFinding;
    int time;
    public override IEnumerator Attack()
    {
        if (mana >= 50)
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
        //타겟 지정 후 타일 알아오고 범위에 있는 피스 1초당 알아오고 데미지 주기
        if (star == 0)
        {
            GetLocationMultiRangeSkill(attackDamage * 0.8f, 6);
        }
        else if (star == 1)
        {
            GetLocationMultiRangeSkill(attackDamage * 1.25f, 6);
        }
        else if (star == 2)
        {
            GetLocationMultiRangeSkill(attackDamage * 4.25f, 10);
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void GetLocationMultiRangeSkill(float damage, int time)
    {
        skillCheckTile = target.currentTile;
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;
        Instantiate(skillEffects, target.currentTile.transform.position, Quaternion.identity);
        StartCoroutine(FindNeighbor(damage, time));
    }

    IEnumerator FindNeighbor(float damage, int time)
    {
        if(pieceState == State.DANCE)
        {
            yield break;
        }
        for (int i = 0; i < time; i++)
        {
            List<Tile> _getNeigbor = pathFinding.GetNeighbor(skillCheckTile);
            _getNeigbor.Add(skillCheckTile);
            foreach (var _Neigbor in _getNeigbor)
            {
                Piece _targets = _Neigbor.piece;

                if (_targets == null)
                {
                    Debug.Log("대상없음");
                }
                else if (_targets.isOwned == false)
                {
                    Instantiate(skillEffects, _targets.transform.position, Quaternion.identity);
                    _targets.SkillDamage(damage);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
