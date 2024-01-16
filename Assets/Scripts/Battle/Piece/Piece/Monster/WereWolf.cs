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
            DoAttack();
        }
    }

    public override void DoAttack()
    {
        if (stun || freeze)
        {
            pieceState = State.IDLE;
            return;
        }
        if (target != null)
        {
            invincible = false;
            SoundManager.instance.Play("Wolf_Series/S_Attack_Were_Wolf", SoundManager.Sound.Effect);
            Damage(attackDamage);
            mana += manaRecovery;
            StartNextBehavior();
        }
        else
        {
            IdleState();
        }
    }

    public override IEnumerator Skill()
    {
        if (star == 0)
        {
            GetMultiSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
            Damage(1000f);
        }
        else if(star == 1)
        {
            GetMultiSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
            Damage(1200f);
        }
        else if(star == 2)
        {
            GetMultiSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)));
            Damage(1500f);
        }
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    void GetMultiSkill(float damage)
    {
        if (dead)
            return;
        SoundManager.instance.Play("Wolf_Series/S_Skill_Were_Wolf", SoundManager.Sound.Effect);
        SkillState();
        pathFinding = FieldManager.Instance.pathFinding;
        Instantiate(skillEffects, target.transform.position, Quaternion.identity);
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
        if (star == 0)
            pieceData.skillExplain = string.Format("��� ������ {0}�� ���ظ� ������, �ֺ� ���鿡�� {1}�� ���ظ� �����ϴ�.", 1000, abilityPower * (1 + (abilityPowerCoefficient / 100)));
        else if (star == 1)
            pieceData.skillExplain = string.Format("��� ������ {0}�� ���ظ� ������, �ֺ� ���鿡�� {1}�� ���ظ� �����ϴ�.", 1200, abilityPower * (1 + (abilityPowerCoefficient / 100)));
        else if (star == 2)
            pieceData.skillExplain = string.Format("��� ������ {0}�� ���ظ� ������, �ֺ� ���鿡�� {1}�� ���ظ� �����ϴ�.", 1500, abilityPower * (1 + (abilityPowerCoefficient / 100)));
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Wolf_Series/S_Death_Were_Wolf", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
