using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JormungandPiece : Piece
{
    public Tile skillCheckTile;
    public PathFinding pathFinding;
    int time;
    protected override IEnumerator Attack()
    {
        if (mana >= 50)
        {
            Skill();
            mana = 0;
            yield return new WaitForSeconds(attackSpeed);
            StartCoroutine(NextBehavior());
        }
        else
        {
            base.Attack();
        }
    }

    protected override IEnumerator Skill()
    {
        base.Skill();
        //Ÿ�� ���� �� Ÿ�� �˾ƿ��� ������ �ִ� �ǽ� 1�ʴ� �˾ƿ��� ������ �ֱ�
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
        StartCoroutine(NextBehavior());
    }

    public void GetLocationMultiRangeSkill(float damage, int time)
    {
        skillCheckTile = target.currentTile;
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;
        StartCoroutine(FindNeighbor(damage, time));
    }

    IEnumerator FindNeighbor(float damage,int time)
    {
        for (int i = 0; i < time; i++)
        {
            List<Tile> _getNeigbor = pathFinding.GetNeighbor(skillCheckTile);
            _getNeigbor.Add(skillCheckTile);
            foreach (var _Neigbor in _getNeigbor)
            {
                Piece _targets = _Neigbor.piece;
                
                if(_targets == null)
                {
                    Debug.Log("������");
                }
                else if(_targets.isOwned == false)
                {
                    _targets.SkillDamage(damage);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
