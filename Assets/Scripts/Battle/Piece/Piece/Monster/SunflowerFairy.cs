using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunflowerFairy : Piece
{
    Tile skillCheckTile;
    PathFinding pathFinding;
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
        GetLocationMultiRangeSkill(abilityPower * (1 + (abilityPowerCoefficient / 100)), 5);
        yield return new WaitForSeconds(attackSpeed);
        StartNextBehavior();
    }

    public void GetLocationMultiRangeSkill(float damage, int time)
    {
        if (dead)
            return;
        SkillState();
        SoundManager.instance.Play("FrostyWind/S_Jormungand", SoundManager.Sound.Effect);
        skillCheckTile = target.currentTile;
        pathFinding = FieldManager.Instance.pathFinding;
        Instantiate(skillEffects, target.currentTile.transform.position, Quaternion.identity);
        StartCoroutine(FindNeighbor(damage, time));
    }

    //2ĭ�̶�� �ߴµ� �̰� ���� �̾߱� �غ��� ��
    IEnumerator FindNeighbor(float damage, int time)
    {
        if (pieceState == State.DANCE)
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
                    Debug.Log("������");
                }
                else if (_targets.isOwned == true)
                {
                    Instantiate(skillEffects, _targets.transform.position, Quaternion.identity);
                    Damage(_targets, damage);
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }
    public override void SkillUpdateText()
    {
        pieceData.skillExplain = string.Format("{0}�� ���� ȸ�� ������ ������ �ֺ� 1ĭ ���� �� ��� ������ �ʴ� {1}�� ���ظ� �����ϴ�.", 5, abilityPower * (1 + (abilityPowerCoefficient / 100)));
    }

    public override void Dead()
    {
        SoundManager.instance.Play("Wolf_Series/S_Death_Were_Wolf", SoundManager.Sound.Effect);
        StopAllCoroutines();
        currentTile.InitTile();
        gameObject.SetActive(false);
    }
}
