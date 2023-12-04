using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WereWolf : Piece
{
    private PathFinding pathFinding;
    [SerializeField] private GameObject hitEffect;

    //�䳢 �ó��� ó�� �ڷ� ���� �κ� �־�� ��
    public override IEnumerator Attack()
    {
        if (mana >= 100 && target != null)
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
        Instantiate(skillEffects, target.transform.position, Quaternion.identity);
        Damage(1500f);
        GetLocationMultiRangeSkill(700f);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void GetLocationMultiRangeSkill(float damage)
    {
        SkillState();
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;
        List<Tile> _getNeigbor = pathFinding.GetNeighbor(currentTile);
        foreach (var _Neigbor in _getNeigbor)
        {
            Piece _targets = _Neigbor.piece;
            if (_targets == null)
            {
                Debug.Log("������");
            }
            else if (_targets.isOwned)
            {
                Instantiate(hitEffect, _targets.transform.position, Quaternion.identity);
                Damage(_targets, damage);
            }
        }
    }
}
