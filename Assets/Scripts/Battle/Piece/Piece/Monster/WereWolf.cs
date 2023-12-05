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
        if (mana >= pieceData.mana[star] && target != null)
        {
            StartSkill();
            mana = 0;
            yield return new WaitForSeconds(attackSpeed);
            StartNextBehavior();
        }
        else
        {
            SoundManager.instance.Play("Wolf_Series/S_Attack_Wolf", SoundManager.Sound.Effect);
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
        SoundManager.instance.Play("Wolf_Series/S_Skill_Were_Wolf", SoundManager.Sound.Effect);
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

    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("��� ������ {0}�� ���ظ� ������, �ֺ� ���鿡�� {1}�� ���ظ� �����ϴ�.", 1500, 700);
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Wolf_Series/S_Death_Were_Wolf", SoundManager.Sound.Effect);
        base.Dead();
    }
}
