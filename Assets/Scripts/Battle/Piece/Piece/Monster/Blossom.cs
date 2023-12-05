using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blossom : Piece
{
    PathFinding pathFinding;
    [SerializeField] private GameObject hitEffect;
    public override IEnumerator Attack()
    {
        if (mana >= pieceData.mana[star])
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
        Damage(1500f);
        GetLocationMultiRangeSkill(currentTile, 700f);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }
    void GetLocationMultiRangeSkill(Tile tiles, float damage)
    {
        SkillState();
        pathFinding = ArenaManager.Instance.fieldManagers[0].pathFinding;
        Instantiate(skillEffects, new Vector3(transform.position.x, transform.position.y + 2.5f, transform.position.z), Quaternion.identity);
        List<Tile> _getNeigbor = pathFinding.GetNeighbor(tiles);
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
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("��� ������ {0}�� ���ظ� ������, �ֺ� ���鿡�� {1}�� ���ظ� �����ϴ�.", 1500, 700);
    }
}
