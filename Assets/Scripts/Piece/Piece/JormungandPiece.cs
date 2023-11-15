using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JormungandPiece : Piece
{
    Tile skillCheckTile;
    PathFinding pathFinding;
    int time;
    protected override void Attack()
    {
        if (mana <= 50)
        {
            Skill();
            mana = 0;
        }
        else
        {
            base.Attack();
        }
    }

    protected override void Skill()
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
    }

    public void GetLocationMultiRangeSkill(float damage, int time)
    {
        skillCheckTile = targetTile;
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;
        FindNeighbor(damage, time);
    }

    IEnumerator FindNeighbor(float damage,int time)
    {
        for (int i = 0; i < time; i++)
        {
            List<Tile> _getNeigbor = pathFinding.GetNeighbor(skillCheckTile);
            _getNeigbor.Add(skillCheckTile);
            foreach (var _Neigbor in _getNeigbor)
            {
                Piece _targets = _Neigbor.GetComponent<Piece>();
                _targets.Damage(damage);
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
